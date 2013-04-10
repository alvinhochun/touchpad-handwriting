using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Keys = System.Windows.Forms.Keys;
using Stopwatch = System.Diagnostics.Stopwatch;

namespace TouchPadHandwriting
{
    internal class KeyboardDoublePressDetector
    {
        internal KeyboardDoublePressDetector(KeyboardHook kbdHook)
        {
            this.keyboardHook = kbdHook;
        }

        KeyboardHook keyboardHook;

        bool enabled;

        Keys key = Keys.None;
        int scancode = 0;
        bool useScancode = true;
        int interval = 500;

        bool isKeyDown = false;
        bool hasPressedOnce = false;
        Stopwatch pressIntervalStopwatch = new Stopwatch();

        internal class KeyboardDoublePressEventArgs : EventArgs
        {
            internal KeyboardDoublePressEventArgs(Keys key)
            {
                this.Key = key;
                this.Scancode = 0;
                this.IsScanCode = false;
            }

            internal KeyboardDoublePressEventArgs(int scanCode)
            {
                this.Key = Keys.None;
                this.Scancode = scanCode;
                this.IsScanCode = true;
            }

            internal Keys Key { get; private set; }
            internal int Scancode { get; private set; }
            internal bool IsScanCode { get; private set; }
        }

        internal delegate void KeyboardDoublePressEventHandler(KeyboardDoublePressDetector sender, KeyboardDoublePressEventArgs e);

        internal event KeyboardDoublePressEventHandler KeyDoublePressed;

        internal void UseKey(Keys key)
        {
            this.key = key;
            this.useScancode = false;
        }

        internal void UseScanCode(int scanCode)
        {
            this.scancode = scanCode;
            this.useScancode = true;
        }

        internal bool Enabled
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
                    this.keyboardHook.GlobalKeyDown += new KeyboardHook.KeyEventHandlerExt(this.keyboardHook_GlobalKeyDown);
                    this.keyboardHook.GlobalKeyUp += new KeyboardHook.KeyEventHandlerExt(this.keyboardHook_GlobalKeyUp);
                    this.enabled = true;
                }
                else
                {
                    this.keyboardHook.GlobalKeyDown -= new KeyboardHook.KeyEventHandlerExt(this.keyboardHook_GlobalKeyDown);
                    this.keyboardHook.GlobalKeyUp -= new KeyboardHook.KeyEventHandlerExt(this.keyboardHook_GlobalKeyUp);
                    this.enabled = false;
                }
            }
        }

        void keyboardHook_GlobalKeyDown(KeyboardHook sender, KeyboardHook.KeyEventArgsExt e)
        {
            //System.Diagnostics.Debug.WriteLine("Down Key = {0}, ScanCode = {1}", e.KeyCode, e.ScanCode);
            if (useScancode ? e.Scancode == scancode : e.KeyCode == key)
            {
                if (!isKeyDown)
                {
                    isKeyDown = true;
                    if (hasPressedOnce)
                    {
                        if (pressIntervalStopwatch.ElapsedMilliseconds > interval)
                        {
                            hasPressedOnce = false;
                            pressIntervalStopwatch.Reset();
                            pressIntervalStopwatch.Start();
                        }
                    }
                    else
                    {
                        pressIntervalStopwatch.Reset();
                        pressIntervalStopwatch.Start();
                    }
                }
            }
            else
            {
                isKeyDown = false;
                hasPressedOnce = false;
            }
        }

        void keyboardHook_GlobalKeyUp(KeyboardHook sender, KeyboardHook.KeyEventArgsExt e)
        {
            //System.Diagnostics.Debug.WriteLine("Up   Key = {0}, ScanCode = {1}", e.KeyCode, e.ScanCode);
            if (useScancode ? e.Scancode == scancode : e.KeyCode == key)
            {
                if (isKeyDown)
                {
                    isKeyDown = false;
                    if (hasPressedOnce)
                    {
                        pressIntervalStopwatch.Stop();
                        hasPressedOnce = false;
                        if (pressIntervalStopwatch.ElapsedMilliseconds <= interval)
                        {
                            //System.Diagnostics.Debug.WriteLine("Yeah!");
                            KeyboardDoublePressEventHandler handler = this.KeyDoublePressed;
                            if (handler != null)
                            {
                                handler(this, this.useScancode ? new KeyboardDoublePressEventArgs(this.scancode) : new KeyboardDoublePressEventArgs(this.key));
                            }
                        }
                    }
                    else
                    {
                        hasPressedOnce = true;
                    }
                }
            }
            else
            {
                isKeyDown = false;
                hasPressedOnce = false;
            }
        }
    }
}
