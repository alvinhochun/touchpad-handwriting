using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TouchPadAbsoluteMouseControl
{
    public partial class FormSelectRegion : Form
    {
        public FormSelectRegion(Rectangle region)
        {
            InitializeComponent();
            this.region = region;
            Rectangle virtualDesktop = SystemInformation.VirtualScreen;

            this.numLeft.Minimum = virtualDesktop.Left;
            this.numLeft.Maximum = virtualDesktop.Right;
            this.numLeft.Value = region.Left;

            this.numTop.Minimum = virtualDesktop.Top;
            this.numTop.Maximum = virtualDesktop.Bottom;
            this.numTop.Value = region.Top;

            this.numWidth.Minimum = 0;
            this.numWidth.Maximum = virtualDesktop.Width;
            this.numWidth.Value = region.Width;

            this.numHeight.Minimum = 0;
            this.numHeight.Maximum = virtualDesktop.Height;
            this.numHeight.Value = region.Height;

            this.fShowRegion.forceTopmost = false;
            this.updateRegion();
            this.fShowRegion.Show();
        }

        Rectangle region;

        internal static Rectangle doSelectRegion(Rectangle existingRegion)
        {
            using (FormSelectRegion f = new FormSelectRegion(existingRegion))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    return f.region;
                }
                else
                {
                    return existingRegion;
                }
            }
        }

        FormShowRegion fShowRegion = new FormShowRegion();

        private void updateRegion()
        {
            this.region = new Rectangle((int)this.numLeft.Value, (int)this.numTop.Value, (int)this.numWidth.Value, (int)this.numHeight.Value);
            fShowRegion.SetLocationAndSize(this.region);
        }

        private void num_ValueChanged(object sender, EventArgs e)
        {
            this.updateRegion();
        }

        private void FormSelectRegion_FormClosing(object sender, FormClosingEventArgs e)
        {
            fShowRegion.Close();
            fShowRegion.Dispose();
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            fShowRegion.Hide();
            FormDrawRegion f = new FormDrawRegion();
            f.ShowDialog();
            Point start = f.PointToScreen(f.start);
            Point end = f.PointToScreen(f.end);
            f.Dispose();
            Rectangle r = new Rectangle(start.X, start.Y, end.X - start.X, end.Y - start.Y);
            this.numLeft.Value = r.Left;
            this.numTop.Value = r.Top;
            this.numWidth.Value = r.Width;
            this.numHeight.Value = r.Height;
            this.updateRegion();
            fShowRegion.Show();
        }
    }
}
