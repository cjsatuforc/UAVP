using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace UAVPSet
{
    /// <summary>
    /// forumular für die Neutralwerte
    /// </summary>
    public partial class Receiver : Form
    {
        FormMain mainForm;
        PICConnect picConnect = new PICConnect();
        ArrayList ret;

        public Receiver(FormMain mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void gasLabel_TextChanged(object sender, EventArgs e)
        {
            gasTrackBar.Value = Convert.ToInt16(gasLabel.Text);
        }

        private void rollLabel_TextChanged(object sender, EventArgs e)
        {
            rollTrackBar.Value = Convert.ToInt16(rollLabel.Text);
        }

        private void nickLabel_TextChanged(object sender, EventArgs e)
        {
            nickTrackBar.Value = Convert.ToInt16(nickLabel.Text);
        }

        private void gierLabel_TextChanged(object sender, EventArgs e)
        {
            gierTrackBar.Value = Convert.ToInt16(gierLabel.Text);
        }

        private void ch5Label_TextChanged(object sender, EventArgs e)
        {
            ch5TrackBar.Value = Convert.ToInt16(ch5Label.Text);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ret = picConnect.askPic(mainForm, "R");
            Log.write(mainForm, "R: " + ret[1].ToString(), 0);
            // teilen der rückgabe
            string[] temp = ret[1].ToString().Split(':');
            // setzen der textboxen
            gasLabel.Text = temp[1].ToString().Substring(0, 3);
            rollLabel.Text = temp[2].ToString().Substring(0, 4);
            nickLabel.Text = temp[3].ToString().Substring(0, 4);
            gierLabel.Text = temp[4].ToString().Substring(0, 4);
            ch5Label.Text = temp[5].ToString().Substring(0, 3);

        }

        private void Receiver_FormClosing(object sender, FormClosingEventArgs e)
        {
            picConnect.sp.Close();
            timer1.Enabled = false;
        }

    }
}