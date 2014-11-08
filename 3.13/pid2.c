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

// Utilities and subroutines

#pragma codepage=1
#include "c-ufo.h"
#include "bits.h"

// Math Library
#include "mymath16.h"


// Limit integral sum of Roll gyros
// it must be limited to avoid numeric overflow
// which would cause a serious flip -> crash
void LimitRollSum(void)
{

	RollSum += (long)RollSamples;

	if( IntegralCount == 0 )
	{
		NegFact = -RollIntLimit;

		if( (int)RollSum.high8 >= RollIntLimit )
		{
			RollSum.high8 = RollIntLimit;
			RollSum.low8 = 0;
		}
		else
		if( (int)RollSum.high8 < NegFact )
		{
			RollSum.high8 = NegFact;
			RollSum.low8 = 0;
		}
		RollSum += LRIntKorr;
		if( RollSum > 0 ) RollSum--;
		if( RollSum < 0 ) RollSum++;
	}
}

// Limit integral sum of Nick gyros
// it must be limited to avoid numeric overflow
// which would cause a serious flip -> crash
void LimitNickSum(void)
{

	NickSum += (long)NickSamples;

	if( IntegralCount == 0 )
	{
		NegFact = -NickIntLimit;
		if( (int)NickSum.high8 >= NickIntLimit )
		{
			NickSum.high8 = NickIntLimit;
			NickSum.low8 = 0;
		}
		else
		if( (int)NickSum.high8 < NegFact )
		{
			NickSum.high8 = NegFact;
			NickSum.low8 = 0;
		}
		NickSum += FBIntKorr;
		if( NickSum > 0 ) NickSum--;
		if( NickSum < 0 ) NickSum++;
	}
}

// Limit integral sum of Yaw gyros
// it must be limited to avoid numeric overflow
// which would cause a uncontrolled yawing -> crash
void LimitYawSum(void)
{

// add the yaw stick value
	TE += ITurn;
#ifdef BOARD_3_1
// add compass heading correction
// CurDeviation is negative if Ufo has yawed to the right (go back left)

// this double "if" is necessary because of dumb CC5X compiler
	NegFact = YawNeutral + COMPASS_MIDDLE;
	if ( ITurn > NegFact )
// yaw stick is not in neutral zone, learn new desired heading
		AbsDirection = COMPASS_INVAL;
	else		
	{
		NegFact = YawNeutral - COMPASS_MIDDLE;
		if ( ITurn < NegFact )
// yaw stick is not in neutral zone, learn new desired heading
			AbsDirection = COMPASS_INVAL;
		else
		{
// yaw stick is in neutral zone, hold heading
			if( CurDeviation > COMPASS_MAXDEV )
				TE -= COMPASS_MAXDEV;
			else
			if( CurDeviation < -COMPASS_MAXDEV )
				TE += COMPASS_MAXDEV;
			else
			{
				TE -= CurDeviation;
			}
		}
	}
#endif

	YawSum += (long)TE;
	NegFact = -YawIntLimit;
	if( (int)YawSum.high8 >= YawIntLimit )
	{
		YawSum.high8 = YawIntLimit;
		YawSum.low8 = 0;
	}
 	if( (int)YawSum.high8 < NegFact )
	{
		YawSum.high8 = NegFact;
		YawSum.low8 = 0;
	}
}

// to avoid stopping motors in the air, the
// motor values are limited to a minimum and
// a maximum
// the eventually corrected value is returned
int SaturInt(long l)
{
#if defined ESC_PPM || defined ESC_HOLGER
	if( l > _Maximum )
		return(_Maximum);
	if( l < MotorLowRun )
		return(MotorLowRun);
// just for safety
	if( l < _Minimum )
		return(_Minimum);
#endif

#ifdef ESC_X3D
	l -= _Minimum;
	if( l > 200 )
		return(200);
	if( l < 1 )
		return(1);
#endif
	return((int)l);
}

// mix the PID-results (Rl, Nl and Tl) and the throttle
// on the motors and check for numerical overrun
void MixAndLimit(void)
{

	GIE=0;	// enter critical section, block interrupts
#ifdef USE_THROTTLECURVE
// TC_ADDER MUST BE >= TC_LINEAR !
#define TC_FPSTART	90U	// flat curve start point 
#define TC_LINEAR	20U
// horizontal distance of the first and the second steep curves
// 120 = (256 - TC_LINEAR/2)/2
#define TC_SCSTART	((100U*2)/3)	// steep curve start point adder

// calculate the dynamic throttle curve

	if( _NewValues )
	{
	//	IGas -= _Minimum;
		_NewValues = 0;
		if( IGas > TC_LINEAR )
		{
			IGas -= TC_LINEAR;
			if( IGas <= (TC_FPSTART-TC_LINEAR)/2)
			{
				IGas <<= 1;		// steep slope
				IGas += TC_LINEAR;
			}
			else
			{
				IGas -= (TC_FPSTART-TC_LINEAR)/2;
				if( IGas <= 2*TC_SCSTART )
				{			
					IGas >>= 1;		// flat slope
					IGas += TC_FPSTART;
				}
				else
				{
					IGas -= 2*TC_SCSTART;
					IGas <<= 1;		// steep slope
					IGas += TC_FPSTART +  TC_SCSTART;
				}
			}
		}
	}
#endif	// USE_THROTTLECURVE end

#ifndef TRICOPTER
	if( FlyCrossMode )
	{	// "Cross" Mode
		Ml = IGas - Rl;		Ml -= Nl;
		Mr = IGas + Rl;		Mr += Nl;
		Mv = IGas + Rl;		Mv -= Nl;
		Mh = IGas - Rl;		Mh += Nl;
	}
	else
	{	// "Plus" Mode
		Ml = IGas - Rl;
		Mr = IGas + Rl;
		Mv = IGas - Nl;
		Mh = IGas + Nl;
	}
	GIE=1;	// end critical section
	Mv += Tl;
	Mh += Tl;
	Ml -= Tl;
	Mr -= Tl;

// Altitude stabilization factor

	Mv += Vud;
	Mh += Vud;
	Ml += Vud;
	Mr += Vud;

// if low-throttle limiting occurs, must limit other motor too
// to prevent flips!

#ifdef USE_DSP
	if( (Mv > Mh) && (Mh < MotorLowRun) )
	{
		NegFact = Mh - MotorLowRun;
		Mv += NegFact;
		Ml += NegFact;
		Mr += NegFact;
	}
	if( (Mh > Mv) && (Mv < MotorLowRun) )
	{
		NegFact = Mv - MotorLowRun;
		Mh += NegFact;
		Ml += NegFact;
		Mr += NegFact;
	}
	if( (Ml > Mr) && (Mr < MotorLowRun) )
	{
		NegFact = Mr - MotorLowRun;
		Ml += NegFact;
		Mv += NegFact;
		Mh += NegFact;
	}
	if( (Mr > Ml) && (Ml < MotorLowRun) )
	{	
		NegFact = Ml - MotorLowRun;
		Mr += NegFact;
		Mv += NegFact;
		Mh += NegFact;
	}
#else	// !USE_DSP
	if( (Mv > Mh) && (Mh < MotorLowRun) )
	{
		// Mv += Mh - MotorLowRun
		Mv += Mh;
		Mv -= MotorLowRun;
	}
	if( (Mh > Mv) && (Mv < MotorLowRun) )
	{
		// Mh += Mv - MotorLowRun
		Mh += Mv;
		Mh -= MotorLowRun;
	}

	if( (Ml > Mr) && (Mr < MotorLowRun) )
	{
		// Mv += Mh - MotorLowRun
		Ml += Mr;
		Ml -= MotorLowRun;
	}
	if( (Mr > Ml) && (Ml < MotorLowRun) )
	{
		// Mh += Mv - MotorLowRun
		Mr += Ml;
		Mr -= MotorLowRun;
	}
#endif	// USE_DSP
#else	// TRICOPTER
	Mv = IGas + Nl;	// front motor
	Ml = IGas + Rl;
	Mr = IGas - Rl;
	Rl >>= 1;
	Ml -= Rl;	// rear left
    Mr -= Nl;	// rear right
	Mh = Tl + _Neutral;	// yaw servo

	if( (Ml > Mr) && (Mr < MotorLowRun) )
	{
		// Mv += Mh - MotorLowRun
		Ml += Mr;
		Ml -= MotorLowRun;
	}
	if( (Mr > Ml) && (Ml < MotorLowRun) )
	{
		// Mh += Mv - MotorLowRun
		Mr += Ml;
		Mr -= MotorLowRun;
	}
#endif


// Ergebnisse auf Überlauf testen und korrigieren

	MVorne = SaturInt(Mv);
	MLinks = SaturInt(Ml);
	MRechts = SaturInt(Mr);
	MHinten = SaturInt(Mh);
}

