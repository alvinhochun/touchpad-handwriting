using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TouchPadAbsoluteMouseControl
{
    public partial class FormDrawRegion : Form
    {
        public FormDrawRegion()
        {
            InitializeComponent();
            Rectangle virtualDesktop = SystemInformation.VirtualScreen;
            this.ClientSize = virtualDesktop.Size;
            this.Location = virtualDesktop.Location;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80/* WS_EX_TOOLWINDOW */;
                return cp;
            }
        }

        bool started = false;

        internal Point start, end;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (started)
            {
                e.Graphics.FillRectangle(Brushes.Green, start.X, start.Y, end.X - start.X, end.Y - start.Y);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            started = true;
            start = e.Location;
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            end = e.Location;
            base.OnMouseMove(e);
            this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            end = e.Location;
            base.OnMouseUp(e);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.BringToFront();
        }

        private void FormDrawRegion_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!started)
            {
                e.Cancel = true;
            }
        }
    }
}
