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

// The sensor bus routines

#pragma codepage=3
#include "c-ufo.h"
#include "bits.h"

// Math Library
#include "mymath16.h"

#pragma sharedAllocation

#ifdef BOARD_3_1

void I2CDelay(void)
{
	nop2();
	nop2();
	nop2();
	nop2();
	nop2();
	nop2();
	nop2();
	nop2();
}

// put SCL to high-z and wait until line is really hi
// returns != 0 if ok
uns8 I2CWaitClkHi(void)
{
	uns8 nii=1;

	I2CDelay();
	I2C_CIO=1;	// set SCL to input, output a high
	while( I2C_SCL == 0 )	// wait for line to come hi
	{
//		nop2();
		nii++;
		if( nii == 0 )
			break;
	}	
	I2CDelay();
	return(nii);
}

// send a start condition
void I2CStart(void)
{
	I2C_DIO=1;	// set SDA to input, output a high
	I2CWaitClkHi();
	I2C_SDA = 0;	// start condition
	I2C_DIO = 0;	// output a low
	I2CDelay();
	I2C_SCL = 0;
	I2C_CIO = 0;	// set SCL to output, output a low
}

// send a stop condition
void I2CStop(void)
{
	I2C_DIO=0;	// set SDA to output
	I2C_SDA = 0;	// output a low
	I2CWaitClkHi();

	I2C_DIO=1;	// set SDA to input, output a high, STOP condition
	I2CDelay();		// leave clock high
}

static shrBank uns8 nii;	// mus be bank0 or shrBank

// send a byte to I2C slave and return ACK status
// 0 = ACK
// 1 = NACK
uns8 SendI2CByte(uns8 nidata)
{

	for(nii=0; nii<8; nii++)
	{
		if( nidata.7 )
		{
			I2C_DIO = 1;	// switch to input, line is high
		}
		else
		{
			I2C_SDA = 0;			
			I2C_DIO = 0;	// switch to output, line is low
		}
	
		if(!I2CWaitClkHi()) 
			return(I2C_NACK);
		I2C_SCL = 0;
		I2C_CIO = 0;	// set SCL to output, output a low
		nidata <<= 1;
	}
	I2C_DIO = 1;	// set SDA to input
	if(!I2CWaitClkHi())
		return(I2C_NACK);
	nii = I2C_SDA;	

	I2C_SCL = 0;
	I2C_CIO = 0;	// set SCL to output, output a low
//	I2CDelay();
//	I2C_IO = 0;		// set SDA to output
//	I2C_SDA = 0;	// leave output low
	return(nii);
}


// read a byte from I2C slave and set ACK status
// 0 = ACK
// 1 = NACK
// returns read byte
uns8 RecvI2CByte(uns8 niack)
{
	uns8 nidata=0;

	I2C_DIO=1;	// set SDA to input, output a high

	for(nii=0; nii<8; nii++)
	{
		if( !I2CWaitClkHi() ) 
			return(0);
		nidata <<= 1;
		if( I2C_SDA )
			nidata |= 1;
		I2C_SCL = 0;
		I2C_CIO = 0;	// set SCL to output, output a low
	}
	I2C_SDA = niack;
	I2C_DIO = 0;	// set SDA to output
	if( !I2CWaitClkHi() )
		return(0);

	I2C_SCL = 0;
	I2C_CIO = 0;	// set SCL to output, output a low
//	I2CDelay();
//	I2C_IO = 0;	// set SDA to output
	return(nidata);
}


// initialize compass sensor
void InitDirection(void)
{

// set Compass device to Compass mode 
	I2CStart();
	if( SendI2CByte(COMPASS_ADDR) != I2C_ACK ) goto IDerror;
	if( SendI2CByte('G')  != I2C_ACK ) goto IDerror;
	if( SendI2CByte(0x74) != I2C_ACK ) goto IDerror;
	// select operation mode, continuous mode, 20 Hz
	if( SendI2CByte(0b0.11.1.00.10) != I2C_ACK ) goto IDerror;
	I2CStop();

	I2CStart();
	if( SendI2CByte(COMPASS_ADDR) != I2C_ACK ) goto IDerror;
	if( SendI2CByte('r')  != I2C_ACK ) goto IDerror;
	if( SendI2CByte(0x06) != I2C_ACK ) goto IDerror;
	I2CStop();

// read multiple read count to avoid wear-and-tear on EEPROM
	I2CStart();
	if( SendI2CByte(COMPASS_ADDR+1) != I2C_ACK ) goto IDerror;
	if( RecvI2CByte(!I2C_ACK) != 16 )
	{	// not correctly set, set it up.
		I2CStop();

		I2CStart();
		if( SendI2CByte(COMPASS_ADDR) != I2C_ACK ) goto IDerror;
		if( SendI2CByte('w')  != I2C_ACK ) goto IDerror;
		if( SendI2CByte(0x06) != I2C_ACK ) goto IDerror;
		if( SendI2CByte(16)   != I2C_ACK ) goto IDerror;
	}
	_UseCompass = 1;
IDerror:
	I2CStop();
}

// Read direction, convert it to 2 degrees unit
// and store result in variable AbsDirection.
// The current heading correction is stored in CurDeviation
void GetDirection(void)
{

	bank2 long DirVal, temp;

// set Compass device to Compass mode 
	I2CStart();
	if( SendI2CByte(COMPASS_ADDR+1) != I2C_ACK ) 
	{
		I2CStop();
		CurDeviation = 0;	// no sensor present, deviation = 0
		return;
	}
	DirVal.high8 = RecvI2CByte(I2C_ACK);
	DirVal.low8  = RecvI2CByte(!I2C_ACK);
	I2CStop();

// DirVal has 1/10th degrees
// convert to set 360.0 deg = 240 units
	DirVal /= 15;

// must use pre-decrement, because of dumb compiler
	if( AbsDirection > COMPASS_MAX )
	{
		CurDeviation = 0;
		AbsDirection--;
	}
	else
	{
// setup desired heading (AbsDirection)
		if( AbsDirection == COMPASS_MAX )	// no heading stored yet
		{
			AbsDirection = DirVal;	// store current heading
			CurDeviation = 0;
		}
// calc deviation and direction of deviation
		DirVal = AbsDirection - DirVal;
// handle wraparound
		if( DirVal <= -240/2 )
			DirVal +=  240;
		if( DirVal >   240/2 )
			DirVal -=  240;

// positive means ufo is left off-heading
// negative means ufo is right off-heading

		if( DirVal > 20 )	// limit to give soft reaction
			DirVal = 20;
		if( DirVal < -20 )
			DirVal = -20;

// Empirical found :-)
// New_CurDev = ((3*Old_CurDev)+DirVal) / 4
//
		temp = (long)CurDeviation;	// the previous value!
		temp *= 3;
		temp += DirVal;			// add the new value
//		temp >>= 2;
		temp <<= 2;		// = 16* NewCurDev
		temp *= CompassFactor;
		CurDeviation = temp.high8;
	}
}

#endif	// BOARD_3_1

#ifdef BOARD_3_0
// dummy function required because of dumb compiler :-(
void SensorDummy(void)
{
}
#endif
