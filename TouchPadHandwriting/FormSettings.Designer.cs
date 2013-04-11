namespace TouchPadHandwriting
{
    partial class FormSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.tabContainer = new System.Windows.Forms.TabControl();
            this.tabPageRecognizer = new System.Windows.Forms.TabPage();
            this.chkAutoInsertion = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numRecognitionTime = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbRecognizer = new System.Windows.Forms.ComboBox();
            this.tabPageHandwriting = new System.Windows.Forms.TabPage();
            this.numStrokeWidth = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.btnStrokeColor = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPageHotkey = new System.Windows.Forms.TabPage();
            this.btnSetHotkey = new System.Windows.Forms.Button();
            this.txtToggleHotkey = new System.Windows.Forms.TextBox();
            this.chkHotkeyCombination = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnSetToggleKey = new System.Windows.Forms.Button();
            this.txtToggleKey = new System.Windows.Forms.TextBox();
            this.chkHotkeyDblPress = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tabPageShortcut = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnOpenStartup = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.handwritingDisplayPanel1 = new TouchPadHandwriting.HandwritingDisplayPanel();
            this.tabContainer.SuspendLayout();
            this.tabPageRecognizer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecognitionTime)).BeginInit();
            this.tabPageHandwriting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStrokeWidth)).BeginInit();
            this.tabPageHotkey.SuspendLayout();
            this.tabPageShortcut.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabContainer
            // 
            resources.ApplyResources(this.tabContainer, "tabContainer");
            this.tabContainer.Controls.Add(this.tabPageRecognizer);
            this.tabContainer.Controls.Add(this.tabPageHandwriting);
            this.tabContainer.Controls.Add(this.tabPageHotkey);
            this.tabContainer.Controls.Add(this.tabPageShortcut);
            this.tabContainer.Name = "tabContainer";
            this.tabContainer.SelectedIndex = 0;
            // 
            // tabPageRecognizer
            // 
            resources.ApplyResources(this.tabPageRecognizer, "tabPageRecognizer");
            this.tabPageRecognizer.Controls.Add(this.chkAutoInsertion);
            this.tabPageRecognizer.Controls.Add(this.comboBox1);
            this.tabPageRecognizer.Controls.Add(this.label4);
            this.tabPageRecognizer.Controls.Add(this.numRecognitionTime);
            this.tabPageRecognizer.Controls.Add(this.label3);
            this.tabPageRecognizer.Controls.Add(this.label1);
            this.tabPageRecognizer.Controls.Add(this.label2);
            this.tabPageRecognizer.Controls.Add(this.cbbRecognizer);
            this.tabPageRecognizer.Name = "tabPageRecognizer";
            this.tabPageRecognizer.UseVisualStyleBackColor = true;
            // 
            // chkAutoInsertion
            // 
            resources.ApplyResources(this.chkAutoInsertion, "chkAutoInsertion");
            this.chkAutoInsertion.Name = "chkAutoInsertion";
            this.chkAutoInsertion.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            resources.GetString("comboBox1.Items")});
            this.comboBox1.Name = "comboBox1";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // numRecognitionTime
            // 
            resources.ApplyResources(this.numRecognitionTime, "numRecognitionTime");
            this.numRecognitionTime.DecimalPlaces = 1;
            this.numRecognitionTime.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numRecognitionTime.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numRecognitionTime.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numRecognitionTime.Name = "numRecognitionTime";
            this.numRecognitionTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cbbRecognizer
            // 
            resources.ApplyResources(this.cbbRecognizer, "cbbRecognizer");
            this.cbbRecognizer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbRecognizer.FormattingEnabled = true;
            this.cbbRecognizer.Name = "cbbRecognizer";
            this.cbbRecognizer.Sorted = true;
            this.cbbRecognizer.DropDown += new System.EventHandler(this.cbbRecognizer_DropDown);
            // 
            // tabPageHandwriting
            // 
            resources.ApplyResources(this.tabPageHandwriting, "tabPageHandwriting");
            this.tabPageHandwriting.Controls.Add(this.handwritingDisplayPanel1);
            this.tabPageHandwriting.Controls.Add(this.numStrokeWidth);
            this.tabPageHandwriting.Controls.Add(this.label8);
            this.tabPageHandwriting.Controls.Add(this.btnStrokeColor);
            this.tabPageHandwriting.Controls.Add(this.label7);
            this.tabPageHandwriting.Name = "tabPageHandwriting";
            this.tabPageHandwriting.UseVisualStyleBackColor = true;
            // 
            // numStrokeWidth
            // 
            resources.ApplyResources(this.numStrokeWidth, "numStrokeWidth");
            this.numStrokeWidth.Maximum = new decimal(new int[] {
            48,
            0,
            0,
            0});
            this.numStrokeWidth.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numStrokeWidth.Name = "numStrokeWidth";
            this.numStrokeWidth.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.numStrokeWidth.ValueChanged += new System.EventHandler(this.numStrokeWidth_ValueChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // btnStrokeColor
            // 
            resources.ApplyResources(this.btnStrokeColor, "btnStrokeColor");
            this.btnStrokeColor.BackColor = System.Drawing.Color.Red;
            this.btnStrokeColor.Name = "btnStrokeColor";
            this.btnStrokeColor.UseVisualStyleBackColor = false;
            this.btnStrokeColor.Click += new System.EventHandler(this.btnStrokeColor_Click);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // tabPageHotkey
            // 
            resources.ApplyResources(this.tabPageHotkey, "tabPageHotkey");
            this.tabPageHotkey.Controls.Add(this.btnSetHotkey);
            this.tabPageHotkey.Controls.Add(this.txtToggleHotkey);
            this.tabPageHotkey.Controls.Add(this.chkHotkeyCombination);
            this.tabPageHotkey.Controls.Add(this.label6);
            this.tabPageHotkey.Controls.Add(this.btnSetToggleKey);
            this.tabPageHotkey.Controls.Add(this.txtToggleKey);
            this.tabPageHotkey.Controls.Add(this.chkHotkeyDblPress);
            this.tabPageHotkey.Controls.Add(this.label5);
            this.tabPageHotkey.Name = "tabPageHotkey";
            this.tabPageHotkey.UseVisualStyleBackColor = true;
            // 
            // btnSetHotkey
            // 
            resources.ApplyResources(this.btnSetHotkey, "btnSetHotkey");
            this.btnSetHotkey.Name = "btnSetHotkey";
            this.btnSetHotkey.UseVisualStyleBackColor = true;
            // 
            // txtToggleHotkey
            // 
            resources.ApplyResources(this.txtToggleHotkey, "txtToggleHotkey");
            this.txtToggleHotkey.Name = "txtToggleHotkey";
            this.txtToggleHotkey.ReadOnly = true;
            // 
            // chkHotkeyCombination
            // 
            resources.ApplyResources(this.chkHotkeyCombination, "chkHotkeyCombination");
            this.chkHotkeyCombination.AutoCheck = false;
            this.chkHotkeyCombination.Checked = true;
            this.chkHotkeyCombination.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHotkeyCombination.Name = "chkHotkeyCombination";
            this.chkHotkeyCombination.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // btnSetToggleKey
            // 
            resources.ApplyResources(this.btnSetToggleKey, "btnSetToggleKey");
            this.btnSetToggleKey.Name = "btnSetToggleKey";
            this.btnSetToggleKey.UseVisualStyleBackColor = true;
            this.btnSetToggleKey.Click += new System.EventHandler(this.btnSetToggleKey_Click);
            // 
            // txtToggleKey
            // 
            resources.ApplyResources(this.txtToggleKey, "txtToggleKey");
            this.txtToggleKey.Name = "txtToggleKey";
            this.txtToggleKey.ReadOnly = true;
            // 
            // chkHotkeyDblPress
            // 
            resources.ApplyResources(this.chkHotkeyDblPress, "chkHotkeyDblPress");
            this.chkHotkeyDblPress.AutoCheck = false;
            this.chkHotkeyDblPress.Checked = true;
            this.chkHotkeyDblPress.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHotkeyDblPress.Name = "chkHotkeyDblPress";
            this.chkHotkeyDblPress.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tabPageShortcut
            // 
            resources.ApplyResources(this.tabPageShortcut, "tabPageShortcut");
            this.tabPageShortcut.Controls.Add(this.label10);
            this.tabPageShortcut.Controls.Add(this.pictureBox1);
            this.tabPageShortcut.Controls.Add(this.btnOpenStartup);
            this.tabPageShortcut.Controls.Add(this.label9);
            this.tabPageShortcut.Name = "tabPageShortcut";
            this.tabPageShortcut.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // btnOpenStartup
            // 
            resources.ApplyResources(this.btnOpenStartup, "btnOpenStartup");
            this.btnOpenStartup.Name = "btnOpenStartup";
            this.btnOpenStartup.UseVisualStyleBackColor = true;
            this.btnOpenStartup.Click += new System.EventHandler(this.btnOpenStartup_Click);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // handwritingDisplayPanel1
            // 
            resources.ApplyResources(this.handwritingDisplayPanel1, "handwritingDisplayPanel1");
            this.handwritingDisplayPanel1.BackColor = System.Drawing.Color.White;
            this.handwritingDisplayPanel1.ForeColor = System.Drawing.Color.Red;
            this.handwritingDisplayPanel1.Name = "handwritingDisplayPanel1";
            this.handwritingDisplayPanel1.PenWidth = 20;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.btnSave;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabContainer);
            this.MaximizeBox = false;
            this.Name = "FormSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Shown += new System.EventHandler(this.FormSettings_Shown);
            this.tabContainer.ResumeLayout(false);
            this.tabPageRecognizer.ResumeLayout(false);
            this.tabPageRecognizer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRecognitionTime)).EndInit();
            this.tabPageHandwriting.ResumeLayout(false);
            this.tabPageHandwriting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numStrokeWidth)).EndInit();
            this.tabPageHotkey.ResumeLayout(false);
            this.tabPageHotkey.PerformLayout();
            this.tabPageShortcut.ResumeLayout(false);
            this.tabPageShortcut.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabContainer;
        private System.Windows.Forms.TabPage tabPageRecognizer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPageHotkey;
        private System.Windows.Forms.ComboBox cbbRecognizer;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numRecognitionTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkHotkeyDblPress;
        private System.Windows.Forms.Button btnSetToggleKey;
        private System.Windows.Forms.TextBox txtToggleKey;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPageHandwriting;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnStrokeColor;
        private System.Windows.Forms.NumericUpDown numStrokeWidth;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkAutoInsertion;
        private HandwritingDisplayPanel handwritingDisplayPanel1;
        private System.Windows.Forms.TabPage tabPageShortcut;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnOpenStartup;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkHotkeyCombination;
        private System.Windows.Forms.TextBox txtToggleHotkey;
        private System.Windows.Forms.Button btnSetHotkey;
    }
}