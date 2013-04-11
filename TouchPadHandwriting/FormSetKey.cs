using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TouchPadHandwriting
{
    internal partial class FormSetKey : Form
    {
        public FormSetKey()
        {
            InitializeComponent();
            this.keyboardHook = new KeyboardHook();
            this.keyboardHook.GlobalKeyDown += new KeyboardHook.KeyEventHandlerExt(keyboardHook_GlobalKeyDown);
            this.keyboardHook.Enabled = true;
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

        internal Keys key { get; private set; }
        internal int scancode { get; private set; }

        void keyboardHook_GlobalKeyDown(KeyboardHook sender, KeyboardHook.KeyEventArgsExt e)
        {
            e.Handled = true;
            if (this.InvokeRequired)
            {
                this.BeginInvoke((KeyboardHook.KeyEventHandlerExt)keyboardHook_GlobalKeyDown, sender, e);
            }
            else
            {
                this.key = e.KeyCode;
                this.scancode = e.Scancode;
                this.keyboardHook.Enabled = true;
                this.keyboardHook.GlobalKeyDown -= new KeyboardHook.KeyEventHandlerExt(keyboardHook_GlobalKeyDown);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

        KeyboardHook keyboardHook;
    }
}
