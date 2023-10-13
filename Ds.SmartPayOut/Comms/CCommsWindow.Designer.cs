namespace Ds.SmartPayOut.Comms
{
    partial class CCommsWindow
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.logWindowText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 403);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(154, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Record Encrypted Packets";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // logWindowText
            // 
            this.logWindowText.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.logWindowText.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logWindowText.Location = new System.Drawing.Point(12, 12);
            this.logWindowText.Multiline = true;
            this.logWindowText.Name = "logWindowText";
            this.logWindowText.ReadOnly = true;
            this.logWindowText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logWindowText.Size = new System.Drawing.Size(456, 385);
            this.logWindowText.TabIndex = 5;
            // 
            // CCommsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(476, 422);
            this.Controls.Add(this.logWindowText);
            this.Controls.Add(this.checkBox1);
            this.Name = "CCommsWindow";
            this.Text = "CCommsWindow";
            this.Load += new System.EventHandler(this.CCommsWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox logWindowText;
    }
}