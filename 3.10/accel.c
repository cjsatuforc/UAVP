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

// Accelerator sensor routine

#pragma codepage=2
#include "c-ufo.h"
#include "bits.h"

// Math Library
#include "mymath16.h"

// read all acceleration values from LISL sensor
// and compute correction adders (Rp, Np, Vud)
void CheckLISL(void)
{

	ReadLISL(LISL_STATUS + LISL_READ);
	Rp.high8 = (int)ReadLISL(LISL_OUTX_H + LISL_READ);
	Rp.low8  = (int)ReadLISL(LISL_OUTX_L + LISL_READ);
	Np.high8 = (int)ReadLISL(LISL_OUTZ_H + LISL_READ);
	Np.low8  = (int)ReadLISL(LISL_OUTZ_L + LISL_READ);
	Tp.high8 = (int)ReadLISL(LISL_OUTY_H + LISL_READ);
	Tp.low8  = (int)ReadLISL(LISL_OUTY_L + LISL_READ);

// 1 unit is 1/4096 of 2g = 1/2048g
	Rp -= MiddleLR;
	Np -= MiddleFB;
	Tp -= 1024;	// subtract 1g
	Tp -= MiddleUD;

	UDSum += Tp;
//OutSSP(Tp.high8);
//OutSSP(Tp.low8);
//OutSSP(UDSum.high8);
//OutSSP(UDSum.low8);
	Tp = UDSum;
	Tp += 8;
	Tp >>= 4;
	Tp *= LinUDIntFactor;
	Tp += 128;

	if( (BlinkCount & 0x04) != 0 )	
	{
		if( (int)Tp.high8 > Vud )
			Vud++;
		if( (int)Tp.high8 < Vud )
			Vud--;
		if( Vud >  20 ) Vud =  20;
		if( Vud < -20 ) Vud = -20;
	}
	
	if( UDSum >  2 ) UDSum -= 2;
	if( UDSum < -2 ) UDSum += 2;


// =====================================
// Roll-Achse
// =====================================
// die statische Korrektur der Erdanziehung
//OutSSP(RollSamples.high8);
//OutSSP(RollSamples.low8);
//OutSSP(Rp.high8);
//OutSSP(Rp.low8);
//OutSSP(RollSum.high8);
//OutSSP(RollSum.low8);

#ifdef OPT_ADXRS
	Tl = RollSum * 11;	// Rp um RollSum*11/32 korrigieren
#endif

#ifdef OPT_IDG
	Tl = RollSum * -15; // Rp um RollSum* -15/32 korrigieren
#endif
	Tl += 16;
	Tl >>= 5;
	Rp -= Tl;

// die dynamische Korrektur der bewegten Massen
#ifdef OPT_ADXRS
	Rp += (long)RollSamples;
	Rp += (long)RollSamples;
#endif

#ifdef OPT_IDG
	Rp -= (long)RollSamples;
#endif

// Korrektur des DC-Pegels des Integrals
	if( Rp > 10 ) LRIntKorr = 1;
	if( Rp < 10 ) LRIntKorr = -1;

// Integral addieren, Abkling-Funktion
	Tl = LRSum >> 4;
	Tl >>= 1;
	LRSum -= Tl;	// LRSum * 0.96875
	LRSum += Rp;
	Rp = LRSum + 128;
	LRSumPosi += (int)Rp.high8;
	if( LRSumPosi >  2 ) LRSumPosi -= 2;
	if( LRSumPosi < -2 ) LRSumPosi += 2;

//OutSSP(RollSum.high8);
//OutSSP(RollSum.low8);
//OutSSP(Rp.high8);
//OutSSP(Rp.low8);
//OutSSP(LRSum.high8);
//OutSSP(LRSum.low8);
//OutSSP(LRSumPosi.high8);
//OutSSP(LRSumPosi.low8);
// Korrekturanteil fuer den PID Regler
	Rp = LRSumPosi * LinLRIntFactor;
	Rp += 128;
	Rp = (int)Rp.high8;
// limit output
	if( Rp >  2 ) Rp = 2;
	if( Rp < -2 ) Rp = -2;

// =====================================
// Nick-Achse
// =====================================
// die statische Korrektur der Erdanziehung

//OutSSP(NickSamples.high8);
//OutSSP(NickSamples.low8);
//OutSSP(Np.high8);
//OutSSP(Np.low8);
//OutSSP(NickSum.high8);
//OutSSP(NickSum.low8);

#ifdef OPT_ADXRS
	Tl = NickSum * 11;	// Np um RollSum* 11/32 korrigieren
#endif

#ifdef OPT_IDG
	Tl = NickSum * -14;	// Np um RollSum* -14/32 korrigieren
#endif
	Tl += 16;
	Tl >>= 5;

// OutSSP(Np.high8);
// OutSSP(Np.low8);
// OutSSP(NickSamples.high8);
// OutSSP(NickSamples.low8);
// OutSSP(NickSum.high8);
// OutSSP(NickSum.low8);

	Np -= Tl;
// die dynamische Korrektur der bewegten Massen
#ifdef OPT_ADXRS
//	Np += (long)NickSamples;
//	Np += (long)NickSamples;
#endif
#ifdef OPT_IDG
//	Np += (long)NickSamples;
//	Np += (long)NickSamples;
#endif
//OutSSP(Np.high8);
//OutSSP(Np.low8);

// Korrektur des DC-Pegels des Integrals
	if( Np > 10 ) FBIntKorr = 1;
	if( Np < 10 ) FBIntKorr = -1;

// Integral addieren
// Integral addieren, Abkling-Funktion
	Tl = FBSum >> 4;
	Tl >>= 1;
	FBSum -= Tl;	// LRSum * 0.96875
	FBSum += Np;
	Np = FBSum + 128;
	FBSumPosi += (int)Np.high8;
	if( FBSumPosi >  2 ) FBSumPosi -= 2;
	if( FBSumPosi < -2 ) FBSumPosi += 2;

// Korrekturanteil fuer den PID Regler
	Np = FBSumPosi * LinFBIntFactor;
	Np += 128;
	Np = (int)Np.high8;
// limit output
	if( Np >  2 ) Np = 2;
	if( Np < -2 ) Np = -2;

}
