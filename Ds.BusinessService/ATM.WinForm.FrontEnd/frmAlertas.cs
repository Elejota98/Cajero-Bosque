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
    public partial class frmAlertas : Form
    {
        private string _MensajeAlerta = string.Empty;

        public frmAlertas(string MensajeAlerta)
        {
            _MensajeAlerta = MensajeAlerta;
            InitializeComponent();
        }
        private void frmAlertas_Load(object sender, EventArgs e)
        {
            CargaRecursos();
            CargaImagenes();
            lblMensaje.Text = _MensajeAlerta.ToString();
        }

        public bool CargaRecursos()
        {
            bool ok = false;

            try
            {
                Imagen_Alertas.Dock = DockStyle.Fill;

                //Imagen_Warning.Text = string.Empty;
                //Imagen_Warning.LockPush = false;

                btn_AceptarAlerta.LockPush = false;
                btn_AceptarAlerta.Text = string.Empty;

                //label1.BackColor = Color.Transparent;
                lblMensaje.BackColor = Color.Transparent;

                ok = true;
            }
            catch (Exception ex)
            {

            }

            return ok;
        }
        public bool CargaImagenes()
        {
            bool ok = false;

            try
            {
                Imagen_Alertas.BackgroundImage = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Alertas.png"));
                //Imagen_Warning.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Warning.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Warning.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Jpg\Imagen_Warning.png"));
                btn_AceptarAlerta.ImageSettings(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Acepto.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Acepto.png"), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Medios\Btn\btn_Acepto.png"));

                ok = true;
            }
            catch (Exception ex)
            {

            }

            return ok;
        }

        private void btn_AceptarAlerta_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
