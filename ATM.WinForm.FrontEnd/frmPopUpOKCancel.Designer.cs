namespace ATM.WinForm.FrontEnd
{
    partial class frmPopUpOKCancel
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
            this.panelFondo = new System.Windows.Forms.Panel();
            this.btnSi = new CustomButton.CustomButton();
            this.btnNo = new CustomButton.CustomButton();
            this.lblTexto = new System.Windows.Forms.Label();
            this.panelFondo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFondo
            // 
            this.panelFondo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelFondo.Controls.Add(this.btnSi);
            this.panelFondo.Controls.Add(this.btnNo);
            this.panelFondo.Controls.Add(this.lblTexto);
            this.panelFondo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFondo.Location = new System.Drawing.Point(0, 0);
            this.panelFondo.Name = "panelFondo";
            this.panelFondo.Size = new System.Drawing.Size(1038, 506);
            this.panelFondo.TabIndex = 2;
            // 
            // btnSi
            // 
            this.btnSi.AutoSize = true;
            this.btnSi.BackColor = System.Drawing.Color.Transparent;
            this.btnSi.FlatAppearance.BorderSize = 0;
            this.btnSi.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSi.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSi.Location = new System.Drawing.Point(759, 299);
            this.btnSi.LockPush = false;
            this.btnSi.Name = "btnSi";
            this.btnSi.Size = new System.Drawing.Size(126, 126);
            this.btnSi.TabIndex = 92;
            this.btnSi.Text = "SI";
            this.btnSi.UseVisualStyleBackColor = false;
            this.btnSi.Click += new System.EventHandler(this.btnSi_Click);
            // 
            // btnNo
            // 
            this.btnNo.AutoSize = true;
            this.btnNo.BackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.BorderSize = 0;
            this.btnNo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNo.Location = new System.Drawing.Point(191, 299);
            this.btnNo.LockPush = false;
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(126, 126);
            this.btnNo.TabIndex = 91;
            this.btnNo.Text = "NO";
            this.btnNo.UseVisualStyleBackColor = false;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // lblTexto
            // 
            this.lblTexto.BackColor = System.Drawing.Color.Transparent;
            this.lblTexto.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTexto.ForeColor = System.Drawing.Color.Black;
            this.lblTexto.Location = new System.Drawing.Point(76, 102);
            this.lblTexto.Name = "lblTexto";
            this.lblTexto.Size = new System.Drawing.Size(917, 134);
            this.lblTexto.TabIndex = 90;
            this.lblTexto.Text = "Si hace un arqueo TOTAL descargara todos los billetes ¿Esta seguro que desea real" +
    "izar un arqueo total?";
            this.lblTexto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmPopUpOKCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 506);
            this.ControlBox = false;
            this.Controls.Add(this.panelFondo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPopUpOKCancel";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.panelFondo.ResumeLayout(false);
            this.panelFondo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelFondo;
        private CustomButton.CustomButton btnSi;
        private CustomButton.CustomButton btnNo;
        private System.Windows.Forms.Label lblTexto;
    }
}