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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace UAVP.UAVPSet
{
    /// <summary>
    /// Klasse um die Konfiguration in den Settings einzustellen und zu speichern
    /// </summary>
    public partial class Configuration : Form
    {

        FormMain mainForm; //Mainform um auf Log-TAB und Statusleiste zugreifen zu können

        /// <summary>
        /// Initialisierung der Klasse
        /// </summary>
        /// <param name="mainForm">MainForm</param>
        public Configuration(FormMain mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm; //in lokale Variable speichern
            // Alle werte aus den Settings den Steuerelementen zuweisen
            comPortComboBox.Text = Properties.Settings.Default.comPort; //ComPort
            loglevelComboBox.SelectedIndex = Properties.Settings.Default.logLevel; //LogLevel 1..Debug
            splashCheckBox.Checked = Properties.Settings.Default.spash; //soll Splash angezeigt werden
            timeOutMaskedTextBox.Text = Properties.Settings.Default.time.ToString(); //Timeout bei PIC read
            writeSleepMaskedTextBox.Text = Properties.Settings.Default.writeSleep.ToString(); //waren auf PIC Antwort beim Brennen
            askBurnPicCheckBox.Checked = Properties.Settings.Default.askBurnPic; //Sicherheitsabfrage beim Brennen
        }

        /// <summary>
        /// Beim Schließen des Formulares die Properties speichern
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Configuration_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        
        // Änderungen der Properties der Steuerelemente in den Properties mitziehen

        private void comPortComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.comPort = comPortComboBox.Text;
            mainForm.comPortToolStripComboBox.Text = comPortComboBox.Text;
        }

        private void loglevelComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.logLevel = loglevelComboBox.SelectedIndex;
        }

        private void splashCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.spash = splashCheckBox.Checked;
        }

        private void writeSleepMaskedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (writeSleepMaskedTextBox.Text == "") {
                Properties.Settings.Default.writeSleep = 0;
            } else {
                Properties.Settings.Default.writeSleep = Convert.ToInt32(writeSleepMaskedTextBox.Text);
            }
        }

        private void timeOutMaskedTextBox_TextChanged(object sender, EventArgs e)
        {
            if (timeOutMaskedTextBox.Text == "") {  //wenn Textbox leer ist muss setting 0 sein
                Properties.Settings.Default.time = 0;
            } else {
                Properties.Settings.Default.time = Convert.ToInt32(timeOutMaskedTextBox.Text);
            }
        }

        private void askBurnPicCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.askBurnPic = askBurnPicCheckBox.Checked;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}