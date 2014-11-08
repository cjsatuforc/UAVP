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

// C-Ufo Header File

// ==============================================
// == Gloal compiler switches
// ==============================================

// use this to allow debugging with MPLAB's Simulator
// #define SIMU
//
// select your board version here
#define BOARD_3_0
//#define BOARD_3_1
//
// if you want the led outputs for test purposes
#define NOLEDGAME
//
// To enable output of values via Out(), OutG() and OutSSP()
//#define DEBUG
//
// Only one of the following 2 defines must be activated:
// When using 3 ADXRS300 gyros
//#define OPT_ADXRS
// When using 1 ADXRS300 and 1 IDG300 gyro
#define OPT_IDG
//
// Enable this to use the Accelerator sensors
#define USE_ACCSENS

//
#ifdef BOARD_3_0
#define Version	"3.05"
#endif
#ifdef BOARD_3_1
#define Version	"3.10"
#endif


// ==============================================
// == External variables
// ==============================================

extern	shrBank	uns8	IGas;
extern	shrBank	int 	IRoll,INick,ITurn;
extern	shrBank	uns8	IK5,IK6,IK7;
extern	shrBank uns8	nisampcnt;

extern	bank2	int		RE, NE, TE;
extern	bank0	int		REp,NEp,TEp;
extern	bank2	long	YawSum;
extern	bank2	long	NickSum, RollSum;
extern	bank2	uns16	RollSamples, NickSamples;
extern	bank2	long	LRSum, FBSum, UDSum;
extern	bank2	int		LRIntKorr, FBIntKorr;
extern	bank2	uns8	NeutralLR, NeutralFB, NeutralUD;

extern	bank1	long	LRSumPosi, FBSumPosi;
extern	bank1	int		NegFact; // general purpose

extern	bank1	uns8	BlinkCount;

// Die Reihenfolge dieser Variablen MUSS gewahrt bleiben!!!!
extern	bank1	int	RollPropFactor; // controller trimming
extern	bank1	int	RollIntFactor;
extern	bank1	int	RollDiffFactor;
extern	bank1	int RollLimit;
extern	bank1	int	RollIntLimit;

extern	bank1	int	NickPropFactor; // controller trimming
extern	bank1	int	NickIntFactor;
extern	bank1	int	NickDiffFactor;
extern	bank1	int NickLimit;
extern	bank1	int	NickIntLimit;

extern	bank1	int	TurnPropFactor; // controller trimming
extern	bank1	int	TurnIntFactor;
extern	bank1	int	TurnDiffFactor;
extern	bank1	int	YawLimit;
extern	bank1	int YawIntLimit;

extern	bank1	int	ConfigParam;
extern	bank1	int TimeSlot;	// output ESC timing in ms
extern	bank1	int	LowVoltThres;

extern	bank1	int	LinLRIntFactor;
extern	bank1	int	LinFBIntFactor;
extern	bank1	int	LinUDIntFactor;
extern	bank1	int MiddleUD;	// counts 
extern	bank1	int	MotorLowRun;	// unused
extern	bank1	int	MiddleLR;
extern	bank1	int	MiddleFB;
extern	bank1	int	CamIntFactor;

int FirstProgReg @RollPropFactor;
int	LastProgReg @CamIntFactor;


extern	bank1	uns8	MVorne,MLinks,MRechts,MHinten;	// output channels
extern	bank1	uns8	MCamRoll,MCamNick;
extern	bank1	long	Ml, Mr, Mv, Mh;
extern	bank1	long	Rl,Nl,Tl;	// PID output values
extern	bank1	long	Rp,Np,Tp,Vud;


extern	shrBank	uns8	Flags;
extern	shrBank	uns8	RecFlags;	// Interrupt save registers for FSR

extern	shrBank	uns8	IntegralCount;

// measured neutral gyro values
// current stick neutral values
extern	bank1	int		RollNeutral, NickNeutral;

extern	bank2	uns16	MidRoll, MidNick, MidTurn;

#ifdef BOARD_3_1
extern	shrBank	uns8	LedShadow;	// shadow register
#endif

#define _ClkOut		(160/4)	/* 16.0 MHz quartz */
#define _PreScale0	16	/* 1:16 TMR0 prescaler */
#define _PreScale1	8	/* 1:8 TMR1 prescaler */
#define _PreScale2	16
#define _PostScale2	16

// wegen dem dummen Compiler muss man händisch rechnen :-(
//#define TMR2_9MS	(9000*_ClkOut/(10*_PreScale2*_PostScale2))
//#define TMR2_9MS	141	/* 2x 9ms = 18ms pause time */
// modified for Spectrum DX6 and DX7
#define TMR2_5MS	78	/* 1x 5ms +  */
#define TMR2_14MS	234	/* 1x 15ms = 20ms pause time */


//                    RX impuls times in 10-microseconds units
//                    vvv   ACHTUNG: Auf numerischen Überlauf achten!
#define	_Minimum	((105* _ClkOut/(2*_PreScale1))&0xFF)	/*-100% */
#define _Neutral	((150* _ClkOut/(2*_PreScale1))&0xFF)    /*   0% */
#define _Maximum	((195* _ClkOut/(2*_PreScale1))&0xFF)	/*+100% */
#define _ThresStop	((113* _ClkOut/(2*_PreScale1))&0xFF)	/*-90% ab hier Stopp! */
#define _ThresStart	((116* _ClkOut/(2*_PreScale1))&0xFF)	/*-85% ab hier Start! */
#define _ProgMode	((160* _ClkOut/(2*_PreScale1))&0xFF)	/*+75% */
#define _ProgUp		((150* _ClkOut/(2*_PreScale1))&0xFF)	/*+60% */
#define _ProgDown	((130* _ClkOut/(2*_PreScale1))&0xFF)	/*-60% */

// Sanity checks
#if _Minimum >= _Maximum
#error _Minimum < _Maximum!
#endif
#if _ThresStart <= _ThresStop
#error _ThresStart <= _ThresStop!
#endif
#if (_Maximum < _Neutral)
#error Maximum < _Neutral !
#endif

#if defined OPT_ADXRS && defined OPT_IDG
#error OPT_ADXRS and OPT_IDG set!
#endif
#if !defined OPT_ADXRS && !defined OPT_IDG
#error OPT_ADXRS and OPT_IDG both not set!
#endif

#if defined BOARD_3_0 && defined BOARD_3_1
#error BOARD_3_0 and BOARD_3_1 set!
#endif
#if !defined BOARD_3_0 && !defined BOARD_3_1
#error BOARD_3_0 and BOARD_3_1 both not set!
#endif


#define MAXDROPOUT	200	// max 200x 20ms = 4sec. dropout allowable

// Counter for flashing Low-Power LEDs
#define BLINK_LIMIT 100	// should be a nmbr dividable by 4!

// Parameters for UART port

#define _B9600	(_ClkOut*100000/(4*9600) - 1)
#define _B19200	(_ClkOut*100000/(4*19200) - 1)
#define _B38400	(_ClkOut*100000/(4*38400) - 1)

// EEPROM parameter set addresses

#define _EESet1	0		// first set starts at address 0x00
#define _EESet2	0x20	// second set starts at address 0x20

// Prototypes

// Bank 0

extern	page0	void OutSignals(void);
extern	page0	void GetGyroValues(void);
extern	page0	void CalcGyroValues(void);
extern	page0	void GetVbattValue(void);
extern	page0	void SendComValH(uns8);
extern	page0	void SendComChar(char);
extern	page0	void ShowSetup(uns8);
extern	page0	void ProcessComCommand(void);
extern	page0	void SendComValU(uns8);

// Bank 1
extern	page1	void GetEvenValues(void);
extern	page1	void ReadEEdata(void);
extern	page1	void DoProgMode(void);
extern	page1	void InitArrays(void);
extern  page1	void PID(void);
extern	page1	void Out(uns8);
extern	page1	void OutG(uns8);
extern	page1	void LimitRollSum(void);
extern	page1	void LimitNickSum(void);
extern	page1	void LimitYawSum(void);
extern	page1	void AddUpLRArr(uns8);
extern	page1	void AddUpFBArr(uns8);
extern	page1	void AcqTime(void);
extern	page1	void MixAndLimit(void);
extern	page1	void MixAndLimitCam(void);
extern	page1	void Delaysec(uns8);

#ifdef BOARD_3_1
extern	page1	void SendLeds(void);
extern	page1	void SwitchLedsOn(uns8);
extern	page1	void SwitchLedsOff(uns8);
#endif /* BOARD_3_1 */

// Bank 2
extern	page2	void CheckLISL(void);
extern	page2	void IsLISLactive(void);
extern 	page2	uns8 ReadLISL(uns8);
extern	page2	void OutSSP(uns8);

// End of c-ufo.h

