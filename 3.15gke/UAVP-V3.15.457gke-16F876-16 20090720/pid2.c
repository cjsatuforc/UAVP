// =======================================================================
// =                   U.A.V.P Brushless UFO Controller                  =
// =                         Professional Version                        =
// =           Copyright (c) 2007, 2008 Ing. Wolfgang Mahringer          =
// =              Copyright 2008, 2009 by Prof. Greg Egan                =
// =                            http://uavp.ch                           =
// =======================================================================
//
//    UAVP is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    UAVP is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// Utilities and subroutines

#pragma codepage=1
#include "c-ufo.h"
#include "bits.h"

// Math Library
#include "mymath16.h"

void LimitRollSum(void)
{

	RollSum += (int16)RollSamples;

	if( IntegralCount == 0 )
	{
		NegFact = -RollIntLimit;

		if( (int8)RollSum.high8 >= RollIntLimit )
		{
			RollSum.high8 = RollIntLimit;
			RollSum.low8 = 0;
		}
		else
		if( (int8)RollSum.high8 < NegFact )
		{
			RollSum.high8 = NegFact;
			RollSum.low8 = 0;
		}
		if( RollSum > 0 ) 
			RollSum--;
		else
			if( RollSum < 0 ) 
				RollSum++;
		RollSum += LRIntKorr;
	}

} // LimitRollSum


void LimitPitchSum(void)
{

	PitchSum += (int16)PitchSamples;

	if( IntegralCount == 0 )
	{
		NegFact = -PitchIntLimit;
		if( (int8)PitchSum.high8 >= PitchIntLimit )
		{
			PitchSum.high8 = PitchIntLimit;
			PitchSum.low8 = 0;
		}
		else
			if( (int8)PitchSum.high8 < NegFact )
			{
				PitchSum.high8 = NegFact;
				PitchSum.low8 = 0;
			}
		if( PitchSum > 0 ) 
			PitchSum--;
		else
			if( PitchSum < 0 ) 
				PitchSum++;
		PitchSum += FBIntKorr;
	}
} // LimitPitchSum

void LimitYawSum(void)
{
	// add the yaw stick value
	YE += IYaw;

	if ( _UseCompass )
	{
		// add compass heading correction
		// CurDeviation is negative if Ufo has yawed to the right (go back left)

		// this double "if" is necessary because of dumb CC5X compiler
		NegFact = YawNeutral + COMPASS_MIDDLE;
		if ( IYaw > NegFact )
			// yaw stick is not in neutral zone, learn new desired heading
			AbsDirection = COMPASS_INVAL;
		else		
		{
			NegFact = YawNeutral - COMPASS_MIDDLE;
			if ( IYaw < NegFact )
				// yaw stick is not in neutral zone, learn new desired heading
				AbsDirection = COMPASS_INVAL;
			else
				// yaw stick is in neutral zone, hold heading
				if( CurDeviation > COMPASS_MAXDEV )
					YE -= COMPASS_MAXDEV;
				else
					if( CurDeviation < -COMPASS_MAXDEV )
						YE += COMPASS_MAXDEV;
					else
						YE -= CurDeviation;
		}
	}

	YawSum += (int16)YE;
	NegFact = -YawIntLimit;
	if( (int8)YawSum.high8 >= YawIntLimit )
	{
		YawSum.high8 = YawIntLimit;
		YawSum.low8 = 0;
	}
	else
 		if( (int8)YawSum.high8 < NegFact )
		{
			YawSum.high8 = NegFact;
			YawSum.low8 = 0;
		}

	if ( YawSum > 0) 
		YawSum--; 
	else 
		if ( YawSum < 0 ) 
			YawSum++;

	if ( YawSum > 0) 
		YawSum--; 
	else 
		if ( YawSum < 0 ) 
			YawSum++;

} // LimitYawSum


uns8 SaturInt(int16 l)
{
	#if defined ESC_PPM || defined ESC_HOLGER || defined ESC_YGEI2C
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
	return((uns8) l );
} // SaturInt


#ifdef ALT_MIXANDLIMIT

// Alternate scheme due to Gary Stofer
void MixAndLimit(void)
{
	int16 bank2 MinMotor;

	#ifndef TRICOPTER

	MinMotor = (int16) IGas;

	if ( MinMotor < Rl )
		Rl = MinMotor;
	if ( MinMotor < Pl )
		Pl = MinMotor;

	MinMotor = -MinMotor;
	
	if ( Rl < MinMotor )
		Rl = MinMotor;
	if ( Pl < MinMotor )
		Pl = MinMotor;

	Ml = IGas - Rl;
	Mr = IGas + Rl;
	Mf = IGas - Pl;
	Mb = IGas + Pl;

	if ( Mf < Mb )
		MinMotor = Mf;
	if ( Ml < MinMotor )
		MinMotor = Ml;
	if ( Mr < MinMotor )
		MinMotor = Mr;
	
	if ( MinMotor < Yl )
		Yl = MinMotor;

	MinMotor = - MinMotor;

	if ( Yl < MinMotor )
		Yl = MinMotor;

	Mf += Yl;
	Mb += Yl;
	Ml -= Yl;
	Mr -= Yl;

	Mf += MotorLowRun;
	Mb += MotorLowRun;
	Ml += MotorLowRun;
	Mr += MotorLowRun;

	// Barometer
	Mf += VBaroComp;
	Mb += VBaroComp;
	Ml += VBaroComp;
	Mr += VBaroComp;

	#ifdef NADA
	// Vertical velocity damping
	Mf += Vud;
	Mb += Vud;
	Ml += Vud;
	Mr += Vud;
	#endif

	#else

		No Tricopter

	#endif // !TRICOPTER

	MFront = SaturInt(Mf);
	MLeft = SaturInt(Ml);
	MRight = SaturInt(Mr);
	MBack = SaturInt(Mb);
} // MixAndLimit

#else

void MixAndLimit(void)
{
	int16 CurrGas;
	int16 bank2 Temp;

	CurrGas = (int16) IGas; // to protect against IGas being changed in interrupt
	Temp = (int16)VBaroComp;
	Temp += (int16)Vud;
	CurrGas += Temp;	

	#ifdef TRICOPTER

	Temp = (Rl - Pl) / 2; 
	Mf = CurrGas + Pl ;				// front motor
	Ml = CurrGas + Temp;			// rear left
	Mr = CurrGas - Temp; 			// rear right
	Mb = Yl + _Neutral;				// yaw servo

	if( CurrGas > MotorLowRun )
	{
		if( (Ml > Mr) && (Mr < MotorLowRun) )
		{
			// Mf += Mb - MotorLowRun
			Ml += Mr;
			Ml -= MotorLowRun;
		}
		if( (Mr > Ml) && (Ml < MotorLowRun) )
		{
			// Mb += Mf - MotorLowRun
			Mr += Ml;
			Mr -= MotorLowRun;
		}
	}
	#else

	if( FlyCrossMode )
	{	// "Cross" Mode
		Ml = CurrGas + Pl; Ml -= Rl;
		Mr = CurrGas - Pl; Mr += Rl;
		Mf = CurrGas - Pl; Mf -= Rl;
		Mb = CurrGas + Pl; Mb += Rl;
	}
	else
	{	// "Plus" Mode
		#ifdef MOUNT_45
		Ml = CurrGas - Rl; Ml -= Pl;	// K2 -> Front right
		Mr = CurrGas + Rl; Mr += Pl;	// K3 -> Rear left
		Mf = CurrGas + Rl; Mf -= Pl;	// K1 -> Front left
		Mb = IGas - Rl; Mb += Pl;	// K4 -> Rear rigt
		#else
		Ml = CurrGas - Rl;	// K2 -> Front right
		Mr = CurrGas + Rl;	// K3 -> Rear left
		Mf = CurrGas - Pl;	// K1 -> Front left
		Mb = CurrGas + Pl;	// K4 -> Rear rigt
		#endif
	}

	Mf += Yl;
	Mb += Yl;
	Ml -= Yl;
	Mr -= Yl;

	// if low-throttle limiting occurs, must limit other motor too
	// to prevent flips!

	if( CurrGas > MotorLowRun )
	{
		if( (Mf > Mb) && (Mb < MotorLowRun) )
		{
			NegFact = Mb - MotorLowRun;
			Mf += NegFact;
			Ml += NegFact;
			Mr += NegFact;
		}
		if( (Mb > Mf) && (Mf < MotorLowRun) )
		{
			NegFact = Mf - MotorLowRun;
			Mb += NegFact;
			Ml += NegFact;
			Mr += NegFact;
		}
		if( (Ml > Mr) && (Mr < MotorLowRun) )
		{
			NegFact = Mr - MotorLowRun;
			Ml += NegFact;
			Mf += NegFact;
			Mb += NegFact;
		}
		if( (Mr > Ml) && (Ml < MotorLowRun) )
		{	
			NegFact = Ml - MotorLowRun;
			Mr += NegFact;
			Mf += NegFact;
			Mb += NegFact;
		}
	}
	#endif

	MFront = SaturInt(Mf);
	MLeft = SaturInt(Ml);
	MRight = SaturInt(Mr);
	MBack = SaturInt(Mb);

} // MixAndLimit

#endif // ALT_MIXANDLIMIT