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

// to define the bank for the variables, set MATHBANK_VARS
// to shrBank, bank0, bank1, bank2, or bank3
// before including mymath16.h file!

// to define the bank for the math routine itself,
// set MATHBANK_PROG to 0, 1, 2, or 3
// before including mymath16.h file!

#include "mymath16.h"

#ifdef MATHBANK_PROG

#pragma codepage = MATHBANK_PROG

#endif

#if __CoreSet__ < 1600
 #define genAdd(r,a) W=a; btsc(Carry); W=incsz(a); r+=W
 #define genSub(r,a) W=a; btss(Carry); W=incsz(a); r-=W
 #define genAddW(r,a) W=a; btsc(Carry); W=incsz(a); W=r+W
 #define genSubW(r,a) W=a; btss(Carry); W=incsz(a); W=r-W
#else
 #define genAdd(r,a) W=a; r=addWFC(r)
 #define genSub(r,a) W=a; r=subWFB(r)
 #define genAddW(r,a) W=a; W=addWFC(r)
 #define genSubW(r,a) W=a; W=subWFB(r)
#endif

uns16 nilarg1;
uns16 nilarg2;
char  counter;
uns8  rm @rm16;
uns16 nilrval;
uns16 rm16;
int8  tmpArg2;
char  sign @tmpArg2;

// OK
void MathMultU8x8(void)	
{
    counter = 8;
    nilrval = 0;
    W = nilarg1.low8;
    do  {
        nilarg2.low8 = rr( nilarg2.low8);
        if (Carry)
            nilrval.high8 += W;
        nilrval = rr( nilrval);
        counter = decsz(counter);
    } while (1);
	return;
}

// OK
void MathMultS8x8(void)
{
    counter = 8;
    tmpArg2 = nilarg2.low8;
    nilrval = 0;
    W = nilarg1.low8;
    do  {
        tmpArg2 = rr( tmpArg2);
        if (Carry)
            nilrval.high8 += W;
        nilrval = rr( nilrval);
        counter = decsz(counter);
    } while (1);
    W = nilarg2.low8;
    if ((int8)nilarg1.low8 < 0)
        nilrval.high8 -= W;
    W = nilarg1.low8;
    if ((int8)nilarg2.low8 < 0)
        nilrval.high8 -= W;
	return;
}

// OK
void MathMultU16x8(void)
{
    counter = 16;
    do  {
        Carry = 0;
        nilrval = rl( nilrval);
        nilarg1 = rl( nilarg1);
        if (Carry)
            nilrval += nilarg2.low8;
        counter = decsz(counter);
    } while (1);
	return;
}

// OK
void MathMultU16x16(void)
{
    counter = 16;
    do  {
        Carry = 0;
        nilrval = rl( nilrval);
        nilarg1 = rl( nilarg1);
        if (Carry)
            nilrval += nilarg2;
        counter = decsz(counter);
    } while (1);
	return;
}

// OK
void MathDivU16_8(void)
{
    rm = 0;
    counter = 16+1;
    goto ENTRY;
    do  {
        rm = rl( rm);
        tmpArg2 = rl( tmpArg2);	// shift in carry from previous operation
        W = rm - nilarg2.low8;
        if (tmpArg2&1)
            Carry = 1;
        if (Carry)
            rm = W;
       ENTRY:
        nilarg1 = rl( nilarg1);
        counter = decsz(counter);
    } while (1);
	nilrval = nilarg1;	
	return;
}

// OK
void MathDivU16_16(void)
{
    rm16 = 0;
    counter = 16+1;
    goto ENTRY;
    do  {
        rm16 = rl( rm16);
        W = rm16.low8 - nilarg2.low8;
        genSubW( rm16.high8, nilarg2.high8);
        if (!Carry)
            goto ENTRY;
        rm16.high8 = W;
        rm16.low8 -= nilarg2.low8;
        Carry = 1;
       ENTRY:
        nilarg1 = rl( nilarg1);
        counter = decsz(counter);
    } while (1);
    return;
}

// OK
void MathDivS16_8(void)
{
    rm = 0;
    counter = 16+1;
    sign = nilarg1.high8 ^ nilarg2.low8;
    if ((long)nilarg1 < 0)  {
       INVERT:
        nilarg1 = -nilarg1;
        if (!counter)
		{
			nilrval = nilarg1;
            return;
		}
    }
    if ((int8)nilarg2.low8 < 0)
        (int8)nilarg2.low8 = -(int8)nilarg2.low8;
    goto ENTRY;
    do  {
        rm = rl( rm);
        W = rm - (int8)nilarg2.low8;
        if (Carry)
            rm = W;
       ENTRY:
        nilarg1 = rl( nilarg1);
        counter = decsz(counter);
    } while (1);
    if (sign & 0x80)
        goto INVERT;
	return;
}

// OK
void MathDivS16_16(void)
{
    rm16 = 0;
    counter = 16+1;
    sign = nilarg1.high8 ^ nilarg2.high8;
    if ((long)nilarg1 < 0)  {
       INVERT:
        nilarg1 = -nilarg1;
        if (!counter)
		{
			nilrval = nilarg1;
			return;
		}
    }
    if ((long)nilarg2 < 0)
        nilarg2 = -nilarg2;
    goto ENTRY;
    do  {
        rm16 = rl( rm16);
        W = rm16.low8 - nilarg2.low8;
        genSubW( rm16.high8, nilarg2.high8);
        if (!Carry)
            goto ENTRY;
        rm16.high8 = W;
        rm16.low8 -= nilarg2.low8;
        Carry = 1;
       ENTRY:
        nilarg1 = rl( nilarg1);
        counter = decsz(counter);
    } while (1);
    if (sign & 0x80)
        goto INVERT;
	nilrval = nilarg1;
	return;
}
