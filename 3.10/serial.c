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

// Serial support (RS232 option)

// this is required on CC5X V3.3
typedef char CHAR;

#pragma codepage=3
#pragma sharedAllocation

#include "c-ufo.h"
#include "bits.h"

// Math Library
#include "mymath16.h"

// data strings

const char page2 SerHello[] = "\r\nU.A.V.P. V" Version " Copyright (c) 2007"
							  " Ing. Wolfgang Mahringer\r\n"
							  "U.A.V.P. comes with ABSOLUTELY NO WARRANTY\r\n"
							  "This is FREE SOFTWARE, see GPL license!\r\n";

const char page2 SerSetup[] = "\r\nProfi-Ufo V" Version " ready.\r\n"
							  "Gyro: "
#ifdef OPT_ADXRS
							  "3x ADXRS300\r\n"
#endif
#ifdef OPT_IDG
							  "1x ADXRS300, 1x IDG300\r\n"
#endif
							  "Linear sensors ";
const char page2 SerLSavail[]="ONLINE\r\n";
const char page2 SerLSnone[]= "not available\r\n";
const char page2 SerChannel[]="Channel mode: Throttle Ch";
const char page2 SerFM_Fut[]= "3";
const char page2 SerFM_Grp[]= "1";

const char page2 SerHelp[]  = "\r\nCommands:\r\n"
					 		  "L...List param\r\n"
							  "M...Modify param\r\n"
							  "S...Show setup\r\n"
							  "N...Neutral values\r\n"
							  "R...Show receiver channels\r\n"
							  "B...start Boot-Loader\r\n";
const char page2 SerReg1[]  = "\r\nRegister ";
const char page2 SerReg2[]  = " = ";
const char page2 SerPrompt[]= "\r\n>";
const char page2 SerList[]  = "\r\nParameter list for set #";
const char page2 SerSelSet[]= "\r\nSelected parameter set: ";

const char page2 SerNeutralR[]="\r\nNeutral Roll:";
const char page2 SerNeutralN[]=" Nick:";
const char page2 SerNeutralY[]=" Yaw:";

const char page2 SerRecvCh[]=  "\r\nT:";

// transmit a fix text from a table
void SendComText(const char *pch)
{
	while( *pch != '\0' )
	{
		SendComChar(*pch);
		pch++;
	}
}

void ShowPrompt(void)
{
	SendComText(SerPrompt);
}

// send a character to the serial port
void SendComChar(char ch)
{
	while( TXIF == 0 ) ;	// wait for transmit ready
	TXREG = ch;		// put new char
}

// converts an unsigned byte to decimal and send it
void SendComValU(uns8 nival)
{
	bank1 char nidigit;

	nidigit = nival / 100;
	SendComChar(nidigit+'0');
	nival %= 100;

	nidigit = nival / 10;
	SendComChar(nidigit+'0');
	nival %= 10;

	SendComChar(nival+'0');
}

// converts a nibble to HEX and sends it
void SendComNibble(uns8 nival)
{
	nival += '0';
	if( nival > '9' )
		nival += 7;		// A to F
	SendComChar(nival);
}

// converts an unsigned byte to HEX and sends it
void SendComValH(uns8 nival)
{
	SendComNibble(nival >> 4);
	SendComNibble(nival & 0x0F);
}

// converts a signed byte to decimal and send it
void SendComValS(int nival)
{
	if( nival < 0 )
	{
		SendComChar('-');	// send sign
		nival = -nival;
	}
	else
		SendComChar('+');	// send sign

	SendComValU((uns8)nival);
}

// if a character is in the buffer
// return it. Else return the NUL character
char RecvComChar(void)
{
	char chread;

	if( RCIF )	// a character is waiting in the buffer
	{
		if( OERR || FERR )	// overrun or framing error?
		{
			CREN = 0;	// diable, then re-enable port to
			CREN = 1;	// reset OERR and FERR bit
			W = RCREG;	// dummy read
		}
		else
		{
			chread = RCREG;	// get the character
			SendComChar(chread);	// echo it
			return(chread);		// and return it
		}
	}
	return( '\0' );	// nothing in buffer
}

static bank0 uns8 i;
static bank0 char chread;

// enter an unsigned number 00 to 99
uns8 RecvComNumU(void)
{

	i = 0;
	do
	{
		chread = RecvComChar();
	}
	while( (chread < '0') || (chread > '9') );
	i = chread - '0';
	do
	{
		chread = RecvComChar();
	}
	while( (chread < '0') || (chread > '9') );
	i *= 10;
	i += chread - '0';
	return(i);
}

// enter a signed number -99 to 99 (always 2 digits)!
int RecvComNumS(void)
{
	i = 0;

	_NegIn = 0;
	do
	{
		chread = RecvComChar();
	}
	while( ((chread < '0') || (chread > '9')) &&
           (chread != '-') );
	if( chread == '-' )
	{
		_NegIn = 1;
		do
		{
			chread = RecvComChar();
		}
		while( (chread < '0') || (chread > '9') );
	}
	i = chread - '0';
	do
	{
		chread = RecvComChar();
	}
	while( (chread < '0') || (chread > '9') );
	i *= 10;
	i += chread - '0';
	if( _NegIn )
		i = -i;
	return(i);
}

// send the current configuration setup to serial port
void ShowSetup(uns8 nimode)
{
	if( nimode )
	{
		SendComText(SerHello);
		IK5 = _Minimum;	
	}

	SendComText(SerSetup);	// send hello message
	if( _UseLISL )
		SendComText(SerLSavail);
	else
		SendComText(SerLSnone);

	ReadEEdata();
	SendComText(SerChannel);
	if( FutabaMode )
		SendComText(SerFM_Fut);
	else
		SendComText(SerFM_Grp);

	SendComText(SerSelSet);
	if( IK5 > _Neutral )
		SendComChar('2');
	else
		SendComChar('1');
	
	ShowPrompt();
}

// if a command is waiting, read and process it.
// Do NOT call this routine while in flight!
void ProcessComCommand(void)
{
    int size1 *p;
	uns8 nireg;
	char chcmd @chread;

	chcmd = RecvComChar();
	if( chcmd.6 )	// 0x40..0x7F, a character
		chcmd.5=0;
	switch( chcmd )
	{
		case '\0' : break;
		case 'L'  :	// List parameters
			SendComText(SerList);
			if( IK5 > _Neutral )
				SendComChar('2');
			else
				SendComChar('1');
			ReadEEdata();
			nireg = 1;
			for(p = &FirstProgReg; p <= &LastProgReg; p++)
			{
				SendComText(SerReg1);
				SendComValU(nireg);
				SendComText(SerReg2);
				SendComValS(*p);
				nireg++;
			}
			ShowPrompt();
			break;
		case 'M'  : // modify parameters
			SendComText(SerReg1);
			nireg = RecvComNumU();
			nireg--;
			SendComText(SerReg2);	// = 
			EEDATA = RecvComNumS();
			if( IK5 > _Neutral )
				nireg += _EESet2;
			EEADR = nireg;
			EEPGD = 0;
// prog values into data flash
			WREN = 1;		// enable eeprom writes
			GIE = 0;
			EECON2 = 0x55;	// fix prog sequence (see 16F628A datasheet)
			EECON2 = 0xAA;
			WR = 1;			// start write cycle
			GIE = 1;
			while( WR == 1 );	// wait to complete
			WREN = 0;	// disable EEPROM write
			ShowPrompt();
			break;
		case 'S' :	// show status
			ShowSetup(0);
			break;
		case 'N' :	// neutral values
			SendComText(SerNeutralR);
			SendComValS(NeutralLR);

			SendComText(SerNeutralN);
			SendComValS(NeutralFB);

			SendComText(SerNeutralY);
			Tp -= 1024;		// subtract 1g (vertical sensor)
			SendComValS(NeutralUD);
			ShowPrompt();
			break;
		case 'R':	// receiver values
			SendComText(SerRecvCh);
			SendComValU(IGas);
			SendComChar(',');
			SendComChar('R');
			SendComChar(':');
			SendComValS(IRoll);
			SendComChar(',');
			SendComChar('N');
			SendComChar(':');
			SendComValS(INick);
			SendComChar(',');
			SendComChar('Y');
			SendComChar(':');
			SendComValS(ITurn);
			SendComChar(',');
			SendComChar('5');
			SendComChar(':');
			SendComValU(IK5);
			ShowPrompt();
			break;

		case 'B':	// call bootloader
#asm
			movlw	0x1f
			movwf	PCLATH
			dw	0x2F00
#endasm
//			BootStart();	// never comes back!
		case '?'  : // help
			SendComText(SerHelp);
			ShowPrompt();
	}
}

