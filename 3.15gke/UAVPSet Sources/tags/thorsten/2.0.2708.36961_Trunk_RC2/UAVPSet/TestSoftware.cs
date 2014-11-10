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
    public partial class TestSoftware : Form
    {
        FormMain mainForm;
        PICConnect picConnect = new PICConnect();

        public TestSoftware(FormMain mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            readFunctions();
        }

        /// <summary>
        /// einlesen aller möglichen Befehle mittels ?
        /// </summary>
        void readFunctions()
        {
            picConnect.connect(mainForm, true);
            ArrayList functions = picConnect.askPic(mainForm, "?");
            functions.RemoveAt(0); // ersten beiden Zeilen löschen - sind nur Info Zeilen
            functions.RemoveAt(0);
            functions.RemoveAt(functions.Count - 1); //letzen beiden Zeilen löschen (nur info und >)
            functions.RemoveAt(functions.Count - 1);
            // alle Befehle durchgehen und in ListView eintragen
            foreach (string function in functions) {
                ListViewItem listViewItem = new ListViewItem(function);
                // wenn Befehl mit 1-8 Beginnt dann daraus 8 Einträge erstellen
                if (listViewItem.Text.Substring(0, 3) == "1-8") {
                    string temp = listViewItem.Text;
                    for (int i = 1; i < 9; i++) {
                        listViewItem = new ListViewItem(function);
                        listViewItem.Text = i.ToString() + "." + temp.Substring(3, temp.Length - 4);
                        functionsListView.Items.Add(listViewItem);
                    }
                } else if (listViewItem.Text.Substring(0, 1) != "B") { //B = Bootloader - nicht anzeigen!
                    listViewItem.Text = listViewItem.Text.Substring(0, listViewItem.Text.Length - 1);
                    functionsListView.Items.Add(listViewItem);
                }
            }
        }

        private void TestSoftware_FormClosing(object sender, FormClosingEventArgs e)
        {
            picConnect.sp.Close();
        }

        /// <summary>
        /// reagieren wenn ein Befehl aus der ListView ausgewählt wurde
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void functionsListView_Click(object sender, EventArgs e)
        {
            if (functionsListView.SelectedItems.Count == 0) return;

            Cursor = Cursors.WaitCursor;
            //antworten vom Pic speichern
            ArrayList infos = picConnect.askPic(mainForm, functionsListView.SelectedItems[0].Text.Substring(0, 1));

            // Letzte Zeile mit # löschen
            infos.RemoveAt(infos.Count - 1);

            //ergebnisse ausgeben
            foreach (string info in infos) {
                outputTextBox.Text += info + "\n";
                // auf das ende der Textbox scrollen
                outputTextBox.SelectionStart = outputTextBox.Text.Length;
                outputTextBox.ScrollToCaret();
            }
            Cursor = Cursors.Default;
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}