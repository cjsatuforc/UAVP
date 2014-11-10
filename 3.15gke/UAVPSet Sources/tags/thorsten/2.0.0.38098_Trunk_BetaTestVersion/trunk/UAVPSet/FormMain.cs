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
using System.Resources;

namespace UAVPSet
{
    public partial class FormMain : Form
    {


        // Eigene Objekte erzeugen
        ParameterSets parameterSets = new ParameterSets();
        PICConnect picConnect = new PICConnect();
        public Cursor cursor = Cursor.Current;
        public ResourceManager help;
        // Texte für Mehrsprachigkeit
        ResourceManager labels;
        public ResourceManager errorLabels;
        // bei write feldupdate auf grün ausschalten
        public bool writeUpdate = false;
        //Applikationspfad
        public string pfad;


        
        public FormMain()
        {
            InitializeComponent();
            // sprachtexte für hilfe
            help = new ResourceManager("UAVPSet.Resources.hilfe", this.GetType().Assembly);
            // sprachtexte für ausgabelabels
            labels = new ResourceManager("UAVPSet.Resources.language", this.GetType().Assembly);
            errorLabels = new ResourceManager("UAVPSet.Resources.error", this.GetType().Assembly);
            pfad = Application.StartupPath;
            comPortToolStripComboBox.Text = Properties.Settings.Default.comPort;

        }

        /// <summary>
        /// wenn in der ListView ein element angeklickt wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewJobs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewJobs.SelectedItems.Count == 0) return;

            if (listViewJobs.SelectedItems[0].Index == 0)
            {
                picConnect.connect(this);
            }
            if (listViewJobs.SelectedItems[0].Index == 1)
            {
                picConnect.parameterSet(this);
            }
            if (listViewJobs.SelectedItems[0].Index == 2)
            {
                picConnect.readParameters(this, parameterSets, false);
            }
            if (listViewJobs.SelectedItems[0].Index == 3)
            {
                picConnect.writeParameters(this, parameterSets);
            }
            if (listViewJobs.SelectedItems[0].Index == 4)
            {
                picConnect.neutral(this);
            }
            if (listViewJobs.SelectedItems[0].Index == 5)
            {
                picConnect.receiver(this);
            }

        }

        /// <summary>
        /// Funktion um die Volt bei der Akku Unterspannung zu errechnen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void akku1NumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            double akkuValue1 = Math.Round((double)akku1NumericUpDown.Value * 4.6d,0,MidpointRounding.AwayFromZero);
            akkuValue1Label.Text = akkuValue1.ToString();
        }


        


        /// <summary>
        /// Funktion um die Labels bei den Checkboxen Zentral zu ändern
        /// </summary>
        /// <param name="changeBox">zu ändernde Checkbox als Object</param>
        private void bitTextWechsel(Object changeBoxObject)
        {
            CheckBox changeBox = (CheckBox)changeBoxObject;
            if (changeBox.Checked == false)
                switch (changeBox.Name.Substring(0,4))
                {
                    case "bit0":
                        changeBox.Text = labels.GetString("bit0");
                        break;
                    case "bit1":
                        changeBox.Text = labels.GetString("bit1");
                        break;
                    case "bit2":
                        changeBox.Text = labels.GetString("bit2");
                        break;
                    case "bit3":
                        changeBox.Text = labels.GetString("bit3");
                        break;
                    case "bit4":
                        changeBox.Text = labels.GetString("bit4");
                        break;
                }
            else
            {
                switch (changeBox.Name.Substring(0, 4))
                {
                    case "bit0":
                        changeBox.Text = labels.GetString("bit01");
                        break;
                    case "bit1":
                        changeBox.Text = labels.GetString("bit11");
                        break;
                    case "bit2":
                        changeBox.Text = labels.GetString("bit21");
                        break;
                    case "bit3":
                        changeBox.Text = labels.GetString("bit31");
                        break;
                    case "bit4":
                        changeBox.Text = labels.GetString("bit41");
                        break;

                }
            }
        }



        //reagieren auf änderung in den Werten
        private void bit01CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit11CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit21CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit31CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit41CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit02CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit12CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit22CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit32CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void bit42CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            bitTextWechsel(sender);
        }

        private void cOMPortToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Configuration configwindow = new Configuration(this);
            configwindow.ShowDialog();
        }

        
        /// <summary>
        /// Laden der Parameterwerte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configLadenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Parameter laden
            parameterOpenFileDialog.Filter = "auv - Setupdateien (*.auv)|*.auv|alle Dateien (*.*)|*.*";
            parameterOpenFileDialog.InitialDirectory = Properties.Settings.Default.openFolder;
            if (parameterOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                parameterSets.laden(parameterOpenFileDialog.FileName, this);
                Properties.Settings.Default.openFolder = parameterOpenFileDialog.InitialDirectory;
            }
        }

        /// <summary>
        /// allgemeine Funktion um alle Felder upzudaten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feldUpdaten_Click_KeyDown(object sender, EventArgs e)
        {
            parameterSets.feldUpdaten(sender, this);
            akkuValue1Label.Text = (Decimal.Round(akku1NumericUpDown.Value / 4.6m, 2)).ToString();
            akkuValue2Label.Text = (Decimal.Round(akku2NumericUpDown.Value / 4.6m, 2)).ToString();

        }


        /// <summary>
        /// allgemeine Funktion um alle Felder upzudaten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void feldUpdaten_KeyDown(object sender, KeyEventArgs e)
        {
            parameterSets.feldUpdaten(sender, this);
            akkuValue1Label.Text = (Decimal.Round(akku1NumericUpDown.Value / 4.6m, 2)).ToString();
            akkuValue2Label.Text = (Decimal.Round(akku2NumericUpDown.Value / 4.6m, 2)).ToString();
        
        }

        /// <summary>
        /// speichern der Parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void configSpeichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Parameter speichern
            parameterSaveFileDialog.Filter = "auv - Setupdateien (*.auv)|*.auv|alle Dateien (*.*)|*.*";
            parameterSaveFileDialog.InitialDirectory = Properties.Settings.Default.saveFolder;
            if (parameterSaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                parameterSets.speichern(parameterSaveFileDialog.FileName, this);
                Properties.Settings.Default.saveFolder = parameterSaveFileDialog.InitialDirectory;
            }
        }

        /// <summary>
        /// anzeige von Programminfos
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info info = new Info();
            info.ShowDialog();
        }

        /// <summary>
        /// anzeige der Hilfe für die Werte
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void infoGetFocus(object sender, EventArgs e)
        {
            Hilfe.info(this);
        }

        /// <summary>
        /// anzeige der Configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void logLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuration configwindow = new Configuration(this);
            configwindow.configurationTabControl.SelectedIndex = 1;
            configwindow.ShowDialog();
        }

        /// <summary>
        /// Aufruf über Menü
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picConnect.connect(this);
        }

        private void parameterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picConnect.parameterSet(this);
        }

        private void readConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picConnect.readParameters(this, parameterSets, false);
        }

        private void writeConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picConnect.writeParameters(this, parameterSets);
        }

        private void neutralToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picConnect.neutral(this);
        }

        private void peceiverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picConnect.receiver(this);
        }

        private void burnPicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // sicherheitsabfrage
            if (Properties.Settings.Default.askBurnPic == true)
            {
                if (MessageBox.Show(labels.GetString("savePicBurn"), "Flash PIC?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    burn();
                }
            } else
            {
                burn();
            }
        }
        
        /// <summary>
        /// auslager für brunPicToolStripMenuItem_Click
        /// </summary>
        void burn()
        {
            //Hex laden
            hexOpenFileDialog.Filter = "hex - Firmware (*.hex)|*.hex|alle Dateien (*.*)|*.*";
            hexOpenFileDialog.InitialDirectory = Properties.Settings.Default.pathHex;
            if (hexOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                picConnect.burnPic(this, hexOpenFileDialog.FileName);
                Properties.Settings.Default.pathHex = hexOpenFileDialog.InitialDirectory;
            }
        }

        private void miscellaneousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Configuration configwindow = new Configuration(this);
            configwindow.configurationTabControl.SelectedIndex = 2;
            configwindow.ShowDialog();
        }

        private void comPortToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.comPort = comPortToolStripComboBox.Text;
            Properties.Settings.Default.Save();
        }





        
    }
}