using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.IO.Ports;

namespace UAVP.UAVPSet
{
    public partial class TestSoft : Form
    {
        FormMain mainForm;
        PICConnect picConnect = new PICConnect();
        public bool errorFlag = false;
        public SerialPort sp = new SerialPort();

       
        public TestSoft(FormMain mainForm, PICConnect picConnect)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.picConnect = picConnect;
            picConnect.connect(mainForm, true);
        }

        private void lesebutton1_Click(object sender, EventArgs e)
        {
           lesebutton1.Enabled = false;
           mainForm.Cursor = Cursors.WaitCursor;
           textBox1.Text = "";
           ArrayList setup = picConnect.askPic(mainForm, "S");
           // erste  und letzte 2 Zeilen löschen
           setup.RemoveAt(0);
           setup.RemoveAt(setup.Count - 1);
           setup.RemoveAt(setup.Count - 1);
           // anzeige des Aktuellen Setups
           foreach (string setups in setup)
            {
                textBox1.Text += setups + "\n";
                textBox1.SelectionStart = textBox1.Text.Length;
                textBox1.ScrollToCaret();
            }

           ArrayList analogwerte = picConnect.askPic(mainForm, "A");
           // erste und letzten 2 zeilen löschen
           analogwerte.RemoveAt(0);
           analogwerte.RemoveAt(0);
           analogwerte.RemoveAt(analogwerte.Count - 1);
           analogwerte.RemoveAt(analogwerte.Count - 1);
           // Ausgabe der Analogwerte
           label5.Text = analogwerte[0].ToString().Substring(4, 5);
           label6.Text = analogwerte[1].ToString().Substring(4, 5);
           label7.Text = analogwerte[2].ToString().Substring(4, 5);
           label8.Text = analogwerte[3].ToString().Substring(4, 5);
           label55.Text = analogwerte[4].ToString().Substring(4, 5);
           
           
           // Ausgabe der Linearwerte
           ArrayList linearwerte = picConnect.askPic(mainForm, "L");
           // ersten 2 zeilen löschen
           linearwerte.RemoveAt(0);
           linearwerte.RemoveAt(0);
           label20.Text = linearwerte[1].ToString().Substring(2, 7);
           label21.Text = linearwerte[2].ToString().Substring(2, 7);
           label22.Text = linearwerte[3].ToString().Substring(2, 7);
           // Status byte mus min 0x07 haben ,hier sollte eine Umwandlung von Hex in int rein ?
           label23.Text = linearwerte[0].ToString().Substring(4, 2);

           // Ausgabe des Kompass
           ArrayList kompass = picConnect.askPic(mainForm, "C");
           // erste und letzten 2 zeilen löschen
           kompass.RemoveAt(0);
           kompass.RemoveAt(0);
           label14.Text = kompass[0].ToString().Substring(0, 3);

           // Ausgabe der Receiverwerte
           ArrayList receiverwerte = picConnect.askPic(mainForm, "R");
           // ersten 1 zeilen löschen
           receiverwerte.RemoveAt(0);
           receiverwerte.RemoveAt(0);
           label36.Text = receiverwerte[0].ToString().Substring(5, 3);
           label37.Text = receiverwerte[1].ToString().Substring(5, 3);
           label38.Text = receiverwerte[2].ToString().Substring(5, 3);
           label39.Text = receiverwerte[3].ToString().Substring(5, 3);
           label40.Text = receiverwerte[4].ToString().Substring(5, 3);
           label41.Text = receiverwerte[5].ToString().Substring(5, 3);
           label42.Text = receiverwerte[6].ToString().Substring(5, 3);
           label43.Text = receiverwerte[7].ToString().Substring(2, 6);
           
           mainForm.Cursor = Cursors.Default;
           lesebutton1.Enabled = true; 
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // String message = "Wollen Sie diesen Test wirklich durchführen\n        Bitte die Propeller entfernen !!!";
           // String caption = "Warnung";
            String message = "Dieser Test wird noch nicht unterstützt";
            String caption = "Warnung";
            MessageBoxButtons buttons = MessageBoxButtons.OK;
            DialogResult result;

            result = MessageBox.Show(this, message, caption, buttons);

            if (result == DialogResult.Yes)
            {
                //ArrayList servotest = picConnect.askPic(mainForm, "v");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mainForm.Cursor = Cursors.WaitCursor;
            button2.Enabled = false; 
            ArrayList ledblau = picConnect.askPic(mainForm, "2");
            vuMeter1.Level = 1;
            ArrayList ledrot = picConnect.askPic(mainForm, "3");
            vuMeter2.Level = 1;
            ArrayList ledgruen = picConnect.askPic(mainForm, "4");
            vuMeter3.Level = 1;
            ArrayList ledgelb = picConnect.askPic(mainForm, "6");
            vuMeter4.Level = 1;
            ArrayList ledbeepergruen = picConnect.askPic(mainForm, "7");
            ArrayList ledbeeperblack = picConnect.askPic(mainForm, "8");
            
            mainForm.Cursor = Cursors.Default;
            button2.Enabled = true;
            vuMeter1.Level = 0;
        }

    }
}