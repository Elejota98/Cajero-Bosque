namespace ATM.WinForm.FrontEnd
{
    partial class frmAlertas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Imagen_Alertas = new System.Windows.Forms.Panel();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.btn_AceptarAlerta = new CustomButton.CustomButton();
            this.Imagen_Alertas.SuspendLayout();
            this.SuspendLayout();
            // 
            // Imagen_Alertas
            // 
            this.Imagen_Alertas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Imagen_Alertas.Controls.Add(this.lblMensaje);
            this.Imagen_Alertas.Controls.Add(this.btn_AceptarAlerta);
            this.Imagen_Alertas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Imagen_Alertas.Location = new System.Drawing.Point(0, 0);
            this.Imagen_Alertas.Name = "Imagen_Alertas";
            this.Imagen_Alertas.Size = new System.Drawing.Size(946, 434);
            this.Imagen_Alertas.TabIndex = 1;
            // 
            // lblMensaje
            // 
            this.lblMensaje.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.Color.Black;
            this.lblMensaje.Location = new System.Drawing.Point(152, 87);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMensaje.Size = new System.Drawing.Size(639, 155);
            this.lblMensaje.TabIndex = 24;
            this.lblMensaje.Text = "FF";
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_AceptarAlerta
            // 
            this.btn_AceptarAlerta.AutoSize = true;
            this.btn_AceptarAlerta.BackColor = System.Drawing.Color.Transparent;
            this.btn_AceptarAlerta.FlatAppearance.BorderSize = 0;
            this.btn_AceptarAlerta.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn_AceptarAlerta.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn_AceptarAlerta.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_AceptarAlerta.Location = new System.Drawing.Point(288, 256);
            this.btn_AceptarAlerta.LockPush = true;
            this.btn_AceptarAlerta.Name = "btn_AceptarAlerta";
            this.btn_AceptarAlerta.Size = new System.Drawing.Size(154, 76);
            this.btn_AceptarAlerta.TabIndex = 21;
            this.btn_AceptarAlerta.Text = "Aceptar";
            this.btn_AceptarAlerta.UseVisualStyleBackColor = false;
            this.btn_AceptarAlerta.Click += new System.EventHandler(this.btn_AceptarAlerta_Click);
            // 
            // frmAlertas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(946, 434);
            this.ControlBox = false;
            this.Controls.Add(this.Imagen_Alertas);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAlertas";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAlertas_Load);
            this.Imagen_Alertas.ResumeLayout(false);
            this.Imagen_Alertas.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Imagen_Alertas;
        private System.Windows.Forms.Label lblMensaje;
        private CustomButton.CustomButton btn_AceptarAlerta;
    }
}