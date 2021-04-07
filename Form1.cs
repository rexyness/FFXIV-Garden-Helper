using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;
using AutoIt;

namespace GardenGUI
{
    
    public partial class Form1 : Form
    {
        public Mem m = new Mem();
        public bool ProcOpen = false;
        public Form1()
        {
            InitializeComponent();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Process[] processCollection = Process.GetProcessesByName("ffxiv_dx11");
            
            // constantly check if the process is available and open
            ProcOpen = m.OpenProcess("ffxiv_dx11");
            Thread.Sleep(100);
            backgroundWorker1.ReportProgress(0); // do UI thread stuff
        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (ProcOpen)
            {
                ProcOpenLabel.ForeColor = Color.Green;
                playerNameLabel.ForeColor = Color.Green;
                ProcOpenLabel.Text = "Game Found";
                playerNameLabel.Text = m.ReadString("base+1DB5789");
                gardenCBox.Enabled = true;
            }
            else
            {
                ProcOpenLabel.ForeColor = Color.Red;
                ProcOpenLabel.Text = "N/A";
                gardenCBox.Enabled = false;
                return; // do not continue if process is not available/open
            }

            if (gardenCBox.Checked && gardenCBox.Enabled)
            {
                string bubbleText = m.ReadString("base+01D69E68,C8,1F0,0");
                // gardenCBox.Text = bubbleText;
                byte[] arr1 = { 0 }; 
                if (System.Convert.ToBoolean(m.ReadByte("base+1D8DE60")))
                {
                    if(bubbleText.Contains("is ready"))
                    {
                        AutoItX.Sleep(200);
                        AutoItX.Send("{numpad0}");
                        AutoItX.Sleep(100);
                        AutoItX.Send("{numpad0}");
                        AutoItX.Sleep(300);
                    }

                    if (bubbleText.Contains("is nothing"))
                    {
                        AutoItX.Sleep(200);
                        short[] seq = { 0,0,0,0,6,0,0,2,4,0,4,0 };
                        foreach (var item in seq)
                        {
                            AutoItX.Send("{numpad"+item+"}");
                            AutoItX.Sleep(100);
                        }
                    }

                    
                    if(bubbleText.Contains("is doing"))
                    {
                        AutoItX.Sleep(200);
                        AutoItX.Send("{numpad0}");
                        AutoItX.Sleep(100);
                        AutoItX.Send("{numpad2}");
                        AutoItX.Sleep(100);
                        AutoItX.Send("{numpad0}");
                        AutoItX.Sleep(200);
                    }
                }
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
    }
}
