using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Marshal = System.Runtime.InteropServices.Marshal;

#region Using directives of Synaptics COM API

using SynAPICtrl = SYNCTRLLib.SynAPICtrl;
using SynDeviceCtrl = SYNCTRLLib.SynDeviceCtrl;
using SynPacketCtrl = SYNCTRLLib.SynPacketCtrl;
using SynConnectionType = SYNCTRLLib.SynConnectionType;
using SynDeviceType = SYNCTRLLib.SynDeviceType;
using SynDeviceProperty = SYNCTRLLib.SynDeviceProperty;
using SynOnPacketEventHandler = SYNCTRLLib._ISynDeviceCtrlEvents_OnPacketEventHandler;

#endregion

namespace AlvinHoChun.SynapticsTouchPad
{
    /// <summary>
    /// Represents a Synaptics TouchPad.
    /// </summary>
    public class TouchPad
    {
        #region Private fields

        SynAPICtrl synApiCtrl = new SynAPICtrl();
        SynDeviceCtrl synDeviceCtrl = new SynDeviceCtrl();
        SynPacketCtrl synPacketCtrl = new SynPacketCtrl();
        int deviceHandle = -1;

        int xMin, xMax, yMin, yMax;
        double xDiff, yDiff;

        int x, y;
        bool isFingerDown;
        double xCalc, yCalc;

        bool acquired = false;
        bool raiseEvents = true;
        bool enabled = true;

        #endregion

        #region Constructor

        public TouchPad()
        {
            // Initialize private fields
            x = 0;
            y = 0;

            // Initialize Synaptics API
            synApiCtrl.Initialize();

            // Activates Synaptics API
            synApiCtrl.Activate();

            // Try hard to get a device (usually after resuming from sleep)
            for (int i = 0; deviceHandle < 0 && i < 20; i++, System.Threading.Thread.Sleep(500))
            {
                // Get device handle
                deviceHandle = synApiCtrl.FindDevice(SynConnectionType.SE_ConnectionAny, SynDeviceType.SE_DeviceTouchPad, -1);
            }

            // Select the device
            synDeviceCtrl.Select(deviceHandle);

            // Activate the device
            synDeviceCtrl.Activate();

            // Get device properties
            //xMin = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_XLoSensor);
            //xMax = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_XHiSensor);
            xMin = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_XLoBorder);
            xMax = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_XHiBorder);
            xDiff = xMax - xMin;
            //yMin = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_YLoSensor);
            //yMax = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_YHiSensor);
            yMin = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_YLoBorder);
            yMax = synDeviceCtrl.GetLongProperty(SynDeviceProperty.SP_YHiBorder);
            yDiff = yMax - yMin;

            // Add event listener
            synDeviceCtrl.OnPacket += new SynOnPacketEventHandler(synDeviceCtrl_OnPacket);
        }

        #endregion

        #region Cleanup

        //~TouchPad()
        //{
        //    // try to prevent strange errors
        //    synDeviceCtrl.OnPacket -= new SynOnPacketEventHandler(synDeviceCtrl_OnPacket);
        //}

        #endregion

        #region Private methods

        void synDeviceCtrl_OnPacket()
        {
            bool isFingerDownOld = isFingerDown;
            int xOld = x;
            int yOld = y;
            var xCalcOld = xCalc;
            var yCalcOld = yCalc;

            // Load the packet
            synDeviceCtrl.LoadPacket(synPacketCtrl);

            x = Math.Max(xMin, Math.Min(xMax, synPacketCtrl.X)) - xMin;
            y = yMax - Math.Max(yMin, Math.Min(yMax, synPacketCtrl.Y));
            isFingerDown = ((SynFingerStateFlags)synPacketCtrl.FingerState & SynFingerStateFlags.FingerPresent) != 0;

            xCalc = (double)x / xDiff;
            yCalc = (double)y / yDiff;

            //System.Diagnostics.Debug.WriteLine("x = {0}, y = {1}, Finger = {2}", synPacketCtrl.X, synPacketCtrl.Y, (SynFingerStateFlags)synPacketCtrl.FingerState);

            if (this.raiseEvents)
            {
                // If finger present
                if (isFingerDown)
                {
                    //System.Diagnostics.Debug.WriteLine("x = {0}, y = {1}, Finger = {2}", synPacketCtrl.X, synPacketCtrl.Y, (SynFingerStateFlags)synPacketCtrl.FingerState);
                    if (!isFingerDownOld)
                    {
                        FingerEventHandler fDown = this.FingerDown;
                        if (fDown != null)
                        {
                            fDown(this, new FingerEventArgs(true, xCalc, yCalc));
                        }
                    }
                    //else if (x != xOld || y != yOld)
                    else if (((SynFingerStateFlags)synPacketCtrl.FingerState & SynFingerStateFlags.FingerMotion) != 0)
                    {
                        FingerEventHandler fMove = this.FingerMove;
                        if (fMove != null)
                        {
                            fMove(this, new FingerEventArgs(true, xCalc, yCalc));
                        }
                    }
                }
                else
                {
                    if (isFingerDownOld)
                    {
                        //System.Diagnostics.Debug.WriteLine("x = {0}, y = {1}, Finger = {2}", synPacketCtrl.X, synPacketCtrl.Y, (SynFingerStateFlags)synPacketCtrl.FingerState);
                        FingerEventHandler fUp = this.FingerUp;
                        if (fUp != null)
                        {
                            // The coordinates from SynCOMAPI after finger up can be invalid,
                            // so we provide the old coordinates.
                            fUp(this, new FingerEventArgs(true, xCalcOld, yCalcOld));
                        }
                    }
                }
            }
        }

        #endregion

        #region Public events

        public class FingerEventArgs : EventArgs
        {
            private bool fingerDown;
            private double x, y;

            public bool FingerDown { get { return fingerDown; } }
            public double X { get { return x; } }
            public double Y { get { return y; } }

            public FingerEventArgs(bool fingerDown, double x, double y)
            {
                this.fingerDown = fingerDown;
                this.x = x;
                this.y = y;
            }
        }
        public delegate void FingerEventHandler(TouchPad sender, FingerEventArgs e);

        /// <summary>
        /// Occurs when a finger touches the touchpad.
        /// </summary>
        /// <remarks>This event may not be raised on the main thread.</remarks>
        public event FingerEventHandler FingerDown;

        /// <summary>
        /// Occurs when a finger moves on the touchpad.
        /// </summary>
        /// <remarks>This event may not be raised on the main thread.</remarks>
        public event FingerEventHandler FingerMove;

        /// <summary>
        /// Occurs when a finger leaves the touchpad.
        /// </summary>
        /// <remarks>This event may not be raised on the main thread.</remarks>
        public event FingerEventHandler FingerUp;

        public delegate void ExclusiveCaptureChangedEventHandler(TouchPad sender, EventArgs e);

        /// <summary>
        /// Occurs when the ExclusiveCapture property has changed.
        /// </summary>
        /// <remarks>This event is raised on the same thread which changes the ExclusiveCapture property.</remarks>
        public event ExclusiveCaptureChangedEventHandler ExclusiveCaptureChanged;

        #endregion

        #region Public readonly properties

        /// <summary>
        /// Gets the value indicating whether a finger is presented on the TouchPad.
        /// </summary>
        public bool IsFingerDown
        {
            get
            {
                return isFingerDown;
            }
        }

        /// <summary>
        /// Gets the absolute horizontal position of the finger, where 0 represents leftmost and 1 represents rightmost.
        /// </summary>
        public double X
        {
            get
            {
                if (!IsFingerDown)
                    return 0.0;
                return xCalc;
            }
        }

        /// <summary>
        /// Gets the absolute vertical position of the finger, where 0 represents topmost and 1 represents downmost.
        /// </summary>
        public double Y
        {
            get
            {
                if (!IsFingerDown)
                    return 0.0;
                return yCalc;
            }
        }

        #endregion

        #region Public read/write properties

        /// <summary>
        /// Gets or sets the value indicating whether this object captures the TouchPad exclusively.
        /// If true, the device will not send mouse events to Windows.
        /// </summary>
        public bool ExclusiveCapture
        {
            get
            {
                return acquired;
            }
            set
            {
                if (value == acquired)
                    return;
                if (value)
                {
                    synDeviceCtrl.Acquire(0);
                }
                else
                {
                    synDeviceCtrl.Unacquire();
                }
                acquired = value;
                if (ExclusiveCaptureChanged != null)
                {
                    ExclusiveCaptureChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the value indicating whether this object should raise events when finger movements are detected on the TouchPad.
        /// </summary>
        public bool ShouldRaiseEvents
        {
            get
            {
                return this.raiseEvents;
            }
            set
            {
                if (value == this.raiseEvents)
                    return;
                this.raiseEvents = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                if (value == this.enabled)
                    return;
                if (value)
                {
                    synDeviceCtrl.Activate();
                    synDeviceCtrl.OnPacket += new SynOnPacketEventHandler(synDeviceCtrl_OnPacket);
                }
                else
                {
                    synDeviceCtrl.OnPacket -= new SynOnPacketEventHandler(synDeviceCtrl_OnPacket);
                    synDeviceCtrl.Deactivate();
                }
                this.enabled = value;
            }
        }

        #endregion
    }
}
