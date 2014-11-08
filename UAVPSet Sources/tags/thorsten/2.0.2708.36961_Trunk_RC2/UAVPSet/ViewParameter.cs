using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace UAVP.UAVPSet
{
    /// <summary>
    /// in diesem form k�nnen die Parameter ge�ndert oder kopiert werden
    /// achtung es wird nicht gepfr�ft
    /// </summary>
    public partial class ViewParameter : Form
    {
        ParameterSets parameterSets;
        FormMain mainForm;
        string para1 = "";
        string para2 = "";

        public ViewParameter(ParameterSets parameterSets, FormMain mainForm)
        {
            InitializeComponent();
            this.parameterSets = parameterSets;
            this.mainForm = mainForm;
            fillParameter();
        }

        /// <summary>
        /// eintragen der Parameter in die Textboxen
        /// </summary>
        void fillParameter()
        {
            int i = 1;
            foreach (ParameterSets.ParameterSetsStruc para in parameterSets.parameterForm1) {
                para1 += i + "=" + para.Value + ";";
                i++;
            }
            i = 1;
            foreach (ParameterSets.ParameterSetsStruc para in parameterSets.parameterForm2) {
                para2 += i + "=" + para.Value + ";";
                i++;
            }

            para1 = para1.Substring(0, para1.Length - 1); //letzten ; wieder l�schen
            para2 = para2.Substring(0, para2.Length - 1); 

            set1TextBox.Text = para1; //inhalt in Textboxen eintragen
            set2TextBox.Text = para2;

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// speichern der Parameter f�r SET1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set1Button_Click(object sender, EventArgs e)
        {
            try {
                string[] set = set1TextBox.Text.Split(';');
                for (int i = 0; i < parameterSets.parameterForm1.Length; i++) {
                    parameterSets.parameterForm1[i].Value = set[i].Substring(set[i].IndexOf('=') + 1,
                        set[i].Length - 1 - set[i].IndexOf('='));
                }
                parameterSets.updateForm(parameterSets.parameterForm1, mainForm, ParameterSets.Farbe.green, true);
            } catch (Exception er) {
                MessageBox.Show("Error!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.write(mainForm, er.ToString(), 1);
            }
        }

        /// <summary>
        /// speichern der Parameter f�r SET2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set2Button_Click(object sender, EventArgs e)
        {
            try {
                string[] set = set2TextBox.Text.Split(';');
                for (int i = 0; i < parameterSets.parameterForm2.Length; i++) {
                    parameterSets.parameterForm2[i].Value = set[i].Substring(set[i].IndexOf('=') + 1,
                        set[i].Length - 1 - set[i].IndexOf('='));
                }
                parameterSets.updateForm(parameterSets.parameterForm2, mainForm, ParameterSets.Farbe.green, false);
            } catch (Exception er) {
                MessageBox.Show("Error!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.write(mainForm, er.ToString(), 1);
            }
        }
    }
}