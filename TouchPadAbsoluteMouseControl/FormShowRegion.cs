using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TouchPadAbsoluteMouseControl
{
    public partial class FormShowRegion : Form
    {
        public FormShowRegion()
        {
            InitializeComponent();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80000 /* WS_EX_LAYERED */ | 0x20 /* WS_EX_TRANSPARENT */ | 0x80/* WS_EX_TOOLWINDOW */;
                return cp;
            }
        }

        public void SetLocationAndSize(Rectangle rec)
        {
            this.MinimumSize = Size.Empty;
            this.MaximumSize = Size.Empty;
            this.ClientSize = rec.Size;
            this.Location = rec.Location;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        bool flashState = false;

        internal bool forceTopmost = true;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                this.Opacity = (flashState = !flashState) ? 0.05 : 0.0;
                if (this.forceTopmost)
                {
                    this.BringToFront();
                }
            }
        }
    }
}
