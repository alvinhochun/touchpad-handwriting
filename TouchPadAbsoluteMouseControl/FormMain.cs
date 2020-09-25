using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using AlvinHoChun.SynapticsTouchPad;
using alvinhc.TouchPadInterface;

namespace TouchPadAbsoluteMouseControl
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            setRegion(Screen.PrimaryScreen.Bounds);
            touchPad.FingerDown += new FingerEventHandler(touchPad_FingerDown);
            touchPad.FingerMove += new FingerEventHandler(touchPad_FingerMove);
            touchPad.FingerUp += new FingerEventHandler(touchPad_FingerUp);
        }

        Rectangle region;

        void setRegion(Rectangle rec)
        {
            region = rec;
            txtTop.Text = string.Format("Top: {0}", rec.Top);
            txtHeight.Text = string.Format("Height: {0}", rec.Height);
            txtLeft.Text = string.Format("Left: {0}", rec.Left);
            txtWidth.Text = string.Format("Width: {0}", rec.Width);
        }

        private void btnSelectRegion_Click(object sender, EventArgs e)
        {
            this.Hide();
            setRegion(FormSelectRegion.doSelectRegion(region));
            this.Show();
            this.BringToFront();
        }

        FormShowRegion formShowRegion = null;

        private void chkEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnabled.Checked)
            {
                enabled = true;
                touchPad.ExclusiveCapture = true;
                formShowRegion = new FormShowRegion();
                formShowRegion.SetLocationAndSize(region);
                formShowRegion.Show();
            }
            else
            {
                enabled = false;
                touchPad.ExclusiveCapture = false;
                formShowRegion.Close();
                formShowRegion.Dispose();
                formShowRegion = null;
            }
        }

        //[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
        //static extern void mouse_event(MouseEventFlags flags, uint dx, uint dy, uint delta, IntPtr extraInfo);

        //[Flags]
        //enum MouseEventFlags : uint
        //{
        //    Absolute = 0x8000,
        //    LeftDown = 0x0002,
        //    LeftUp = 0x0004,
        //    MiddleDown = 0x0020,
        //    MiddleUp = 0x0040,
        //    Move = 0x0001,
        //    RightDown = 0x0008,
        //    RightUp = 0x0010,
        //    Wheel = 0x0800,
        //    XDown = 0x0080,
        //    XUp = 0x0100,
        //    HWheel = 0x1000,
        //}

        readonly TouchPad touchPad = new TouchPad();
        bool enabled = false;

        void touchPad_FingerDown(ITouchPad sender, FingerEventArgs e)
        {
            if (enabled)
            {
                Cursor.Position = new Point((int)(e.X * region.Width) + region.Left, (int)(e.Y * region.Height) + region.Top);
                //mouse_event(MouseEventFlags.LeftDown, 0, 0, 0, IntPtr.Zero);
                uint a = InputSender.SendInput(new InputSender.Input[]{
                    new InputSender.Input{
                        type = InputSender.InputType.Mouse,
                        mi = new InputSender.MouseInput{
                            flags = InputSender.MouseEventFlags.LeftDown,
                        }
                    }
                });
            }
        }

        void touchPad_FingerMove(ITouchPad sender, FingerEventArgs e)
        {
            if (enabled)
            {
                Cursor.Position = new Point((int)(e.X * region.Width) + region.Left, (int)(e.Y * region.Height) + region.Top);
            }
        }

        void touchPad_FingerUp(ITouchPad sender, FingerEventArgs e)
        {
            if (enabled)
            {
                Cursor.Position = new Point((int)(e.X * region.Width) + region.Left, (int)(e.Y * region.Height) + region.Top);
                //mouse_event(MouseEventFlags.LeftUp, 0, 0, 0, IntPtr.Zero);
                InputSender.SendInput(new InputSender.Input[]{
                    new InputSender.Input{
                        type = InputSender.InputType.Mouse,
                        mi = new InputSender.MouseInput{
                            flags = InputSender.MouseEventFlags.LeftUp,
                        }
                    }
                });
            }
        }
    }
}
