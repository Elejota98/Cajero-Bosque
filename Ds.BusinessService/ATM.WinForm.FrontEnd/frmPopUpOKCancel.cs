using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ATM.WinForm.FrontEnd
{
    public partial class frmPopUpOKCancel : Form
    {
        private string _Texto = string.Empty;

        public string Texto
        {
            get { return _Texto; }
            set { _Texto = value; }
        }

        public frmPopUpOKCancel(string sMensaje)
        {
            Texto = sMensaje;
            InitializeComponent();
            CargaRecursos();
            CargarImagenes();
        }

        private bool CargarImagenes()
        {
            bool ok = false;

            try
            {

                ok = true;

                panelFondo.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Alertas.png"));

                btnSi.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_SI_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_SI_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_SI_Presionado.png"));
                btnNo.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_NO_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_NO_Normal.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\Btn_NO_Presionado.png"));

            }
            catch (Exception ex)
            {
                ok = false;
            }

            return ok;
        }
        private void CargaRecursos()
        {
            btnNo.Text = string.Empty;
            btnSi.Text = string.Empty;
            lblTexto.Text = Texto;

            btnNo.LockPush = false;
            btnSi.LockPush = false;

            btnNo.Parent = panelFondo;
            btnSi.Parent = panelFondo;
            lblTexto.Parent = panelFondo;
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Dispose();
            this.Close();
        }

        private void btnSi_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Dispose();
            this.Close();
        }
    }
}
