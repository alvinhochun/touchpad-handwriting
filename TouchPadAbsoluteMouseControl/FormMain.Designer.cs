namespace TouchPadAbsoluteMouseControl
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
            this.btnSelectRegion = new System.Windows.Forms.Button();
            this.txtTop = new System.Windows.Forms.TextBox();
            this.txtLeft = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSelectRegion
            // 
            this.btnSelectRegion.Location = new System.Drawing.Point(112, 49);
            this.btnSelectRegion.Name = "btnSelectRegion";
            this.btnSelectRegion.Size = new System.Drawing.Size(100, 23);
            this.btnSelectRegion.TabIndex = 0;
            this.btnSelectRegion.Text = "Select Region";
            this.btnSelectRegion.UseVisualStyleBackColor = true;
            this.btnSelectRegion.Click += new System.EventHandler(this.btnSelectRegion_Click);
            // 
            // txtTop
            // 
            this.txtTop.Location = new System.Drawing.Point(112, 21);
            this.txtTop.Name = "txtTop";
            this.txtTop.ReadOnly = true;
            this.txtTop.Size = new System.Drawing.Size(100, 22);
            this.txtTop.TabIndex = 1;
            // 
            // txtLeft
            // 
            this.txtLeft.Location = new System.Drawing.Point(6, 50);
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.ReadOnly = true;
            this.txtLeft.Size = new System.Drawing.Size(100, 22);
            this.txtLeft.TabIndex = 2;
            // 
            // txtBottom
            // 
            this.txtHeight.Location = new System.Drawing.Point(112, 78);
            this.txtHeight.Name = "txtBottom";
            this.txtHeight.ReadOnly = true;
            this.txtHeight.Size = new System.Drawing.Size(100, 22);
            this.txtHeight.TabIndex = 3;
            // 
            // txtRight
            // 
            this.txtWidth.Location = new System.Drawing.Point(218, 50);
            this.txtWidth.Name = "txtRight";
            this.txtWidth.ReadOnly = true;
            this.txtWidth.Size = new System.Drawing.Size(100, 22);
            this.txtWidth.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelectRegion);
            this.groupBox1.Controls.Add(this.txtWidth);
            this.groupBox1.Controls.Add(this.txtTop);
            this.groupBox1.Controls.Add(this.txtHeight);
            this.groupBox1.Controls.Add(this.txtLeft);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 106);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Screen Region to Control";
            // 
            // chkEnabled
            // 
            this.chkEnabled.AutoSize = true;
            this.chkEnabled.Location = new System.Drawing.Point(12, 124);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(62, 16);
            this.chkEnabled.TabIndex = 6;
            this.chkEnabled.Text = "Enabled";
            this.chkEnabled.UseVisualStyleBackColor = true;
            this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 152);
            this.Controls.Add(this.chkEnabled);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormMain";
            this.Text = "TouchPad Absolute Mouse Control";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectRegion;
        private System.Windows.Forms.TextBox txtTop;
        private System.Windows.Forms.TextBox txtLeft;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkEnabled;
    }
}

