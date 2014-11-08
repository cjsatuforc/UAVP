// ==============================================
// =      U.A.V.P Brushless UFO Controller      =
// =           Professional Version             =
// = Copyright (c) 2007 Ing. Wolfgang Mahringer =
// ==============================================
//
//  This program is free software; you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation; either version 2 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License along
//  with this program; if not, write to the Free Software Foundation, Inc.,
//  51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
//
// ==============================================
// =  please visit http://www.uavp.de           =
// =               http://www.mahringer.co.at   =
// ==============================================


// CC5X Compiler parameters:
// -CC -fINHX8M -a -L -Q -V -FM -DMATHBANK_VARS=bank0 -DMATHBANK_PROG=2


#ifdef ICD2_DEBUG
#pragma	config = 0x377A	// BODEN, HVP, no WDT, MCLRE disabled, PWRTE disabled
#else
#pragma	config = 0x3F72	// BODEN, HVP, no WDT, MCLRE disabled, PWRTE enabled
#endif

#include "c-ufo.h"
#include "bits.h"

#pragma codepage = 0

// Math Library
#include "mymath16.h"

// EEPROM-Data can currently not be used with relocatable assembly
// CC5X limitation!
// #define EEPROM_START	0x2100
// #pragma cdata[EEPROM_START] = 0,0,0,0,0,0,0,0,0,0,20,8,16

// The globals

uns8	IGas;			// actual input channel, can only be positive!
int 	IRoll,INick,ITurn;	// actual input channels, 0 = neutral
uns8	IK5;		// actual channel 5 input
uns8	IK6;		// actual channel 6 input
uns8	IK7;		// actual channel 7 input
uns8	nisampcnt;

// PID Regler Variablen
int		RE,NE,TE;	// Fehlersignal aus dem aktuellen Durchlauf
int		REp,NEp,TEp;	// Fehlersignal aus dem vorigen Durchlauf (war RE/NE/TE)
int		TEp2;	// Fehlersignal aus dem zweitvorigen Durchlauf (war TEp)
long	YawSum;	// Integrales Fehlersignal fuer Turn, 0 = neutral
long	RollSum, NickSum;	// Integrales Fehlersignal fuer Roll und Nick
uns16	RollSamples, NickSamples;
long	LRSum, FBSum, UDSum;	// Integrales Fehlersignal des LISL Sensors (=Geschwindigkeit)
int		LRIntKorr, FBIntKorr;
uns8	NeutralLR, NeutralFB, NeutralUD;
long	LRSumPosi, FBSumPosi;	// Integral des Integrals des Fehlersignal des LISL Sensors (=Geschwindigkeit)

int		NegFact; // general purpose

uns8 BlinkCount;

// Die Reihenfolge dieser Variablen MUSS gewahrt bleiben!!!!
int	RollPropFactor;
int	RollIntFactor;
int	RollDiffFactor;
int RollLimit;
int RollIntLimit;

int	NickPropFactor;
int	NickIntFactor;
int	NickDiffFactor;
int NickLimit;
int NickIntLimit;

int	TurnPropFactor;
int	TurnIntFactor;
int	TurnDiffFactor;
int	YawLimit;
int YawIntLimit;

int	ConfigParam;
int	TimeSlot;
int LowVoltThres;

int	LinLRIntFactor;
int	LinFBIntFactor;
int	LinUDIntFactor;
int MiddleUD;
int MotorLowRun;
int	MiddleLR;
int MiddleFB;
int	CamIntFactor;
// Ende Reihenfolgezwang

uns16	MidRoll, MidNick, MidTurn;

#ifdef BOARD_3_1
uns8	LedShadow;	// shadow register
#endif

uns8	MVorne,MLinks,MRechts,MHinten;	// output channels
uns8	MCamRoll,MCamNick;
long	Ml, Mr, Mv, Mh;
long	Rl,Nl,Tl;	// PID output values
long	Rp,Np,Tp;
long	Vud;

uns8	Flags;

uns8	IntegralCount;
int		RollNeutral, NickNeutral;



void main(void)
{
	uns8	DropoutCount;
	int		nii@NegFact;
	uns8	LowGasCount;

	OPTION_REG = 0b.10000.011;	// TMR0 w/ int clock, 1:16 presc (see _PreScale0),
					// weak pullups off
// general ports setup
	TRISA = 0b.00111111;	// all inputs
	ADCON1 = 0b.0.000.0010;	// uses 5V as Vref

#ifdef BOARD_3_0
	PORTB = 0b.1000.0000;		// all outputs to low, except RB7 (LISL-CS)!
	TRISB = 0b.0000.0000;	// all servo and LED outputs
	PORTC = 0b.0100.0000;		// all outputs to low, except TxD
	TRISC = 0b.10000100;	// RC7, RC2 are inputs
#endif
#ifdef BOARD_3_1
	PORTB = 0b.1100.0000;		// all outputs to low, except RB6 & 7 (I2C)!
	TRISB = 0b.1100.0000;	// all servo and LED outputs
	PORTC = 0b.0110.0000;		// all outputs to low, except TxD and CS
	TRISC = 0b.10000100;	// RC7, RC2 are inputs
#endif

#ifdef BOARD_3_1
	LedShadow = 0;
#endif

    ALL_LEDS_OFF;

// timer setup
	T1CON = 0b.00.11.0001;	// enable TMR1, 1:8 prescaler, run with 2us clock
	CCP1CON = 0b.00.00.0100;	// capture mode every falling edge

	TMR2 = 0;
	T2CON = 0b.0.1111.1.11;	// enable TMR2, 1:16 prescaler, 1:16 postscaler (see _Pre/PostScale2)
	PR2 = TMR2_5MS;		// set compare reg to 9ms

// setup flags register
	Flags = 0;
	_NoSignal = 1;		// assume no signal present

	LedRed_ON;		// red LED on
	Delaysec(1);	// wait 1/10 sec until LISL is ready to talk
#ifdef USE_ACCSENS
	IsLISLactive();
#ifdef ICD2_DEBUG
	_UseLISL = 1;	// because debugger uses RB7 (=LISL-CS) :-(
#endif
	NeutralLR = 0;
	NeutralFB = 0;
	NeutralUD = 0;
	if( _UseLISL )
		GetEvenValues();	// into Rp, Np, Tp
#endif

// setup serial port for 8N1
	TXSTA = 0b.0010.0100;	// async mode, BRGH = 1
	RCSTA = 0b.1001.0000;	// receive mode
	SPBRG = _B38400;
#ifdef BOARD_3_0
	_SerEnabled = 1;	// serial link is enabled
#endif
	W = RCREG;			// be sure to empty FIFO

// enable the interrupts
	CCP1IE = 1;
	TMR2IE = 1;		// T1-Capture and T2 interrupt enable
		
	PEIE = 1;		// enable peripheral interrupts


// send hello text to serial COM
	Delaysec(1);	// just to see the output after powerup

	ShowSetup(1);

	IK6 = IK7 = _Neutral;

Restart:
	IGas =IK5 = _Minimum;	// assume parameter set #1

// DON'T MOVE THE UFO!
// ES KANN LOSGEHEN!

	while(1)
	{
		ALL_LEDS_OFF;
		LedRed_ON;		// red LED on
		if(_UseLISL)
			LedYellow_ON;	// to signal LISL sensor is active

		InitArrays();

		GIE = 1;		// enable all interrupts

// Wait until a valid RX signal is received

		DropoutCount = MODELLOSTTIMER;
		do
		{
			Delaysec(2);	// wait 2/10 sec until signal is there
			ProcessComCommand();
			if( _NoSignal )
			{
				if( Switch )
				{
					if( --DropoutCount == 0 )
					{
						Beeper_TOG;	// toggle beeper "model lost"
						DropoutCount = MODELLOSTTIMERINT;
					}
				}
				else
					Beeper_OFF;
			}
		}
		while( _NoSignal || !Switch);	// no signal or switch is off

// RX Signal is OK now
// Wait 2 sec to allow enter of prog mode
		LedRed_OFF;
		LedYellow_ON;	
		Delaysec(20);
		LedYellow_OFF;

// die Variablen einlesen
		ReadEEdata();
		
// check for prog mode (max. throttle)
		if( IGas > _ProgMode ) 
		{
			DoProgMode();
			goto Restart;
		}

// Just for safety: don't let motors start if throttle is open!
// check if Gas is below _ThresStop

		DropoutCount = 1;
		while( IGas >= _ThresStop )
		{
			if( _NoSignal )
				goto Restart;

			if( _NewValues )
			{
				OutSignals();
				_NewValues = 0;
				if( --DropoutCount <= 0 )
				{
					LedRed_TOG;	// toggle red Led 
					DropoutCount = 10;		// to signal: THROTTLE OPEN
				}
			}
			ProcessComCommand();
		}

// ###############
// ## MAIN LOOP ##
// ###############
// loop length is controlled by a programmable variable "TimeSlot"
// which sets wait time in ms
// standard ESCs will need at least 9 or 10 as TimeSlot.
		DropoutCount = 0;
		BlinkCount = 0;
		IntegralCount = 32;	// do 32 cycles to find integral zero point

		while(Switch == 1)	// as long as power switch is ON
		{
// wait pulse pause delay time (TMR0 has 1024us for one loop)
			TMR0 = 0;
			nisampcnt = 0;
			RollSamples =
			NickSamples = 0;
// sample gyro data and add everything up while waiting for timing delay
			if( !_Flying )
				TimeSlot++;	// to compensate for calulation delays

			while( TimeSlot > 0 )
			{
				while( T0IF == 0 ) 	// 1024us wait
				{	
					if( nisampcnt < 64 )
					{
						nisampcnt++;	// 1 sample is 200us
						GetGyroValues();
					}
				}
				T0IF = 0;
				TimeSlot--;
			}
			if( nisampcnt & 0x01 )	// odd value?
			{
				nisampcnt++;	// one more sample to get even numbers
				GetGyroValues();
			}
			ReadEEdata();

// Setup Blink counter
			if( BlinkCount == 0 )
				BlinkCount = BLINK_LIMIT;
			BlinkCount--;

// get the gyro values and Batt status
			CalcGyroValues();
			GetVbattValue();
			if( _LowBatt && (BlinkCount < BLINK_LIMIT/2) )
			{
				Beeper_OFF;
			}
			else
			{
				if( _LowBatt )
				{
					Beeper_ON;
				}
				else
				{
					Beeper_OFF;
				}
			}

// check for signal dropout while in flight
			if( _NoSignal && _Flying )
			{
				DropoutCount++;
				if( DropoutCount < MAXDROPOUT )
				{	// use last Gas
					ALL_LEDS_OFF;
					IRoll = 0;
					INick = 0;
					ITurn = 0;
					goto DoPID;
				}
				break;	// timeout, stop everything
			}
// allow motors to run on low throttle 
// even if stick is at minimum for a short time
			if( _Flying && (IGas <= _ThresStop) )
			{
				if( --LowGasCount > 0 )
					goto DoPID;
			}

			if( _NoSignal || 
			    ( (_Flying && (IGas <= _ThresStop)) ||
			      (!_Flying && (IGas <= _ThresStart)) ) )
			{	// UFO is landed, stop all motors
				IntegralCount = 32;	// do 32 cycles to find integral zero point
				InitArrays();	// resets _Flying flag!
				MidRoll = 0;	// gyro neutral points
				MidNick = 0;
				MidTurn = 0;
				RollNeutral = 	// stick neutral points
				NickNeutral = 0x7F;
				if( _NoSignal && Switch )	// _NoSignal set, but Switch is on?
				{
					break;	// then RX signal was lost
				}
				ALL_LEDS_OFF;
				LedGreen_ON;

#ifdef BOARD_3_0
				if( !_SerEnabled )
				{
					SPEN = 1;	// enable RS232
					CREN = 1;
					_SerEnabled = 1;
				}
#endif
				ProcessComCommand();
			}
			else
			{	// UFO is flying!
				if( !_Flying )	// about to start
				{	// set current gyro values as midpoints
#ifdef BOARD_3_0
// check if LISL Sensor is available
					if( _UseLISL && _SerEnabled )
					{
						SPEN = 0;	// disable RS232 to allow LISL sensor to work
						CREN = 0;
						TRISC.7 = 1;	// RC7 is input (data)
						TRISC.6 = 0;	// RC6 is output (clock)
						_SerEnabled = 0;
					}

// if no LISL available, do COM commands also (important if you set ConfigParam wrong!)
					if( _SerEnabled )
#endif
						ProcessComCommand();
				}

				_Flying = 1;
				DropoutCount = 0;
				LowGasCount = 100;
// LED game 
#ifndef NOLEDGAME
// do the LED game :-)
				ALL_LEDS_OFF;
#define LED_SHOW_LIM	2
				if( !_LowBatt )
				{
					nii = IRoll-RollNeutral;
					if( nii >  LED_SHOW_LIM )
					{
						LedRed_ON;
					}
					if( nii < -LED_SHOW_LIM )
					{
						LedGreen_ON;
					}
					x = INick-NickNeutral;
					if( nii >  LED_SHOW_LIM )
					{
						LedBlue_ON;
					}
					if( nii < -LED_SHOW_LIM )
					{
						LedYellow_ON;
					}
					if( ARE_ALL_LEDS_OFF )
						ALL_LEDS_ON;
				}
#endif
DoPID:
// do the calculations
				if( RollNeutral == 0x7F)  
					RollNeutral = IRoll;
				if( NickNeutral == 0x7F)
					NickNeutral = INick;

				Rp = 0;
				Np = 0;
				if(	IntegralCount > 0 )
					IntegralCount--;
				else
				{
					PID();
					MixAndLimit();
				}

// remember old gyro values
				TEp2 = TEp;
				REp = RE;
				NEp = NE;
				TEp = TE;
			}
			// Output the results to the speed controllers
			MixAndLimitCam();
			OutSignals();

		}	// END NORMAL OPERATION WHILE LOOP

// CPU kommt hierher wenn der Schalter ausgeschaltet wird
#ifdef BOARD_3_0
		SPEN = 1;	// enable RS232
		CREN = 1;
		_SerEnabled = 1;
#endif
	}
	ALL_OUTPUTS_OFF;
}

