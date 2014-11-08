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

// only CC5X 3.3 knows why this is necessary...
page1 int SaturInt(long);

// to avoid stopping motors in the air, the
// motor values are limited to a minimum and
// a maximum
// the eventually corrected value is returned
int SaturInt(long l)
{
#if defined ESC_PWM || defined ESC_HOLGER
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
	Mv += Tl;
	Mh += Tl;
	Ml -= Tl;
	Mr -= Tl;

// Altitude stabilization factor

	Mv += Vud;
	Mh += Vud;
	Ml += Vud;
	Mr += Vud;

// Ergebnisse auf Überlauf testen und korrigieren

	MVorne = SaturInt(Mv);
	MLinks = SaturInt(Ml);
	MRechts = SaturInt(Mr);
	MHinten = SaturInt(Mh);
}

// Mix the Camera tilt channel (Ch6) and the
// ufo air angles (roll and nick) to the 
// camera servos. 
void MixAndLimitCam(void)
{
// Cam Servos
	Rp += IK6;
	Np += IK6;
	if( Rp > _Maximum )
		MCamRoll = _Maximum;
	else
	if( Rp < _Minimum )
		MCamRoll = _Minimum;
	else
		MCamRoll = Rp;

	if( Np > _Maximum )
		MCamNick = _Maximum;
	else
	if( Np < _Minimum )
		MCamNick = _Minimum;
	else
		MCamNick = Np;
}

