﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using alvinhc.TouchPadInterface;

namespace AlvinHoChun.TouchPadAbsolute
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            touchPad.FingerMove += new FingerEventHandler(touchPad_FingerMove);
        }

        void touchPad_FingerMove(ITouchPad sender, FingerEventArgs e)
        {
            if (InvokeRequired)
            {
                FingerEventHandler h = new FingerEventHandler(touchPad_FingerMove);
                Invoke(h, sender, e);
            }
            else
            {
                panel1.Location = new Point((int)(e.X * this.ClientRectangle.Width), (int)(e.Y * this.ClientRectangle.Height));
            }
        }

        readonly ITouchPad touchPad = new IpcTouchPad(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "SynapticsTouchPadIpcClient.exe"));

        private void chkExclusive_CheckedChanged(object sender, EventArgs e)
        {
            touchPad.ExclusiveCapture = chkExclusive.Checked;
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {

        }

    }
}
