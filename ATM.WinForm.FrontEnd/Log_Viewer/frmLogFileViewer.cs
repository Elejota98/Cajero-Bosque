using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM.WinForm.FrontEnd.Log_Viewer
{
    public partial class frmLogFileViewer : Form
    {

        private string _FilePath = string.Empty;
        private int _CountProgressBar = 0;
        public frmLogFileViewer(string sFilePath)
        {
            _FilePath = sFilePath;
            InitializeComponent();


            this.Text = "Log ->" + Path.GetFileName(sFilePath);

            TextBox.BackColor = Color.White;
            TextBox.ReadOnly = true;
            Btn_Cerrar.Enabled = false;
            TextBox.Enabled = false;

            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker1.RunWorkerAsync();
            timer1.Enabled = true;

        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timer1.Enabled = false;
            progressBar1.Value = 100;
            TextBox.ScrollToCaret();
            Btn_Cerrar.Enabled = true;
            TextBox.Enabled = true;
            using (Graphics gr = progressBar1.CreateGraphics())
            {
                gr.DrawString("Carga Finalizada...", SystemFonts.DefaultFont, Brushes.White, new PointF(progressBar1.Width / 2 - (gr.MeasureString("Carga Finalizada...", SystemFonts.DefaultFont).Width / 2.0F), progressBar1.Height / 2 - (gr.MeasureString("Carga Finalizada...", SystemFonts.DefaultFont).Height / 2.0F)));
            }

            TextBox.SelectionStart = TextBox.Text.Length;
            TextBox.Focus();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string Suffix = "";
            Suffix = CheckForNetworkShare(_FilePath);
            LoadFileLog(_FilePath, Suffix, "INICIO REGISTRO", 3);
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private static string CheckForNetworkShare(string File)
        {
            //Check to see if the file is being accessed over a network, if not return an empty string
            if (File.Substring(0, 2) == "\\\\")
            {
                string[] SplitString = Regex.Split(File, "\\\\");

                //Return the name/IP of the remote PC
                if (SplitString.Length > 2) return string.Format("on {0}", SplitString[2]);
            }
            return "";
        }

        private void Btn_Cerrar_Click(object sender, EventArgs e)
        {
            this.TextBox.Text = string.Empty;
            timer1.Enabled = false;
            this.backgroundWorker1 = null;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            using (Graphics gr = progressBar1.CreateGraphics())
            {
                gr.DrawString("Cargando Log...", SystemFonts.DefaultFont, Brushes.White, new PointF(progressBar1.Width / 2 - (gr.MeasureString("Cargando Log...", SystemFonts.DefaultFont).Width / 2.0F), progressBar1.Height / 2 - (gr.MeasureString("Cargando Log...", SystemFonts.DefaultFont).Height / 2.0F)));
            }

            _CountProgressBar++;
            backgroundWorker1.ReportProgress(_CountProgressBar);
            if (_CountProgressBar >= 100)
            {
                _CountProgressBar = 0;
            }
        }

        delegate void SetTextCallback(string sText);
        private void SetText(string sText)
        {
            if (this.TextBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { sText });
            }
            else
            {
                this.TextBox.Text = sText;
            }
        }

        delegate void InsertLineAboveCallback(string sLine);
        private void InsertLineAbove(string sLine)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.TextBox.InvokeRequired)
            {
                InsertLineAboveCallback d = new InsertLineAboveCallback(InsertLineAbove);
                this.Invoke(d, new object[] { sLine });
            }
            else
            {
                this.TextBox.Text = this.TextBox.Text.Insert(0, sLine + "\n");
            }
        }

        public void LoadFileLog(string FileName, string Suffix, string sMatchText, int iCountMatchText)
        {
            SetText("");
            BackwardReader _BackwardReader = new BackwardReader(FileName);
            int contador = 0;
            while (!_BackwardReader.SOF)
            {
                string sLine = _BackwardReader.Readline();
                if (sLine.Contains(sMatchText))
                {
                    contador++;
                    InsertLineAbove(sLine);
                    if (contador == iCountMatchText)
                    {
                        break;
                    }
                    continue;
                }
                InsertLineAbove(sLine);
            }


            _BackwardReader.Close();
            _BackwardReader = null;
        }

    }
}
