namespace AlvinHoChun.TouchPadAbsolute
{
    partial class FormMain
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
            this.chkExclusive = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // chkExclusive
            // 
            this.chkExclusive.AutoSize = true;
            this.chkExclusive.Location = new System.Drawing.Point(12, 12);
            this.chkExclusive.Name = "chkExclusive";
            this.chkExclusive.Size = new System.Drawing.Size(185, 16);
            this.chkExclusive.TabIndex = 0;
            this.chkExclusive.Text = "Gain exclusive access to TouchPad";
            this.chkExclusive.UseVisualStyleBackColor = true;
            this.chkExclusive.CheckedChanged += new System.EventHandler(this.chkExclusive_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(16, 16);
            this.panel1.TabIndex = 1;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkExclusive);
            this.DoubleBuffered = true;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkExclusive;
        private System.Windows.Forms.Panel panel1;
    }
}

