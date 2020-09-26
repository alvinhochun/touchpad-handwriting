using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Microsoft.Ink;

using AlvinHoChun.SynapticsTouchPad;
using alvinhc.TouchPadInterface;

namespace TouchPadHandwriting
{
    internal partial class FormMain : Form
    {
        Settings settings;
        public FormMain()
        {
            InitializeComponent();
            this.settings = Settings.LoadSettings();

            this.recognizerContext = this.settings.InkRecognizer.CreateRecognizerContext();
            this.handwritingDisplayPanel1.AspectRatio = 1.5;
            Rectangle writingBox = new Rectangle(new Point(0, 0), this.handwritingDisplayPanel1.HandWritingBox);
            this.recognizerContext.Guide = new RecognizerGuide(1, 1, 0, writingBox, writingBox);

            this.timer1.Interval = this.settings.RecognitionTime;

            this.handwritingDisplayPanel1.ForeColor = this.settings.StrokeColor;
            this.handwritingDisplayPanel1.PenWidth = this.settings.StrokeWidth;

            this.touchPad.ShouldRaiseEvents = false;

            this.touchPad.FingerDown += new FingerEventHandler(touchPad_FingerDown);
            this.touchPad.FingerMove += new FingerEventHandler(touchPad_FingerMove);
            this.touchPad.FingerUp += new FingerEventHandler(touchPad_FingerUp);

            this.dblPressDetector = new KeyboardDoublePressDetector(this.keyboardHook);
            this.dblPressDetector.KeyDoublePressed += new KeyboardDoublePressDetector.KeyboardDoublePressEventHandler(dblPressDetector_KeyDoublePressed);
            if (this.settings.ToggleKeyUseScancode)
            {
                this.dblPressDetector.UseScanCode(this.settings.ToggleKeyScancode);
            }
            else
            {
                this.dblPressDetector.UseKey(this.settings.ToggleKey);
            }
            this.dblPressDetector.Enabled = true;
            this.keyboardHook.GlobalKeyDown += new KeyboardHook.KeyEventHandlerExt(keyboardHook_GlobalKeyDown);
            this.keyboardHook.Enabled = true;

            this.resultButtons = new Button[10];
            for (int i = 0; i < this.resultButtons.Length; i++)
            {
                Button btn = new Button();
                btn.FlatStyle = FlatStyle.Popup;
                btn.Margin = new Padding(0, 0, 0, 0);
                btn.Text = "";
                this.tableLayoutPanel1.Controls.Add(btn, i, 0);
                btn.Dock = DockStyle.Fill;
                btn.Click += new EventHandler(resultButtons_Click);
                this.resultButtons[i] = btn;

                Label lbl = new Label();
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Text = ((i + 1) % 10).ToString();
                this.tableLayoutPanel1.Controls.Add(lbl, i, 1);
                lbl.Dock = DockStyle.Fill;
            }

            this.setFormSize();
            this.setFormLocation();

            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);
            Microsoft.Win32.SystemEvents.PowerModeChanged += new Microsoft.Win32.PowerModeChangedEventHandler(SystemEvents_PowerModeChanged);
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);

            RegisterHotKey(this.Handle, HOTKEY_ID, 0x0002 /* MOD_CONTROL */ | 0x0008 /* MOD_WIN */, (uint)Keys.N);

            if (Properties.Settings.Default.FirstRun)
            {
                this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                this.notifyIcon1.BalloonTipTitle = Resources.Messages.TipReadyTitle;

                string keyDisplayText;
                if (this.settings.ToggleKeyUseScancode)
                {
                    keyDisplayText = Resources.Messages.TipReadyTextSelectedKey;
                }
                else
                {
                    keyDisplayText = Resources.KeyNames.ResourceManager.GetString(this.settings.ToggleKey.ToString()) ?? this.settings.StrokeWidth.ToString();
                }
                this.notifyIcon1.BalloonTipText = string.Format(Resources.Messages.TipReadyText, keyDisplayText, "Ctrl + Win + N");
                this.notifyIcon1.ShowBalloonTip(30000);
            }
        }

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock)
            {
                this.Hide();
            }
        }

        void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PowerModeChanged {0}", e.Mode);
            if (e.Mode == Microsoft.Win32.PowerModes.Suspend)
            {
                this.touchPad.Enabled = false;
            }
            else if (e.Mode == Microsoft.Win32.PowerModes.Resume)
            {
                Program.restartFlag = true;
                this.Close();
                this.Dispose();
                return;
            }
        }

        const int HOTKEY_ID = 0x0001;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 /* WM_HOTKEY */)
            {
                if (this.Visible)
                {
                    this.Hide();
                }
                else
                {
                    this.allowShow = true;
                    this.Show();
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        void keyboardHook_GlobalKeyDown(KeyboardHook sender, KeyboardHook.KeyEventArgsExt e)
        {
            if (this.Visible && !settingsShown)
            {
                int inputNum = -1;
                if (e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9)
                {
                    inputNum = e.KeyCode - Keys.D0;
                }
                /*if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
                {
                    inputNum = e.KeyCode - Keys.NumPad0;
                }*/
                if (inputNum >= 0)
                {
                    e.Handled = true;
                    timer1.Stop();
                    TextInputHelper.InputString(resultButtons[(inputNum + 9) % 10].Text);
                    this.handwritingDisplayPanel1.ClearStrokes();
                    this.ink.DeleteStrokes();
                    this.Invalidate();
                }
                else if (e.KeyCode == Keys.OemMinus)
                {
                    e.Handled = true;
                    timer1.Stop();
                    this.handwritingDisplayPanel1.ClearStrokes();
                    this.ink.DeleteStrokes();
                    this.Invalidate();
                }
            }
        }

        void resultButtons_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                timer1.Stop();
                TextInputHelper.InputString(btn.Text);
                this.handwritingDisplayPanel1.ClearStrokes();
                this.ink.DeleteStrokes();
                this.Invalidate();
            }
        }

        Button[] resultButtons;

        void setFormLocation()
        {
            Rectangle area = Screen.PrimaryScreen.WorkingArea;
            this.Left = area.Width - this.Width - 32;
            this.Top = area.Height - this.Height - 32;
        }

        void setFormSize()
        {
            this.ClientSize = new Size(handwritingDisplayPanel1.Width + handwritingDisplayPanel1.Left + 12, handwritingDisplayPanel1.Height + handwritingDisplayPanel1.Top + 12);
        }

        void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((EventHandler)SystemEvents_DisplaySettingsChanged, sender, e);
            }
            else
            {
                this.setFormLocation();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                this.keyboardHook.Dispose();
            }
            base.Dispose(disposing);
        }

        void touchPad_FingerDown(ITouchPad sender, FingerEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((FingerEventHandler)touchPad_FingerDown, sender, e);
            }
            else
            {
                if (Properties.Settings.Default.FirstRun)
                {
                    this.tip.Active = false;
                }
                this.timer1.Stop();
                this.handwritingDisplayPanel1.AddStrokePoint(e.X, e.Y);
            }
        }

        void touchPad_FingerMove(ITouchPad sender, FingerEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((FingerEventHandler)touchPad_FingerMove, sender, e);
            }
            else
            {
                this.handwritingDisplayPanel1.AddStrokePoint(e.X, e.Y);
            }
        }

        void touchPad_FingerUp(ITouchPad sender, FingerEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((FingerEventHandler)touchPad_FingerUp, sender, e);
            }
            else
            {
                //this.handwritingDisplayPanel1.AddStrokePoint(e.X, e.Y);
                this.ink.CreateStroke(this.handwritingDisplayPanel1.EndStroke());
                this.updateRecognitionResult();
                //RecognitionResult result = this.ink.Strokes.RecognitionResult;
                ////this.recognizerContext.Strokes = this.ink.Strokes;
                //RecognitionStatus status;
                //RecognitionResult result = this.recognizerContext.Recognize(out status);
                //System.Diagnostics.Debug.WriteLine(result.TopString);
                if (this.settings.AutoInsertionEnabled)
                {
                    this.timer1.Start();
                }
            }
        }

        void dblPressDetector_KeyDoublePressed(KeyboardDoublePressDetector sender, KeyboardDoublePressDetector.KeyboardDoublePressEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke((KeyboardDoublePressDetector.KeyboardDoublePressEventHandler)dblPressDetector_KeyDoublePressed, sender, e);
            }
            else
            {
                if (this.Visible)
                {
                    this.Hide();
                }
                else
                {
                    this.allowShow = true;
                    this.Show();
                }
            }
        }

        readonly TouchPad touchPad = new TouchPad();
        readonly KeyboardHook keyboardHook = new KeyboardHook();
        readonly KeyboardDoublePressDetector dblPressDetector;

        RecognizerContext recognizerContext;

        Ink ink = new Ink();

        bool allowShow = false;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                //cp.ExStyle |= 0x8000000 /* WS_EX_NOACTIVATED */ | 0x80000 /* WS_EX_LAYERED */ | 0x20 /* WS_EX_TRANSPARENT */ | 0x80 /* WS_EX_TOOLWINDOW */;
                //cp.ExStyle |= 0x80000 /* WS_EX_LAYERED */ | 0x20 /* WS_EX_TRANSPARENT */;
                cp.ExStyle |= 0x8000000 /* WS_EX_NOACTIVATED */;
                return cp;
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            if (!this.allowShow)
                value = false;
            base.SetVisibleCore(value);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                this.dblPressDetector.Enabled = false;
                this.dblPressDetector.KeyDoublePressed -= new KeyboardDoublePressDetector.KeyboardDoublePressEventHandler(dblPressDetector_KeyDoublePressed);
                this.keyboardHook.Enabled = false;
                this.keyboardHook.GlobalKeyDown -= new KeyboardHook.KeyEventHandlerExt(keyboardHook_GlobalKeyDown);
                UnregisterHotKey(this.Handle, HOTKEY_ID);
                Microsoft.Win32.SystemEvents.DisplaySettingsChanged -= new EventHandler(SystemEvents_DisplaySettingsChanged);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void handwritingDisplayPanel1_Resize(object sender, EventArgs e)
        {
            this.setFormSize();
        }

        bool settingsShown = false;

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!settingsShown)
            {
                settingsShown = true;
                if (new FormSettings().ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Program.restartFlag = true;
                    this.Close();
                    this.Dispose();
                    return;
                }
                settingsShown = false;
            }
        }

        private void FormMain_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                try
                {
                    this.touchPad.ExclusiveCapture = true;
                }
                catch (UnauthorizedAccessException)
                {
                    // Usually happens when hotkey set to Left Control and user is zooming using Synaptics touchpad
                    this.Hide();
                    return;
                }
                this.touchPad.ShouldRaiseEvents = true;
            }
            else
            {
                timer1.Stop();
                this.handwritingDisplayPanel1.ClearStrokes();
                this.ink.DeleteStrokes();
                this.Invalidate();
                this.touchPad.ExclusiveCapture = false;
                this.touchPad.ShouldRaiseEvents = false;
            }
        }

        private string[] recognize()
        {
            List<string> results = new List<string>(10);
            this.recognizerContext.Strokes = this.ink.Strokes;
            RecognitionStatus status;
            RecognitionResult result = this.recognizerContext.Recognize(out status);
            if (status == RecognitionStatus.NoError)
            {
                RecognitionAlternates alts = result.GetAlternatesFromSelection(0, -1, 10);
                foreach (var alt in alts)
                {
                    results.Add(alt.ToString());
                }
            }
            return results.ToArray();
        }

        private void updateRecognitionResult()
        {
            string[] results = this.recognize();
            if (results.Length > 0)
            {
                for (int i = 0; i < results.Length; i++)
                {
                    resultButtons[i].Text = results[i];
                }
                for (int i = results.Length; i < 10; i++)
                {
                    resultButtons[i].Text = "";
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            TextInputHelper.InputString(resultButtons[0].Text);
            this.handwritingDisplayPanel1.ClearStrokes();
            this.ink.DeleteStrokes();
            this.Invalidate();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!settingsShown)
            {
                settingsShown = true;
                if (new FormSettings().ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Program.restartFlag = true;
                    this.Close();
                    this.Dispose();
                    return;
                }
                settingsShown = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            this.handwritingDisplayPanel1.ClearStrokes();
            this.ink.DeleteStrokes();
            this.Invalidate();
        }

        ToolTip tip = new ToolTip();

        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FirstRun)
            {
                this.notifyIcon1.Visible = false; // Hides the balloon
                this.notifyIcon1.Visible = true;
                tip.IsBalloon = true;
                tip.ToolTipIcon = ToolTipIcon.Info;
                tip.ToolTipTitle = Resources.Messages.TipWriteTitle;
                tip.Show("", this.handwritingDisplayPanel1, 0); //HACK: Workaround of a "known bug"... hopefully
                tip.Show(Resources.Messages.TipWriteText, this.handwritingDisplayPanel1, new Point(this.handwritingDisplayPanel1.Width / 2, this.handwritingDisplayPanel1.Height / 2));
                Properties.Settings.Default.FirstRun = false;
                Properties.Settings.Default.Save();
            }
        }
    }
}
