using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace UAVP.UAVPSet.Debug
{
    public partial class DebugSoftware : Form
    {
        FormMain mainForm;
        PICConnect picConnect = new PICConnect();



        // Variablen für Debug
        Graphics graph1;
        Graphics graph2;
        Graphics graph3;
        int x = 0;
        bool run = true;
        int xAlt = 0;


        public DebugSoftware(FormMain mainForm, PICConnect picConnect)
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;
            this.mainForm = mainForm;
            this.picConnect = picConnect;
            


        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            picConnect.connect(mainForm, true, false);
        }







        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // für Ausgabe
            graph1 = dia1Panel.CreateGraphics();
            graph2 = dia2Panel.CreateGraphics();
            graph3 = dia3Panel.CreateGraphics();

            string temp = "";

            int[] last = { 210, 210, 210, 210, 210, 210, 210, 210, 210 };
            int[] aktuell = { 210, 210, 210, 210, 210, 210, 210, 210, 210 };

            int[] mapping = {
                                    Properties.Settings.Default.map1 - 1,
                                    Properties.Settings.Default.map2 - 1,
                                    Properties.Settings.Default.map3 - 1,
                                    Properties.Settings.Default.map4 - 1,
                                    Properties.Settings.Default.map5 - 1,
                                    Properties.Settings.Default.map6 - 1,
                                    Properties.Settings.Default.map7 - 1,
                                    Properties.Settings.Default.map8 - 1,
                                    Properties.Settings.Default.map9 - 1,
                                };

            int[] div = {
                                    Properties.Settings.Default.div1,
                                    Properties.Settings.Default.div2,
                                    Properties.Settings.Default.div3,
                                    Properties.Settings.Default.div4,
                                    Properties.Settings.Default.div5,
                                    Properties.Settings.Default.div6,
                                    Properties.Settings.Default.div7,
                                    Properties.Settings.Default.div8,
                                    Properties.Settings.Default.div9,
                                };

            int[] off = {
                                    Properties.Settings.Default.off1,
                                    Properties.Settings.Default.off2,
                                    Properties.Settings.Default.off3,
                                    Properties.Settings.Default.off4,
                                    Properties.Settings.Default.off5,
                                    Properties.Settings.Default.off6,
                                    Properties.Settings.Default.off7,
                                    Properties.Settings.Default.off8,
                                    Properties.Settings.Default.off9,
                                };

            int[] divisor = {
                                    div[mapping[0]],
                                    div[mapping[1]],
                                    div[mapping[2]],
                                    div[mapping[3]],
                                    div[mapping[4]],
                                    div[mapping[5]],
                                    div[mapping[6]],
                                    div[mapping[7]],
                                    div[mapping[8]],
                                };

            int[] offset = {
                                    off[mapping[0]],
                                    off[mapping[1]],
                                    off[mapping[2]],
                                    off[mapping[3]],
                                    off[mapping[4]],
                                    off[mapping[5]],
                                    off[mapping[6]],
                                    off[mapping[7]],
                                    off[mapping[8]],
                                };

            // StreamReader-Instanz für die Datei erzeugen
            StreamReader sr = null;

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {


                string fileName = openFileDialog1.FileName;
                try
                {
                    sr = new StreamReader(fileName, Encoding.GetEncoding("windows-1252"));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Öffnen der Datei '" + fileName + "': " +
                        ex.Message, Application.ProductName, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }
                // Datei zeilenweise einlesen
                while ((temp = sr.ReadLine()) != null && run == true)
                {
                    string[] teile = temp.Split(';');
                    try
                    {

                        for (int i = 0; i < teile.Length; i++)
                        {
                            if (teile[i] == "")
                            {
                                teile[i] = "0";
                            }
                        }

                        aktuell.CopyTo(last, 0);


                        aktuell[0] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[0]], 16) / divisor[0] - offset[0]);
                        aktuell[1] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[1]], 16) / divisor[1] - offset[1]);
                        aktuell[2] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[2]], 16) / divisor[2] - offset[2]);
                        aktuell[3] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[3]], 16) / divisor[3] - offset[3]);
                        aktuell[4] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[4]], 16) / divisor[4] - offset[4]);
                        aktuell[5] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[5]], 16) / divisor[5] - offset[5]);
                        aktuell[6] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[6]], 16) / divisor[6] - offset[6]);
                        aktuell[7] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[7]], 16) / divisor[7] - offset[7]);
                        aktuell[8] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[8]], 16) / divisor[8] - offset[8]);


                        for (int i = 0; i < 9; i++)
                        {
                            if (aktuell[i] == 210 && last[i] != 210)
                            {
                                aktuell[i] = last[i];
                            }
                        }


                        if (x < 900)
                        {
                            graph1.FillRectangle(Brushes.WhiteSmoke, x + 1, 1, 1, 219);
                            graph2.FillRectangle(Brushes.WhiteSmoke, x + 1, 1, 1, 219);
                            graph3.FillRectangle(Brushes.WhiteSmoke, x + 1, 1, 1, 219);
                        }
                        
                        graph1.FillRectangle(Brushes.Black, x, 0, 1, 220);
                        graph1.DrawLine(Pens.Yellow, xAlt, last[0], x, aktuell[0]);
                        graph1.DrawLine(Pens.Green, xAlt, last[1], x, aktuell[1]);
                        graph1.DrawLine(Pens.HotPink, xAlt, last[2], x, aktuell[2]);

                        graph2.FillRectangle(Brushes.Black, x, 0, 1, 220);
                        graph2.DrawLine(Pens.Yellow, xAlt, last[3], x, aktuell[3]);
                        graph2.DrawLine(Pens.Green, xAlt, last[4], x, aktuell[4]);
                        graph2.DrawLine(Pens.HotPink, xAlt, last[5], x, aktuell[5]);

                        graph3.FillRectangle(Brushes.Black, x, 0, 1, 220);
                        graph3.DrawLine(Pens.Yellow, xAlt, last[6], x, aktuell[6]);
                        graph3.DrawLine(Pens.Green, xAlt, last[7], x, aktuell[7]);
                        graph3.DrawLine(Pens.HotPink, xAlt, last[8], x, aktuell[8]);



                        xAlt = x++;

                        if (x == 900)
                        {
                            x = xAlt = 0;
                        }
                        Application.DoEvents();
                    }
                    catch (Exception ex)
                    { }
                    System.Threading.Thread.Sleep(10);
                }

                run = true;
                // StreamReader schließen
                sr.Close();
            }
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapping map = new mapping();
            map.ShowDialog();
        }


        public void DebugStop()
        {
            run = false;
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            picConnect.connect(mainForm, true, false);

            // für Ausgabe
            graph1 = dia1Panel.CreateGraphics();
            graph2 = dia2Panel.CreateGraphics();
            graph3 = dia3Panel.CreateGraphics();

            string temp = "";

            int[] last = { 210, 210, 210, 210, 210, 210, 210, 210, 210 };
            int[] aktuell = { 210, 210, 210, 210, 210, 210, 210, 210, 210 };

            int[] mapping = {
                                    Properties.Settings.Default.map1 - 1,
                                    Properties.Settings.Default.map2 - 1,
                                    Properties.Settings.Default.map3 - 1,
                                    Properties.Settings.Default.map4 - 1,
                                    Properties.Settings.Default.map5 - 1,
                                    Properties.Settings.Default.map6 - 1,
                                    Properties.Settings.Default.map7 - 1,
                                    Properties.Settings.Default.map8 - 1,
                                    Properties.Settings.Default.map9 - 1,
                                };

            int[] div = {
                                    Properties.Settings.Default.div1,
                                    Properties.Settings.Default.div2,
                                    Properties.Settings.Default.div3,
                                    Properties.Settings.Default.div4,
                                    Properties.Settings.Default.div5,
                                    Properties.Settings.Default.div6,
                                    Properties.Settings.Default.div7,
                                    Properties.Settings.Default.div8,
                                    Properties.Settings.Default.div9,
                                };

            int[] off = {
                                    Properties.Settings.Default.off1,
                                    Properties.Settings.Default.off2,
                                    Properties.Settings.Default.off3,
                                    Properties.Settings.Default.off4,
                                    Properties.Settings.Default.off5,
                                    Properties.Settings.Default.off6,
                                    Properties.Settings.Default.off7,
                                    Properties.Settings.Default.off8,
                                    Properties.Settings.Default.off9,
                                };

            int[] divisor = {
                                    div[mapping[0]],
                                    div[mapping[1]],
                                    div[mapping[2]],
                                    div[mapping[3]],
                                    div[mapping[4]],
                                    div[mapping[5]],
                                    div[mapping[6]],
                                    div[mapping[7]],
                                    div[mapping[8]],
                                };

            int[] offset = {
                                    off[mapping[0]],
                                    off[mapping[1]],
                                    off[mapping[2]],
                                    off[mapping[3]],
                                    off[mapping[4]],
                                    off[mapping[5]],
                                    off[mapping[6]],
                                    off[mapping[7]],
                                    off[mapping[8]],
                                };


            // DebugWerte lesen 
            while (run == true)
            {
                try
                {
                    temp = picConnect.sp.ReadLine();
                }
                catch {
                    Application.DoEvents();
                    continue;
                }

                string[] teile = temp.Split(';');
                if (teile.Length < 9)
                    continue;

                try
                {

                    for (int i = 0; i < teile.Length; i++)
                    {
                        if (teile[i] == "")
                        {
                            teile[i] = "0";
                        }
                    }
                    teile[teile.Length-1] = teile[teile.Length-1].Replace("\r", "");

                    aktuell.CopyTo(last, 0);

                    try
                    {
                        aktuell[0] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[0]], 16) / divisor[0] - offset[0]);
                    }
                    catch
                    {
                        aktuell[0] = 210;
                    }
                    try
                    {
                        aktuell[1] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[1]], 16) / divisor[1] - offset[1]);
                    }
                    catch {
                        aktuell[1] = 210;

                    }
                    try
                    {
                        aktuell[2] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[2]], 16) / divisor[2] - offset[2]);
                    }
                    catch
                    {
                        aktuell[2] = 210;
                    }
                    try
                    {
                        aktuell[3] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[3]], 16) / divisor[3] - offset[3]);
                    }
                    catch
                    {
                        aktuell[3] = 210;
                    }
                    try
                    {
                        aktuell[4] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[4]], 16) / divisor[4] - offset[4]);
                    }
                    catch
                    {
                        aktuell[4] = 210;
                    }
                    try
                    {
                        aktuell[5] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[5]], 16) / divisor[5] - offset[5]);
                    }
                    catch {
                        aktuell[5] = 210;
                    }
                    try
                    {
                        aktuell[6] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[6]], 16) / divisor[6] - offset[6]);
                    }
                    catch
                    {
                        aktuell[6] = 210;
                    }
                    try
                    {
                        aktuell[7] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[7]], 16) / divisor[7] - offset[7]);
                    }
                    catch {
                        aktuell[7] = 210;
                    }
                    try
                    {
                        aktuell[8] = Convert.ToInt32(210 - Convert.ToInt32(teile[mapping[8]], 16) / divisor[8] - offset[8]);
                    }
                    catch
                    {
                        aktuell[8] = 210;
                    }
                    
                    
                    for (int i = 0; i < 9; i++)
                    {
                        if (aktuell[i] == 210 && last[i] != 210)
                        {
                            aktuell[i] = last[i];
                        }
                    }


                    if (x < 900)
                    {
                        graph1.FillRectangle(Brushes.WhiteSmoke, x + 1, 1, 1, 219);
                        graph2.FillRectangle(Brushes.WhiteSmoke, x + 1, 1, 1, 219);
                        graph3.FillRectangle(Brushes.WhiteSmoke, x + 1, 1, 1, 219);
                    }

                    graph1.FillRectangle(Brushes.Black, x, 0, 1, 220);
                    graph1.DrawLine(Pens.Yellow, xAlt, last[0], x, aktuell[0]);
                    graph1.DrawLine(Pens.Green, xAlt, last[1], x, aktuell[1]);
                    graph1.DrawLine(Pens.HotPink, xAlt, last[2], x, aktuell[2]);

                    graph2.FillRectangle(Brushes.Black, x, 0, 1, 220);
                    graph2.DrawLine(Pens.Yellow, xAlt, last[3], x, aktuell[3]);
                    graph2.DrawLine(Pens.Green, xAlt, last[4], x, aktuell[4]);
                    graph2.DrawLine(Pens.HotPink, xAlt, last[5], x, aktuell[5]);

                    graph3.FillRectangle(Brushes.Black, x, 0, 1, 220);
                    graph3.DrawLine(Pens.Yellow, xAlt, last[6], x, aktuell[6]);
                    graph3.DrawLine(Pens.Green, xAlt, last[7], x, aktuell[7]);
                    graph3.DrawLine(Pens.HotPink, xAlt, last[8], x, aktuell[8]);



                    xAlt = x++;

                    if (x == 900)
                    {
                        x = xAlt = 0;
                    }
                    Application.DoEvents();
                }
                catch (Exception ex)
                { }
            }

            run = true;
            // StreamReader schließen
            picConnect.sp.Close();
            
        }

        private void sTOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            run = false;
        }
    }
}