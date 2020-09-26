using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alvinhc.TouchPadInterface
{

    public class FingerEventArgs : EventArgs
    {
        public bool FingerDown { get; }
        public double X { get; }
        public double Y { get; }

        public FingerEventArgs(bool fingerDown, double x, double y)
        {
            this.FingerDown = fingerDown;
            this.X = x;
            this.Y = y;
        }
    }

    public delegate void FingerEventHandler(ITouchPad sender, FingerEventArgs e);

}
