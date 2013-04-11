using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Ink;

namespace TouchPadHandwriting
{
    internal partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
            this.initializeSettings();
            this.MinimumSize = this.Size;
            this.handwritingDisplayPanel1.ShowExample();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x200 /* CP_NOCLOSE_BUTTON */;
                return cp;
            }
        }

        Settings settings;

        private void initializeSettings()
        {
            settings = Settings.LoadSettings();
            this.cbbRecognizer.Items.AddRange(RecognizersHelper.GetSuitableRecognizers());
            this.cbbRecognizer.SelectedItem = settings.InkRecognizer;

            this.numRecognitionTime.Value = (decimal)settings.RecognitionTime / 1000.0M;
            this.chkAutoInsertion.Checked = settings.AutoInsertionEnabled;

            this.btnStrokeColor.BackColor = settings.StrokeColor;
            this.handwritingDisplayPanel1.ForeColor = settings.StrokeColor;
            this.numStrokeWidth.Value = settings.StrokeWidth;
            this.handwritingDisplayPanel1.PenWidth = settings.StrokeWidth;

            if (settings.ToggleKeyUseScancode)
            {
                this.txtToggleKey.Text = string.Format("Scancode {0}", settings.ToggleKeyScancode);
            }
            else
            {
                this.txtToggleKey.Text = Resources.KeyNames.ResourceManager.GetString(settings.ToggleKey.ToString()) ?? settings.StrokeWidth.ToString();
            }
        }

        private bool validateSettings()
        {
            return true;
        }

        private void saveSettings()
        {
            settings.InkRecognizer = (Recognizer)this.cbbRecognizer.SelectedItem;

            settings.RecognitionTime = (ushort)(this.numRecognitionTime.Value * 1000.0M);
            settings.AutoInsertionEnabled = this.chkAutoInsertion.Checked;

            settings.StrokeColor = this.btnStrokeColor.BackColor;
            settings.StrokeWidth = (int)this.numStrokeWidth.Value;

            Settings.SaveSettings(settings);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.validateSettings())
            {
                this.saveSettings();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        private void btnSetToggleKey_Click(object sender, EventArgs e)
        {
            FormSetKey f = new FormSetKey();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (Enum.IsDefined(typeof(Keys), f.key) && f.key != Keys.None)
                {
                    this.settings.ToggleKeyUseScancode = false;
                    this.settings.ToggleKey = f.key;
                    this.txtToggleKey.Text = Resources.KeyNames.ResourceManager.GetString(this.settings.ToggleKey.ToString()) ?? this.settings.ToggleKey.ToString();
                }
                else if (f.scancode != 0)
                {
                    this.settings.ToggleKeyUseScancode = true;
                    this.settings.ToggleKeyScancode = f.scancode;
                    this.txtToggleKey.Text = string.Format("Scancode {0}", this.settings.ToggleKeyScancode);
                }
            }
        }

        private void btnStrokeColor_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            dlg.Color = this.btnStrokeColor.BackColor;
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.btnStrokeColor.BackColor = dlg.Color;
                this.handwritingDisplayPanel1.ForeColor = dlg.Color;
            }
        }

        private void numStrokeWidth_ValueChanged(object sender, EventArgs e)
        {
            this.handwritingDisplayPanel1.PenWidth = (int)numStrokeWidth.Value;
        }

        private void btnOpenStartup_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Startup));
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            DataObject objs = new DataObject();
            objs.SetFileDropList(new System.Collections.Specialized.StringCollection() { Link.CreateShortcut() });
            pictureBox1.DoDragDrop(objs, DragDropEffects.Copy);
        }

        ToolTip tip = new ToolTip();

        private void FormSettings_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FirstRun)
            {
                this.btnCancel.Enabled = false;
                tip.IsBalloon = true;
                tip.ToolTipIcon = ToolTipIcon.Info;
                tip.ToolTipTitle = Resources.Messages.TipRecognizerTitle;
                tip.Show("", this.cbbRecognizer, 0); //HACK: Workaround of a "known bug"... hopefully
                tip.Show(Resources.Messages.TipRecognizerText, this.cbbRecognizer, new Point(this.cbbRecognizer.Width - 8, this.cbbRecognizer.Height / 2));
            }
        }

        private void cbbRecognizer_DropDown(object sender, EventArgs e)
        {
            this.tip.Active = false;
        }
    }
}
