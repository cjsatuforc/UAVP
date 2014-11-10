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
using System.IO;
using System.IO.Ports;
using System.Windows.Forms;
using System.Drawing;
using System.Resources;
using System.Collections;
using System.Threading;

namespace UAVPSet
{
    /// <summary>
    /// Klasse die die gesamte Kommunikation mit dem PIC abbildet 
    /// </summary>
    class PICConnect
    {
        public PICConnect()
        {
            // laden der Texte für Ausgabe je nach Sprache
            labels = new ResourceManager("UAVPSet.Resources.language", this.GetType().Assembly);
            errorLabels = new ResourceManager("UAVPSet.Resources.error", this.GetType().Assembly);
        }
        
        // Texte für Mehrsprachigkeit
        ResourceManager labels;
        ResourceManager errorLabels;
        // Serielle Schnittstelle initialisieren
        public SerialPort sp = new SerialPort();

        /// <summary>
        /// verbinden mit PIC
        /// </summary>
        /// <param name="mainForm"></param>
        public void connect(FormMain mainForm)
        {

            // wenn Verbindung noch nicht offen ist
            if (!sp.IsOpen)
            {
                //TODO: wird zur Zeit noch nicht aus den Einstellungen übernommen (nur Port)
                sp.PortName = Properties.Settings.Default.comPort;
                sp.BaudRate = 38400;
                sp.StopBits = StopBits.One;
                sp.DataBits = 8;
                sp.Parity = Parity.None;
                sp.Handshake = Handshake.None;
                sp.ReadTimeout = Convert.ToInt16(Properties.Settings.Default.time);

                Log.write(mainForm, Properties.Settings.Default.comPort,1);
                
                
                try
                {
                    sp.Open();
                    // testen der Verbindung über ?
                    testComm(mainForm);
                    // schreiben in Status und log
                    mainForm.infoleisteToolStripStatusLabel.Text = labels.GetString("picConnected");
                    Log.write(mainForm, labels.GetString("picConnected"), 0);
                    // austauschen text und textfarbe bei listview
                    mainForm.listViewJobs.Items[0].Text = labels.GetString("listviewConnected");
                    mainForm.listViewJobs.Items[0].ForeColor = Color.Green;
                }
                catch(Exception e)
                {
                    MessageBox.Show(errorLabels.GetString("connect") + "\r\n" + e.ToString());
                    Log.write(mainForm, e.ToString(),1);
                }


            }
            else
            {
                // wenn bereits verbunden dann info
                mainForm.infoleisteToolStripStatusLabel.Text = labels.GetString("alreadyConnected");
                Log.write(mainForm, labels.GetString("alreadyConnected"), 0);
            }
        }

        /// <summary>
        /// lesen der Parameter
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="parameter"></param>
        public void readParameters(FormMain mainForm, ParameterSets parameter)
        {

            // Cursor auf wait setzen
            Cursor.Current = Cursors.WaitCursor;


            // Abfrage der Prameter mit L und speichern in array
            ArrayList para = askPic(mainForm, "L");
            
            // wenn Loglevel auf debug - deshalb mit if damit foreach nicht so oft aufgerufen wird
            if (Properties.Settings.Default.logLevel > 0)
            {
                foreach (string output in para)
                {
                    Log.write(mainForm, output, 1);

                }
            }
            // standardausgabe
            else
            {
                Log.write(mainForm, para[1].ToString(), 0);
            }

            // springen auf richtiges TAB
            mainForm.tabControlParameter.SelectedIndex = Convert.ToInt16(para[1].ToString().Substring(24, 1)) - 1;
            
            // markieren des TABS
            if (mainForm.tabControlParameter.SelectedIndex == 0)
            {
                mainForm.tabPageParameterSet1.Text = labels.GetString("paraSetAktiv1");
                mainForm.tabPageParameterSet2.Text = labels.GetString("paraSet2");
            }
            else
            {
                mainForm.tabPageParameterSet1.Text = labels.GetString("paraSet1");
                mainForm.tabPageParameterSet2.Text = labels.GetString("paraSetAktiv2");
            }

            // wenn tab Parameter Set 1 gelesen wird
            if (mainForm.tabControlParameter.SelectedIndex == 0)
            {
                //TODO: hier die Schleife erweitern wenn neuer Parameter hinzukommt
                // durchgehen des arrays und in Prameter Struc speichern
                for (int i = 0; i < para.Count-2; i++)
                {
                    // + in den Parametern ausschneiden
                    if (para[i+2].ToString().Substring(15, 1) == "+")
                    {
                        para[i+2] = Convert.ToInt16(para[i+2].ToString().Substring(16, 3));
                        parameter.parameterPic1[i].Command = "Register "+(i+1).ToString();
                        parameter.parameterPic1[i].Value = para[i+2].ToString();
                    }
                    else 
                    {
                        para[i+2] = Convert.ToInt16(para[i+2].ToString().Substring(15, 4));
                        parameter.parameterPic1[i].Command = "Register " + (i+1).ToString();
                        parameter.parameterPic1[i].Value = para[i+2].ToString(); 
                    }
                }
                // ganzes Form mit den Daten updaten
                parameter.updateForm(parameter.parameterPic1, mainForm, ParameterSets.Farbe.green);
            }
            // gleiches wenn Parameter Set 2 gelesen wird
            else
            {
                //TODO: Auch hier die Schleife erweitern wenn Parameter hinzukommt
                // durchgehen des arrays und in Prameter Struc speichern
                for (int i = 0; i < para.Count-2; i++)
                {
                    // + in den Parametern ausschneiden
                    if (para[i+2].ToString().Substring(15, 1) == "+")
                    {
                        para[i+2] = Convert.ToInt16(para[i+2].ToString().Substring(16, 3));
                        parameter.parameterPic2[i].Command = "Register " + (i+1).ToString();
                        parameter.parameterPic2[i].Value = para[i+2].ToString();
                    }
                    else 
                    {
                        para[i+2] = Convert.ToInt16(para[i+2].ToString().Substring(15, 4));
                        parameter.parameterPic2[i].Command = "Register " + (i + 1).ToString();
                        parameter.parameterPic2[i].Value = para[i+2].ToString();
                    }
                }
                // ganzes Form mit den Daten updaten
                parameter.updateForm(parameter.parameterPic2, mainForm, ParameterSets.Farbe.green);

                // Cursor auf default setzen
                Cursor.Current = Cursors.Default;
            }
            
            
            
        }

        /// <summary>
        /// werte an Pic schreiben
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="parameter"></param>
        public void writeParameters(FormMain mainForm, ParameterSets parameter)
        {

            // Verbindung öffnen wenn noch nicht verbunden
            if (!sp.IsOpen)
            {
                connect(mainForm);
            }
            // Progressbar einblenden und Cursor auf wait setzen
            mainForm.toolStripProgressBar.Visible = true;
            Cursor.Current = Cursors.WaitCursor;

            // wenn Prameter Set 1
            if (mainForm.tabControlParameter.SelectedIndex == 0)
            {
                // alle Parameter schreiben
                foreach(ParameterSets.ParameterSetsStruc value in parameter.parameterForm1)
                {
                    parameterWrite(mainForm, value);
                }

            }
            // wenn SET 2 
            else
            {
                // alle Parameter schreiben
                foreach (ParameterSets.ParameterSetsStruc value in parameter.parameterForm2)
                {
                    parameterWrite(mainForm, value);
                }


            }
            // Progressbar ausblenden und Cursor auf default setzen
            mainForm.toolStripProgressBar.Value = 0;
            mainForm.toolStripProgressBar.Visible = false;
            Cursor.Current = Cursors.Default;


            //TODO: nur wenn OK Werte auf grün setzen
            parameter.farbenSetzen(mainForm, ParameterSets.Farbe.green);
        }




        /// <summary>
        /// SChreiben der Parameter für Funktion writeParameter
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="value"></param>
        private void parameterWrite(FormMain mainForm, ParameterSets.ParameterSetsStruc value)
        {
            try
            {
                string temp;
                sp.WriteLine("M");
                // log über sendekey
                Log.write(mainForm, "send: \"M\"", 1);
                temp = sp.ReadLine();
                // alle inputs durchgehen
                while (temp != "Register ")
                {
                    temp = sp.ReadLine();
                    // log schreiben wenn debuglevel
                    Log.write(mainForm, temp, 1);
                }
                sp.WriteLine(Convert.ToInt16(value.Command.Substring(9)).ToString("00"));
                temp = sp.ReadLine();
                while (temp != (Convert.ToInt16(value.Command.Substring(9)).ToString("00") + " = "))
                {
                    temp = sp.ReadLine();
                    // log schreiben wenn debuglevel
                    Log.write(mainForm, temp, 1);
                }
                sp.WriteLine(Convert.ToInt16(value.Value).ToString("00"));
                temp = sp.ReadLine();
                while (temp != ">")
                {
                    temp = sp.ReadLine();
                    // log schreiben wenn debuglevel
                    Log.write(mainForm, temp, 1);
                }
                // log über sendekey
                Log.write(mainForm, (labels.GetString("writeParameter") +
                    Convert.ToInt16(value.Command.Substring(9)).ToString("00") + " = " +
                    Convert.ToInt16(value.Value).ToString("00")), 0);

                // Progressbar hochzählen
                mainForm.toolStripProgressBar.Increment(4);
                // Infotext in status schreiben
                mainForm.infoleisteToolStripStatusLabel.Text = labels.GetString("writeParameter") + Convert.ToInt16(value.Command.Substring(9)).ToString("00") + " = " + Convert.ToInt16(value.Value).ToString("00");
                Application.DoEvents();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }



        /// <summary>
        /// lesen welches SET aktiv am PIC ist 
        /// </summary>
        /// <param name="mainForm"></param>
        public void parameterSet(FormMain mainForm)
        {

            // pic fragen
            ArrayList setup = askPic(mainForm, "S");

            // logausgabe
            if (Properties.Settings.Default.logLevel > 0)
            {
                foreach (string output in setup)
                {
                    Log.write(mainForm, output, 1);

                }
            }
            else 
            {
                // wenn keine Verbindung besteht gibt es kein setup[5]
                if (setup[5] != null)
                {
                    Log.write(mainForm, setup[5].ToString(), 0);
                }
                else
                {
                    Log.write(mainForm, "Error: Setup[5] = null", 1);
                }
            }

            // aktives TAB selectieren
            mainForm.tabControlParameter.SelectedIndex = Convert.ToInt16(setup[5].ToString().Substring(24,1))-1;
            if (mainForm.tabControlParameter.SelectedIndex == 0)
            {
                mainForm.tabPageParameterSet1.Text = labels.GetString("paraSetAktiv1");
                mainForm.tabPageParameterSet2.Text = labels.GetString("paraSet2");
            }
            else
            {
                mainForm.tabPageParameterSet1.Text = labels.GetString("paraSet1");
                mainForm.tabPageParameterSet2.Text = labels.GetString("paraSetAktiv2");
            }
        }

        /// <summary>
        /// Verbindung mit PIC über ? testen
        /// </summary>
        /// <param name="mainForm"></param>
        /// <returns></returns>
        public ArrayList testComm(FormMain mainForm)
        {
            ArrayList ret = askPic(mainForm, "?");
            Log.write(mainForm, "Function testCom:", 1);
            
            // logausgabe
            if (Properties.Settings.Default.logLevel > 0)
            {
                foreach (string output in ret)
                {
                    Log.write(mainForm, output, 1);
                    
                }
            }
            return ret;
        }

        /// <summary>
        /// senden einer Anfrage an den PIC und Ergebnis zurückgeben
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public ArrayList askPic(FormMain mainForm, string key)
        {

            // wenn noch keine Verbindung besteht automatisch öffnen
            if (!sp.IsOpen)
            {
                connect(mainForm);
            }
            ArrayList ret = new ArrayList();
            string temp;

            try
            {
                sp.WriteLine(key);
                // log über sendekey
                Log.write(mainForm, labels.GetString("writeParameter") + key, 1);
                temp = sp.ReadLine();
                while (temp != ">")
                {
                    ret.Add(temp);
                    temp = sp.ReadLine();
                    // log schreiben wenn debuglevel
                    Log.write(mainForm, temp, 1);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                Log.write(mainForm, e.ToString(), 1);
            }
            return ret;
        }

        /// <summary>
        /// funktion um die Neutralwerte aus pic zu lesen
        /// </summary>
        /// <param name="mainForm"></param>
        public void neutral(FormMain mainForm)
        {
            ArrayList ret = askPic(mainForm, "N");
            Log.write(mainForm, "N: " + ret[1].ToString(), 0);
            Neutral neutralWindow = new Neutral(mainForm);
            neutralWindow.neutralLabel.Text = ret[1].ToString();
            neutralWindow.ShowDialog();
        }

        /// <summary>
        /// Receiver Werte lesen
        /// </summary>
        /// <param name="mainForm"></param>
        public void receiver(FormMain mainForm)
        {
            ArrayList ret = askPic(mainForm, "R");
            Log.write(mainForm, "R: " + ret[1].ToString(), 0);
            Receiver receiverWindow = new Receiver(mainForm);
            // teilen der rückgabe
            string [] temp = ret[1].ToString().Split(':');
            // setzen der textboxen
            sp.Close();
            receiverWindow.gasLabel.Text = temp[1].ToString().Substring(0, 3);
            receiverWindow.rollLabel.Text = temp[2].ToString().Substring(0, 4);
            receiverWindow.nickLabel.Text = temp[3].ToString().Substring(0, 4);
            receiverWindow.gierLabel.Text = temp[4].ToString().Substring(0, 4);
            receiverWindow.ch5Label.Text = temp[5].ToString().Substring(0, 3);
            // dialog anzeigen
            receiverWindow.ShowDialog();
        }


        public void burnPic(FormMain mainForm, string fileName)
        {

            //Hex laden
            // StreamReader-Instanz für die Datei erzeugen
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(fileName, Encoding.GetEncoding("windows-1252"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Log.write(mainForm, ex.ToString(), 1);
                return;
            }

            // Datei zeilenweise einlesen
            
            ArrayList hexFile = new ArrayList();
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                hexFile.Add(line);
            }

            Log.write(mainForm, ("Read File: " + fileName), 0);
            Log.write(mainForm, ("Lines: " + hexFile.Count.ToString()), 1);


            // Progressbar einblenden und Cursor auf wait setzen
            mainForm.toolStripProgressBar.Visible = true;
            Cursor.Current = Cursors.WaitCursor;

            // Brennen in PIC
            // wenn noch keine Verbindung besteht automatisch öffnen
            if (!sp.IsOpen)
            {
                connect(mainForm);
            } 


            Log.write(mainForm, Properties.Settings.Default.comPort, 1);

                        
            int i = 1;
            string temp;
            
            sp.WriteLine("B");
            temp = sp.ReadLine();
            Log.write(mainForm, temp, 1);
            mainForm.toolStripProgressBar.Maximum = Convert.ToInt16(hexFile.Count);

            try
            {
                foreach (string zeile in hexFile)
                {
                    Log.write(mainForm, zeile, 1);
                    // Progressbar hochzählen
                    mainForm.toolStripProgressBar.Increment(1);
                    // Infotext in status schreiben
                    mainForm.infoleisteToolStripStatusLabel.Text = labels.GetString("writeLine") +
                        " " + i.ToString() + " " + labels.GetString("writeMax") + " " + Convert.ToInt16(hexFile.Count) + "  ";
                    // Infotext in Log schreiben
                    Log.write(mainForm, ("Line: " + i.ToString() + ": " + zeile), 0);
                    Application.DoEvents();
                    i++;

                    // zeile senden
                    sp.WriteLine(zeile);
                    for (int j = 1; j < 4; j++)
                    {
                        // etwas warten bis der PIC die Daten verarbeitet hat und bei jedem Versuch länger!
                        Thread.Sleep(Properties.Settings.Default.writeSleep * j * j);
                        temp = sp.ReadLine();
                        Log.write(mainForm, temp, 1);
                        if (temp == "ERR\r")
                        {
                            // zeile senden
                            sp.WriteLine(zeile);
                            Log.write(mainForm, ("Try: " + (j).ToString() + " " + zeile), 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (temp == "ERR\r")
                    {
                        MessageBox.Show(errorLabels.GetString("flashError"));
                        break;
                    }
                    // log schreiben wenn debuglevel
                    Log.write(mainForm, temp, 1);


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                Log.write(mainForm, e.ToString(), 1);
            }

            // Info um Board neu zu starten
            MessageBox.Show(labels.GetString("flashInfo"));

            // Progressbar ausblenden und Cursor auf default setzen
            mainForm.toolStripProgressBar.Value = 0;
            mainForm.toolStripProgressBar.Visible = false;
            mainForm.toolStripProgressBar.Maximum = 100;
            Cursor.Current = Cursors.Default;
            
        }


    }
}
