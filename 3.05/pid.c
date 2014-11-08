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

// The PID controller algorithm

#pragma codepage=1
#include "c-ufo.h"
#include "bits.h"

// Math Library
#include "mymath16.h"

// Routine dividiert ein long (*shx) gerundet durch 16.
void Shift4(long *shx)
{
	long l;

	l = *shx;
	l += 8;			// to round up
	l >>= 4;		// div by 16, no round
	*shx = l;
}


// compute the correction adders for the motors
// using the gyro values (PID controller)
// for the axes Roll and Nick
void PID(void)
{

	if( IntegralTest )
		ALL_LEDS_OFF;

// Roll/Nick Linearsensoren
	Rp = 0;
	Np = 0;
	Vud = 0;
	if( _UseLISL )
		CheckLISL();	// get the linear sensors data, if available

// PID controller
// E0 = current gyro error
// E1 = previous gyro error
// Sum(Ex) = integrated gyro error, sinst start of ufo!
// A0 = current correction value
// fx = programmable controller factors
//
// for Roll and Nick:
//       E0*fP + E1*fD     Sum(Ex)*fI
// A0 = --------------- + ------------
//            16               256


// ####################################
// Roll

// Differential and Proportional for Roll axis

	Rl  = (long)REp  * (long)RollDiffFactor;
	Rl += (long)RE   * (long)RollPropFactor;

	Shift4(&Rl);	// div by 16

	Rl += Rp;	// add linear part

	if( IntegralTest )
	{
		if( (int)RollSum.high8 > 0 )
		{
			LedRed_ON;
		}
		if( (int)RollSum.high8 < -1 )
		{
			LedGreen_ON;
		}
	}

// Integral part for Roll

	if( IntegralCount == 0 )
	{
		Rp = RollSum * (long)RollIntFactor;
		Rp += 128;
		Rl += (int)Rp.high8;
	}

// subtract stick signal
	Rl -= IRoll;

// muss so gemacht werden, weil CC5X kein if(Nl < -RollNickLimit) kann!
	NegFact = -RollLimit;
	if( Rl < NegFact ) Rl = NegFact;
	if( Rl > RollLimit ) Rl = RollLimit;

// ####################################
// Nick

// Differential and Proportional for Nick

	Nl  = (long)NEp  * (long)NickDiffFactor;
	Nl += (long)NE   * (long)NickPropFactor;
	Shift4(&Nl);	// div by 16

	Nl += Np;	// add linear part

	if( IntegralTest )
	{
		if( (int)NickSum.high8 >  0 )
		{
			LedYellow_ON;
		}
		if( (int)NickSum.high8 < -1 )
		{
			LedBlue_ON;
		}
	}

// Integral part for Nick
	if( IntegralCount == 0 )
	{
		Np = NickSum * (long)NickIntFactor;
		Np += 128;
		Nl += (int)Np.high8;
	}

// subtract stick signal
	Nl -= INick;

// muss so gemacht werden, weil CC5X kein if(Nl < -RollNickLimit) kann!
	NegFact = -NickLimit;
	if( Nl < NegFact ) Nl = NegFact;
	if( Nl > NickLimit ) Nl = NickLimit;

// PID controller for Yaw (Heading Lock)
//       E0*fp + E1*fD     Sum(Ex)*fI
// A0 = --------------- + ------------
//             16              256

// ####################################
// Yaw

// the yaw stick signal is already added in LimitYawSum() !
//	TE += ITurn;

	Tl = YawSum * (long)TurnIntFactor;
	Tl += 128;
	Tl = (int)Tl.high8;

// Differential for Yaw (for quick and stable reaction)

	Tp  =  (long)TEp * (long)TurnDiffFactor;
	Tp  += (long)TE  * (long)TurnPropFactor;
	Shift4(&Tp);	// div by 16
	Tl += Tp;

	NegFact = -YawLimit;
	if( Tl < NegFact ) Tl = NegFact;
	if( Tl > YawLimit ) Tl = YawLimit;

//
// calculate camera servos
//
// use only integral part (direct angle)
//
	if( IntegralCount == 0 )
	{
		Rp = RollSum / (long)CamIntFactor;
		Np = NickSum / (long)CamIntFactor;
	}
	else
	{
		Rp = 0;
		Np = 0;
	}
}
