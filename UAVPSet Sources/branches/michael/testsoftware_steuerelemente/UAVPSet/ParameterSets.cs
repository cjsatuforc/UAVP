// UAVPSet
// Copyright (C) 2007  Thorsten Raab
// Email: thorsten.raab@gmx.at
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace UAVP.UAVPSet
{
    /// <summary>
    /// zentrale klasse um alle Prameter zu speichern
    /// </summary>
    public class ParameterSets
    {
        /// <summary>
        /// initialisierung der Parameter
        /// </summary>
        public ParameterSets()
        {
            // alle Parameter werden vorbereitet
            //TODO: Erweitern wenn neue Parameter hinzukommen
            for (int i = 0; i < parameterForm1.Length; i++) 
            {
                parameterForm1[i].Chapter = "";
                parameterForm2[i].Chapter = "";
                parameterPic1[i].Chapter = "";
                parameterPic2[i].Chapter = "";
                // mindestwerte vorbelegen
                switch (i)
                {
                    case 3:
                    case 4:
                    case 8:
                    case 9:
                    case 13:
                    case 14:
                    case 16:
                        parameterForm1[i].Value = "1";
                        parameterForm2[i].Value = "1";
                        parameterPic1[i].Value = "1";
                        parameterPic2[i].Value = "1";
                        break;
                    case 22:
                        parameterForm1[i].Value = "5";
                        parameterForm2[i].Value = "5";
                        parameterPic1[i].Value = "5";
                        parameterPic2[i].Value = "5";
                        break;
                    default:
                        parameterForm1[i].Value = "0";
                        parameterForm2[i].Value = "0";
                        parameterPic1[i].Value = "0";
                        parameterPic2[i].Value = "0";
                        break;

                }
                parameterForm1[i].Command = "Register " + (i+1).ToString();
                parameterForm2[i].Command = "Register " + (i + 1).ToString();
                parameterPic1[i].Command = "Register " + (i + 1).ToString();
                parameterPic2[i].Command = "Register " + (i + 1).ToString();
                parameterForm1[i].Comment = kurzParameter[i];
                parameterForm2[i].Comment = kurzParameter[i];
                parameterPic1[i].Comment = kurzParameter[i];
                parameterPic2[i].Comment = kurzParameter[i];
            }

        }
        
        
        // farben um die Felder bei update zu markieren
        public enum Farbe {black, green, orange, red};

        /// <summary>
        /// allgemeine Struktur um die Parameter aufzunehmen
        /// </summary>
        public struct ParameterSetsStruc
        {
            public string Command;
            public string Value;
            public string Comment;
            public string Chapter;
        }

        //TODO: Array erweitern wenn neue Parameter hinzukommen
        // Parameter Sets erzeugen für Form
        public ParameterSetsStruc[] parameterForm1 = new ParameterSetsStruc[27];
        public ParameterSetsStruc[] parameterForm2 = new ParameterSetsStruc[27];
        // für PIC Register 1 und 2
        public ParameterSetsStruc[] parameterPic1 = new ParameterSetsStruc[27];
        public ParameterSetsStruc[] parameterPic2 = new ParameterSetsStruc[27];


        //namen der Parameter für kurz-Ausgabe
        //TODO: Array erweitern wenn neue Parameter hinzukommen
        string[] kurzParameter = { "RP","RI","RD","RL","RIL",
                                    "\r\nNP","NI","ND","NL","NIL",
                                    "\r\nGP","GI","GD","GL","GIL",
                                    "\r\nGK","IA","AU","LRL","LVZ",
                                    "\r\nLAA","LWG","LL","LWR","LWN","KRN","KO" };
  

        
        /// <summary>
        /// Laden einer Konfigurationsdatei
        /// </summary>
        /// <param name="pfad"></param>
        /// <param name="mainForm"></param>
        public void laden(string pfad, FormMain mainForm)
        {
            // try / catch wenn fehler beim öffnen oder interpretieren der datei
            try 
            {
                IniReader iniReader = new IniReader(pfad);
                // log schreiben bei debug
                Log.write(mainForm, "Load Parameterset: " + pfad, 1);
                ParameterSetsStruc[] registers = iniReader.GetChapter("ChannelSet");
                // alle Felder auf rot setzen
                updateForm(registers, mainForm, Farbe.red, false);
            } 
            catch (Exception e) 
            {
                MessageBox.Show(e.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.write(mainForm, "Wrong Parameter File!", 0);
            }
        }

        /// <summary>
        /// speichern der Parameter in eine Datei
        /// </summary>
        /// <param name="pfad"></param>
        /// <param name="mainForm"></param>
        public void speichern(string pfad, FormMain mainForm)
        {

            StreamWriter sw = new StreamWriter(pfad, false, Encoding.GetEncoding("windows-1252")); 
            try 
            {
                // header für Datei
                sw.WriteLine("[ChannelSet]");
                // es wird immer das aktive Tab gespeichert
                if (mainForm.tabControlParameter.SelectedIndex == 0) 
                {
                    foreach (ParameterSetsStruc register in parameterForm1) 
                    {
                        if (register.Command != null) 
                        {
                            sw.WriteLine(register.Command + "=" + register.Value);
                        }
                    }
                } 
                else 
                {
                    foreach (ParameterSetsStruc register in parameterForm2) 
                    {
                        if (register.Command != null) 
                        {
                            sw.WriteLine(register.Command + "=" + register.Value);
                        }
                    }
                }
            } 
            catch (Exception e) 
            {
                MessageBox.Show(e.ToString(), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.write(mainForm, e.ToString(), 1);
            } 
            finally 
            {
                // datei schließen
                sw.Close();
            }
            // log schreiben bei debug
            Log.write(mainForm, "Write Parameterset: " + pfad, 1);
        }

        /// <summary>
        /// ein einzelnes Feld updaten
        /// </summary>
        /// <param name="objekt"></param>
        /// <param name="mainForm"></param>
        public void feldUpdaten(Object objekt, FormMain mainForm)
        {
            // wenn es sich um ein Parameter feld handelt
            if (objekt.GetType().Name == "NumericUpDown") 
            {
                NumericUpDown feld = (NumericUpDown)objekt;
                // feld auf orange setzen
                feld.ForeColor = Color.Orange;
                // je nach Parameter TAB
                if (mainForm.tabControlParameter.SelectedIndex == 0) 
                {
                    parameterForm1[Convert.ToInt16(feld.Tag)-1].Value = feld.Value.ToString();
                    // wenn es gleich dem PIC wert ist -> wieder grün
                    if (parameterForm1[Convert.ToInt16(feld.Tag) - 1].Value == 
                        parameterPic1[Convert.ToInt16(feld.Tag) - 1].Value &&
                        parameterPic1[Convert.ToInt16(feld.Tag) - 1].Chapter == "ChannelSet") 
                    {
                        if (mainForm.writeUpdate == false) 
                        {
                            feld.ForeColor = Color.Green;
                        }
                    }
                } 
                else 
                {
                    parameterForm2[Convert.ToInt16(feld.Tag)-1].Value = feld.Value.ToString();
                    // wenn es gleich dem PIC wert ist -> wieder grün
                    if (parameterForm2[Convert.ToInt16(feld.Tag)-1].Value ==
                        parameterPic2[Convert.ToInt16(feld.Tag) - 1].Value &&
                        parameterPic2[Convert.ToInt16(feld.Tag) - 1].Chapter == "ChannelSet") 
                    {
                        if (mainForm.writeUpdate == false) 
                        {
                            feld.ForeColor = Color.Green;
                        }
                    }
                }
            }
            // wenn es ein BIT Wert ist muss der Parameter Wert errechnet werden
            else {
                CheckBox feld = (CheckBox)objekt;
                feld.ForeColor = Color.Orange;

                // welches BIT ist betroffen?
                switch (feld.Name) 
                {
                    case "bit01CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) | 1).ToString();
                        } 
                        else 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) ^ 1).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm1[15].Value) & 1) ==
                            (Convert.ToInt16(parameterPic1[15].Value) & 1) &&
                            parameterPic1[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit11CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) | 2).ToString();
                        } 
                        else 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) ^ 2).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm1[15].Value) & 2) ==
                            (Convert.ToInt16(parameterPic1[15].Value) & 2) &&
                            parameterPic1[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit21CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) | 4).ToString();
                        } 
                        else 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) ^ 4).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm1[15].Value) & 4) ==
                            (Convert.ToInt16(parameterPic1[15].Value) & 4) &&
                            parameterPic1[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit31CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) | 8).ToString();
                        } 
                        else 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) ^ 8).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm1[15].Value) & 8) ==
                            (Convert.ToInt16(parameterPic1[15].Value) & 8) &&
                            parameterPic1[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit41CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) | 16).ToString();
                        } 
                        else 
                        {
                            parameterForm1[15].Value = (Convert.ToInt16(parameterForm1[15].Value) ^ 16).ToString();
                        } 
                        if ((Convert.ToInt16(parameterForm1[15].Value) & 16) ==
                            (Convert.ToInt16(parameterPic1[15].Value) & 16) &&
                            parameterPic1[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit02CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) | 1).ToString();
                        } 
                        else 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) ^ 1).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm2[15].Value) & 1) ==
                            (Convert.ToInt16(parameterPic2[15].Value) & 1) &&
                            parameterPic2[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit12CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) | 2).ToString();
                        } 
                        else 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) ^ 2).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm2[15].Value) & 2) ==
                            (Convert.ToInt16(parameterPic2[15].Value) & 2) &&
                            parameterPic2[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit22CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) | 4).ToString();
                        } 
                        else 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) ^ 4).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm2[15].Value) & 4) ==
                            (Convert.ToInt16(parameterPic2[15].Value) & 4) &&
                            parameterPic2[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit32CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) | 8).ToString();
                        } 
                        else 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) ^ 8).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm2[15].Value) & 8) ==
                            (Convert.ToInt16(parameterPic2[15].Value) & 8) &&
                            parameterPic2[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                    case "bit42CheckBox":
                        if (feld.Checked == true) 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) | 16).ToString();
                        } 
                        else 
                        {
                            parameterForm2[15].Value = (Convert.ToInt16(parameterForm2[15].Value) ^ 16).ToString();
                        }
                        if ((Convert.ToInt16(parameterForm2[15].Value) & 16) ==
                            (Convert.ToInt16(parameterPic2[15].Value) & 16) &&
                            parameterPic2[15].Chapter == "ChannelSet") 
                        {
                            if (mainForm.writeUpdate == false) 
                            {
                                feld.ForeColor = Color.Green;
                            }
                        }
                        break;
                }

            }
        }


     


        /// <summary>
        /// Update der Felder in der Form
        /// </summary>
        /// <param name="registers">Structur der Registerwerte</param>
        ///// <param name="mainForm">Mainform</param>
        public void updateForm(ParameterSetsStruc[] registers, FormMain mainForm, Farbe farbe, bool testen)
        {

            farbenSetzen(mainForm, Farbe.black);

            Color farbeBasis = Color.Orange;
            Color farbeElement = Color.Black;
            switch (farbe) 
            {
                case Farbe.red:
                    farbeElement = Color.Red;
                    break;
                case Farbe.green:
                    farbeElement = Color.Green;
                    break;
                case Farbe.orange:
                    farbeElement = Color.Orange;
                    break;
            }

            //TODO: hier case für neuen Parameter hinzufügen
            // alle Parameter durchgehen und lt. array setzen
            foreach (ParameterSetsStruc register in registers) 
            {
                if (mainForm.tabControlParameter.SelectedIndex == 0) 
                {
                    switch (Convert.ToInt16(register.Command.Substring(8))) 
                    {
                        case 1:
                            mainForm.rollProp1NumericUpDown.ForeColor = farbeBasis; 
                            if (testen == true && mainForm.rollProp1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollProp1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[0].Value = register.Value;
                            mainForm.rollProp1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 2:
                            mainForm.rollInt1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollInt1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollInt1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[1].Value = register.Value;
                            mainForm.rollInt1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 3:
                            mainForm.rollDiff1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollDiff1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollDiff1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[2].Value = register.Value;
                            mainForm.rollDiff1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 4:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm1[3].Value = register.Value;
                                mainForm.rollLimit1NumericUpDown.Value = 1;
                                mainForm.rollLimit1NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.rollLimit1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.rollLimit1NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.rollLimit1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[3].Value = register.Value;
                                mainForm.rollLimit1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 5:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm1[4].Value = register.Value;
                                mainForm.rollIntLimit1NumericUpDown.Value = 1;
                                mainForm.rollIntLimit1NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.rollIntLimit1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.rollIntLimit1NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.rollIntLimit1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[4].Value = register.Value;
                                mainForm.rollIntLimit1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 6:
                            mainForm.nickProp1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickProp1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickProp1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[5].Value = register.Value;
                            mainForm.nickProp1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 7:
                            mainForm.nickInt1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickInt1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickInt1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[6].Value = register.Value;
                            mainForm.nickInt1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 8:
                            mainForm.nickDiff1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickDiff1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickDiff1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[7].Value = register.Value;
                            mainForm.nickDiff1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 9:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm1[8].Value = register.Value;
                                mainForm.nickLimit1NumericUpDown.Value = 1;
                                mainForm.nickLimit1NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.nickLimit1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.nickLimit1NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.nickLimit1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[8].Value = register.Value;
                                mainForm.nickLimit1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 10:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm1[9].Value = register.Value;
                                mainForm.nickIntLimit1NumericUpDown.Value = 1;
                                mainForm.nickIntLimit1NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.nickIntLimit1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.nickIntLimit1NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.nickIntLimit1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[9].Value = register.Value;
                                mainForm.nickIntLimit1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 11:
                            mainForm.gierProp1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierProp1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierProp1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[10].Value = register.Value;
                            mainForm.gierProp1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 12:
                            mainForm.gierInt1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierInt1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierInt1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[11].Value = register.Value;
                            mainForm.gierInt1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 13:
                            mainForm.gierDiff1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierDiff1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierDiff1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[12].Value = register.Value;
                            mainForm.gierDiff1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 14:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm1[13].Value = register.Value;
                                mainForm.gierLimit1NumericUpDown.Value = 1;
                                mainForm.gierLimit1NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.gierLimit1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.gierLimit1NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.gierLimit1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[13].Value = register.Value;
                                mainForm.gierLimit1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 15:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm1[14].Value = register.Value;
                                mainForm.gierIntLimit1NumericUpDown.Value = 1;
                                mainForm.gierIntLimit1NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.gierIntLimit1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.gierIntLimit1NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.gierIntLimit1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[14].Value = register.Value;
                                mainForm.gierIntLimit1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 16:
                            int temp = Convert.ToInt16(register.Value);
                            if (temp == -1)
                            {
                                temp = 0;
                            }
                            if ((temp & 1) == 1) 
                            {
                                mainForm.bit01CheckBox.ForeColor = farbeBasis;
                                mainForm.bit01CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit01CheckBox.ForeColor = farbeBasis;
                                mainForm.bit01CheckBox.Checked = false;
                            }
                            if ((temp & 2) == 2) 
                            {
                                mainForm.bit11CheckBox.ForeColor = farbeBasis;
                                mainForm.bit11CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit11CheckBox.ForeColor = farbeBasis;
                                mainForm.bit11CheckBox.Checked = false;
                            }
                            if ((temp & 4) == 4) 
                            {
                                mainForm.bit21CheckBox.ForeColor = farbeBasis;
                                mainForm.bit21CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit21CheckBox.ForeColor = farbeBasis;
                                mainForm.bit21CheckBox.Checked = false;
                            }
                            if ((temp & 8) == 8) 
                            {
                                mainForm.bit31CheckBox.ForeColor = farbeBasis;
                                mainForm.bit31CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit31CheckBox.ForeColor = farbeBasis;
                                mainForm.bit31CheckBox.Checked = false;
                            }
                            if ((temp & 16) == 16) 
                            {
                                mainForm.bit41CheckBox.ForeColor = farbeBasis;
                                mainForm.bit41CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit41CheckBox.ForeColor = farbeBasis;
                                mainForm.bit41CheckBox.Checked = false;
                            }
                            if (testen == true && parameterForm1[15].Value != register.Value) 
                            {
                                mainForm.bit01CheckBox.ForeColor = farbeElement;
                                mainForm.bit11CheckBox.ForeColor = farbeElement;
                                mainForm.bit21CheckBox.ForeColor = farbeElement;
                                mainForm.bit31CheckBox.ForeColor = farbeElement;
                                mainForm.bit41CheckBox.ForeColor = farbeElement;
                            }
                            parameterForm1[15].Value = temp.ToString();
                            break;
                        case 17:
                            mainForm.impulseAusgabe1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.impulseAusgabe1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.impulseAusgabe1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[16].Value = register.Value;
                            mainForm.impulseAusgabe1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 18:
                            if (Convert.ToInt16(register.Value) < 0)
                            {
                                parameterForm1[17].Value = register.Value;
                                mainForm.akku1NumericUpDown.Value = 0;
                                mainForm.akku1NumericUpDown.ForeColor = farbeBasis;
                            }
                            else
                            {
                                mainForm.akku1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.akku1NumericUpDown.Value !=
                                    Convert.ToInt16(register.Value))
                                {
                                    mainForm.akku1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[17].Value = register.Value;
                                mainForm.akku1NumericUpDown.Value = Convert.ToDecimal(Math.Round(Convert.ToDouble(register.Value), 1, MidpointRounding.AwayFromZero));
                            }
                            break;
                        case 19:
                            mainForm.rollMa1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollMa1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollMa1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[18].Value = register.Value;
                            mainForm.rollMa1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 20:
                            mainForm.nickMa1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickMa1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickMa1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[19].Value = register.Value;
                            mainForm.nickMa1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 21:
                            mainForm.gierMa1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierMa1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierMa1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[20].Value = register.Value;
                            mainForm.gierMa1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 22:
                            mainForm.gierA1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierA1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierA1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[21].Value = register.Value;
                            mainForm.gierA1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 23:
                            if (Convert.ToInt16(register.Value) < 5) 
                            {
                                parameterForm1[22].Value = register.Value;
                                mainForm.leerlaufgas1NumericUpDown.Value = 5;
                                mainForm.leerlaufgas1NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.leerlaufgas1NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.leerlaufgas1NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.leerlaufgas1NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm1[22].Value = register.Value;
                                mainForm.leerlaufgas1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 24:
                            mainForm.rollA1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollA1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollA1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[23].Value = register.Value;
                            mainForm.rollA1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 25:
                            mainForm.nickA1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickA1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickA1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[24].Value = register.Value;
                            mainForm.nickA1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 26:
                            mainForm.kamera1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.kamera1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.kamera1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[25].Value = register.Value;
                            mainForm.kamera1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 27:
                            mainForm.compass1NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.compass1NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.compass1NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm1[26].Value = register.Value;
                            mainForm.compass1NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                    }
                } 
                else 
                {
                    switch (Convert.ToInt16(register.Command.Substring(9))) 
                    {
                        case 1:
                            mainForm.rollProp2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollProp2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollProp2NumericUpDown.ForeColor = farbeElement;
                            }
                            mainForm.rollProp2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            parameterForm2[0].Value = register.Value;

                            break;
                        case 2:
                            mainForm.rollInt2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollInt2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollInt2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[1].Value = register.Value;
                            mainForm.rollInt2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 3:
                            mainForm.rollDiff2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollDiff2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollDiff2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[2].Value = register.Value;
                            mainForm.rollDiff2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 4:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm2[3].Value = register.Value;
                                mainForm.rollLimit2NumericUpDown.Value = 1;
                                mainForm.rollLimit2NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.rollLimit2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.rollLimit2NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.rollLimit2NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm2[3].Value = register.Value;
                                mainForm.rollLimit2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 5:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm2[4].Value = register.Value;
                                mainForm.rollIntLimit2NumericUpDown.Value = 1;
                                mainForm.rollIntLimit2NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.rollIntLimit2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.rollIntLimit2NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.rollIntLimit2NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm2[4].Value = register.Value;
                                mainForm.rollIntLimit2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 6:
                            mainForm.nickProp2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickProp2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickProp2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[5].Value = register.Value;
                            mainForm.nickProp2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 7:
                            mainForm.nickInt2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickInt2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickInt2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[6].Value = register.Value;
                            mainForm.nickInt2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 8:
                            mainForm.nickDiff2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickDiff2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickDiff2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[7].Value = register.Value;
                            mainForm.nickDiff2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 9:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm2[8].Value = register.Value;
                                mainForm.nickLimit2NumericUpDown.Value = 1;
                                mainForm.nickLimit2NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.nickLimit2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.nickLimit2NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.nickLimit2NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm2[8].Value = register.Value;
                                mainForm.nickLimit2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 10:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm2[9].Value = register.Value;
                                mainForm.nickIntLimit2NumericUpDown.Value = 1;
                                mainForm.nickIntLimit2NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.nickIntLimit2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.nickIntLimit2NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.nickIntLimit2NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm2[9].Value = register.Value;
                                mainForm.nickIntLimit2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 11:
                            mainForm.gierProp2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierProp2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierProp2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[10].Value = register.Value;
                            mainForm.gierProp2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 12:
                            mainForm.gierInt2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierInt2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierInt2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[11].Value = register.Value;
                            mainForm.gierInt2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 13:
                            mainForm.gierDiff2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierDiff2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierDiff2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[12].Value = register.Value;
                            mainForm.gierDiff2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 14:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                mainForm.gierLimit2NumericUpDown.Value = 1;
                                mainForm.gierLimit2NumericUpDown.ForeColor = farbeBasis;
                                parameterForm2[13].Value = register.Value;
                            } 
                            else 
                            {
                                mainForm.gierLimit2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.gierLimit2NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.gierLimit2NumericUpDown.ForeColor = farbeElement;
                                }
                                mainForm.gierLimit2NumericUpDown.Value = Convert.ToInt16(register.Value);
                                parameterForm2[13].Value = register.Value;
                            }
                            break;
                        case 15:
                            if (Convert.ToInt16(register.Value) <= 0) 
                            {
                                parameterForm2[14].Value = register.Value;
                                mainForm.gierIntLimit2NumericUpDown.Value = 1;
                                mainForm.gierIntLimit2NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.gierIntLimit2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.gierIntLimit2NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.gierIntLimit2NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm2[14].Value = register.Value;
                                mainForm.gierIntLimit2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 16:
                            int temp = Convert.ToInt16(register.Value);
                            if (temp == -1)
                            {
                                temp = 0;
                            }
                            if ((temp & 1) == 1) 
                            {
                                mainForm.bit02CheckBox.ForeColor = farbeBasis;
                                mainForm.bit02CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit02CheckBox.ForeColor = farbeBasis;
                                mainForm.bit02CheckBox.Checked = false;
                            }
                            if ((temp & 2) == 2) 
                            {
                                mainForm.bit12CheckBox.ForeColor = farbeBasis;
                                mainForm.bit12CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit12CheckBox.ForeColor = farbeBasis;
                                mainForm.bit12CheckBox.Checked = false;
                            }
                            if ((temp & 4) == 4) 
                            {
                                mainForm.bit22CheckBox.ForeColor = farbeBasis;
                                mainForm.bit22CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit22CheckBox.ForeColor = farbeBasis;
                                mainForm.bit22CheckBox.Checked = false;
                            }
                            if ((temp & 8) == 8) 
                            {
                                mainForm.bit32CheckBox.ForeColor = farbeBasis;
                                mainForm.bit32CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit32CheckBox.ForeColor = farbeBasis;
                                mainForm.bit32CheckBox.Checked = false;
                            }
                            if ((temp & 16) == 16) 
                            {
                                mainForm.bit42CheckBox.ForeColor = farbeBasis;
                                mainForm.bit42CheckBox.Checked = true;
                            } 
                            else 
                            {
                                mainForm.bit42CheckBox.ForeColor = farbeBasis;
                                mainForm.bit42CheckBox.Checked = false;
                            }
                            if (testen == true && parameterForm2[15].Value != register.Value) 
                            {
                                mainForm.bit02CheckBox.ForeColor = farbeElement;
                                mainForm.bit12CheckBox.ForeColor = farbeElement;
                                mainForm.bit22CheckBox.ForeColor = farbeElement;
                                mainForm.bit32CheckBox.ForeColor = farbeElement;
                                mainForm.bit42CheckBox.ForeColor = farbeElement;
                            }
                            parameterForm2[15].Value = temp.ToString();
                            break;
                        case 17:
                            mainForm.impulseAusgabe2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.impulseAusgabe2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.impulseAusgabe2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[16].Value = register.Value;
                            mainForm.impulseAusgabe2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 18:
                            if (Convert.ToInt16(register.Value) < 0)
                            {
                                parameterForm2[17].Value = register.Value;
                                mainForm.akku2NumericUpDown.Value = 0;
                                mainForm.akku2NumericUpDown.ForeColor = farbeBasis;
                            }
                            else
                            {
                                mainForm.akku2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.akku2NumericUpDown.Value !=
                                    Convert.ToInt16(register.Value))
                                {
                                    mainForm.akku2NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm2[17].Value = register.Value;
                                mainForm.akku2NumericUpDown.Value = Convert.ToDecimal(Math.Round(Convert.ToDouble(register.Value), 1, MidpointRounding.AwayFromZero));
                            }
                            break;
                        case 19:
                            mainForm.rollMa2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollMa2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollMa2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[18].Value = register.Value;
                            mainForm.rollMa2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 20:
                            mainForm.nickMa2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickMa2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickMa2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[19].Value = register.Value;
                            mainForm.nickMa2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 21:
                            mainForm.gierMa2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierMa2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierMa2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[20].Value = register.Value;
                            mainForm.gierMa2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 22:
                            mainForm.gierA2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.gierA2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.gierA2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[21].Value = register.Value;
                            mainForm.gierA2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 23:
                            if (Convert.ToInt16(register.Value) < 5) 
                            {
                                parameterForm2[22].Value = register.Value;
                                mainForm.leerlaufgas2NumericUpDown.Value = 5;
                                mainForm.leerlaufgas2NumericUpDown.ForeColor = farbeBasis;
                            } 
                            else 
                            {
                                mainForm.leerlaufgas2NumericUpDown.ForeColor = farbeBasis;
                                if (testen == true && mainForm.leerlaufgas2NumericUpDown.Value != 
                                    Convert.ToInt16(register.Value)) 
                                {
                                    mainForm.leerlaufgas2NumericUpDown.ForeColor = farbeElement;
                                }
                                parameterForm2[22].Value = register.Value;
                                mainForm.leerlaufgas2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            }
                            break;
                        case 24:
                            mainForm.rollA2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.rollA2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.rollA2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[23].Value = register.Value;
                            mainForm.rollA2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 25:
                            mainForm.nickA2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.nickA2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.nickA2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[24].Value = register.Value;
                            mainForm.nickA2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 26:
                            mainForm.kamera2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.kamera2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.kamera2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[25].Value = register.Value;
                            mainForm.kamera2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                        case 27:
                            mainForm.compass2NumericUpDown.ForeColor = farbeBasis;
                            if (testen == true && mainForm.compass2NumericUpDown.Value != 
                                Convert.ToInt16(register.Value)) 
                            {
                                mainForm.compass2NumericUpDown.ForeColor = farbeElement;
                            }
                            parameterForm2[26].Value = register.Value;
                            mainForm.compass2NumericUpDown.Value = Convert.ToInt16(register.Value);
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// fabe bei allen Feldern setzen
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="farbe"></param>
        public void farbenSetzen(FormMain mainForm, Farbe farbe)
        { 
            
            Color farbeElement = Color.Black;
            switch (farbe) 
            { 
                case Farbe.red:
                   farbeElement = Color.Red;
                    break;
                case Farbe.green:
                    farbeElement = Color.Green;
                    break;
            }

            //TODO: hier Parameter hinzufügen wenn neu
            if (mainForm.tabControlParameter.SelectedIndex == 0) 
            {
                mainForm.rollProp1NumericUpDown.ForeColor = farbeElement;
                mainForm.rollInt1NumericUpDown.ForeColor = farbeElement;
                mainForm.rollDiff1NumericUpDown.ForeColor = farbeElement;
                mainForm.rollLimit1NumericUpDown.ForeColor = farbeElement;
                mainForm.rollIntLimit1NumericUpDown.ForeColor = farbeElement;
                mainForm.nickProp1NumericUpDown.ForeColor = farbeElement;
                mainForm.nickInt1NumericUpDown.ForeColor = farbeElement;
                mainForm.nickDiff1NumericUpDown.ForeColor = farbeElement;
                mainForm.nickLimit1NumericUpDown.ForeColor = farbeElement;
                mainForm.nickIntLimit1NumericUpDown.ForeColor = farbeElement;
                mainForm.gierProp1NumericUpDown.ForeColor = farbeElement;
                mainForm.gierInt1NumericUpDown.ForeColor = farbeElement;
                mainForm.gierDiff1NumericUpDown.ForeColor = farbeElement;
                mainForm.gierLimit1NumericUpDown.ForeColor = farbeElement;
                mainForm.gierIntLimit1NumericUpDown.ForeColor = farbeElement;
                mainForm.bit01CheckBox.ForeColor = farbeElement;
                mainForm.bit11CheckBox.ForeColor = farbeElement;
                mainForm.bit21CheckBox.ForeColor = farbeElement;
                mainForm.bit31CheckBox.ForeColor = farbeElement;
                mainForm.bit41CheckBox.ForeColor = farbeElement;
                mainForm.impulseAusgabe1NumericUpDown.ForeColor = farbeElement;
                mainForm.akku1NumericUpDown.ForeColor = farbeElement;
                mainForm.rollMa1NumericUpDown.ForeColor = farbeElement;
                mainForm.nickMa1NumericUpDown.ForeColor = farbeElement;
                mainForm.gierMa1NumericUpDown.ForeColor = farbeElement;
                mainForm.gierA1NumericUpDown.ForeColor = farbeElement;
                mainForm.leerlaufgas1NumericUpDown.ForeColor = farbeElement;
                mainForm.rollA1NumericUpDown.ForeColor = farbeElement;
                mainForm.nickA1NumericUpDown.ForeColor = farbeElement;
                mainForm.kamera1NumericUpDown.ForeColor = farbeElement;
                mainForm.compass1NumericUpDown.ForeColor = farbeElement;
            } 
            else 
            {
                mainForm.rollProp2NumericUpDown.ForeColor = farbeElement;
                mainForm.rollInt2NumericUpDown.ForeColor = farbeElement;
                mainForm.rollDiff2NumericUpDown.ForeColor = farbeElement;
                mainForm.rollLimit2NumericUpDown.ForeColor = farbeElement;
                mainForm.rollIntLimit2NumericUpDown.ForeColor = farbeElement;
                mainForm.nickProp2NumericUpDown.ForeColor = farbeElement;
                mainForm.nickInt2NumericUpDown.ForeColor = farbeElement;
                mainForm.nickDiff2NumericUpDown.ForeColor = farbeElement;
                mainForm.nickLimit2NumericUpDown.ForeColor = farbeElement;
                mainForm.nickIntLimit2NumericUpDown.ForeColor = farbeElement;
                mainForm.gierProp2NumericUpDown.ForeColor = farbeElement;
                mainForm.gierInt2NumericUpDown.ForeColor = farbeElement;
                mainForm.gierDiff2NumericUpDown.ForeColor = farbeElement;
                mainForm.gierLimit2NumericUpDown.ForeColor = farbeElement;
                mainForm.gierIntLimit2NumericUpDown.ForeColor = farbeElement;
                mainForm.bit02CheckBox.ForeColor = farbeElement;
                mainForm.bit12CheckBox.ForeColor = farbeElement;
                mainForm.bit22CheckBox.ForeColor = farbeElement;
                mainForm.bit32CheckBox.ForeColor = farbeElement;
                mainForm.bit42CheckBox.ForeColor = farbeElement;
                mainForm.impulseAusgabe2NumericUpDown.ForeColor = farbeElement;
                mainForm.akku2NumericUpDown.ForeColor = farbeElement;
                mainForm.rollMa2NumericUpDown.ForeColor = farbeElement;
                mainForm.nickMa2NumericUpDown.ForeColor = farbeElement;
                mainForm.gierMa2NumericUpDown.ForeColor = farbeElement;
                mainForm.gierA2NumericUpDown.ForeColor = farbeElement;
                mainForm.leerlaufgas2NumericUpDown.ForeColor = farbeElement;
                mainForm.rollA2NumericUpDown.ForeColor = farbeElement;
                mainForm.nickA2NumericUpDown.ForeColor = farbeElement;
                mainForm.kamera2NumericUpDown.ForeColor = farbeElement;
                mainForm.compass2NumericUpDown.ForeColor = farbeElement;
            }
        }
            

    }
}
