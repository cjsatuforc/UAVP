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
using System.Resources;
using System.Windows.Forms;

namespace UAVPSet
{
    /// <summary>
    /// statische Klasse um Logmeldungen auszugeben
    /// </summary>
    static class Log
    {
        /// <summary>
        /// Funtkion um Logtext zu schreiben
        /// </summary>
        /// <param name="mainForm"></param>
        /// <param name="logtext"></param>
        /// <param name="level"></param>
        public static void write(FormMain mainForm, string logtext, int level)
        {

            // ausgabe nur wenn eingestellter Loglevel größer ist
            if (Properties.Settings.Default.logLevel >= level && logtext != null)
            {
                // bei den pic zeilen wird ein \n mitgegeben - bei anderen nicht
                // deshalb immer löschen und komplett hinzufügen
                string temp = logtext;
                if ((temp.Substring(logtext.Length - 1) == "\r") && (logtext.Length >= 1))
                {
                    logtext = logtext.Substring(0, logtext.Length - 1);
                }
                mainForm.logTextBox.Text += DateTime.Now + ": "+ logtext + "\r\n";
                // auf das ende der Textbox scrollen
                mainForm.logTextBox.SelectionStart = mainForm.logTextBox.Text.Length;
                mainForm.logTextBox.ScrollToCaret();
                if (level == 1)
                {
                    // Dateiname ermitteln
                    string fileName = Path.Combine(mainForm.pfad, "DebugLog.txt");

                    //// Überprüfen, ob die Datei bereits existiert
                    //if (File.Exists(fileName))
                    //{
                    //    File.Delete(fileName);
                    //}
                    //File.Create(fileName);

                    // StreamWriter-Instanz für die Datei erzeugen
                    StreamWriter sw = null;
                    try
                    {
                        sw = new StreamWriter(fileName, true, Encoding.GetEncoding("windows-1252"));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(mainForm.errorLabels.GetString("errorLog") + "\r\n" + e.ToString(), "Error!", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        return;
                    }

                    // Datei zeilenweise schreiben
                    sw.WriteLine(DateTime.Now + ": " + logtext + "\r\n");

                    // StreamWriter schließen
                    sw.Close();
                }
            }
        }
    }
}
