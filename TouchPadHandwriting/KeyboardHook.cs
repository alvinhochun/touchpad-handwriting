using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TouchPadHandwriting
{
    internal class KeyboardHook : IDisposable
    {
        IntPtr myHhk = IntPtr.Zero;

        bool enabled = false;

        LowLevelKeyboardHookProc llKbdHookDelegate;

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
                    this.llKbdHookDelegate = this.keyboardHookProc;
                    this.myHhk = SetWindowsHookEx(
                        WindowsHookEnum.KeyboardLowLevel,
                        this.llKbdHookDelegate,
                        System.Diagnostics.Process.GetCurrentProcess().MainModule.BaseAddress,
                        0);
                    if (this.myHhk != IntPtr.Zero)
                    {
                        this.enabled = true;
                    }
                }
                else
                {
                    UnhookWindowsHookEx(this.myHhk);
                    this.llKbdHookDelegate = null;
                    this.enabled = false;
                }
            }
        }


        internal class KeyEventArgsExt : System.Windows.Forms.KeyEventArgs
        {
            internal KeyEventArgsExt(System.Windows.Forms.Keys key, int scancode)
                : base(key)
            {
                this.Scancode = scancode;
            }

            internal int Scancode { get; private set; }
        }

        internal delegate void KeyEventHandlerExt(KeyboardHook sender, KeyEventArgsExt e);

        internal event KeyEventHandlerExt GlobalKeyDown;
        internal event KeyEventHandlerExt GlobalKeyUp;

        bool processKeyDown(KeyboardLowLevelHookStruct data)
        {
            data.scanCode &= 0xFF;
            if ((data.flags & 0x01) == 0x01)
            {
                data.scanCode |= 0x100;
            }
            KeyEventArgsExt e = new KeyEventArgsExt((System.Windows.Forms.Keys)data.vkCode, data.scanCode);
            KeyEventHandlerExt eKeyDown = this.GlobalKeyDown;
            if (eKeyDown != null)
            {
                eKeyDown(this, e);
            }
            return e.Handled;
        }

        bool processKeyUp(KeyboardLowLevelHookStruct data)
        {
            data.scanCode &= 0xFF;
            if ((data.flags & 0x01) == 0x01)
            {
                data.scanCode |= 0x100;
            }
            KeyEventArgsExt e = new KeyEventArgsExt((System.Windows.Forms.Keys)data.vkCode, data.scanCode);
            KeyEventHandlerExt eKeyUp = this.GlobalKeyUp;
            if (eKeyUp != null)
            {
                eKeyUp(this, e);
            }
            return e.Handled;
        }

        int keyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == 0)
            {
                KeyboardHookMessages message = (KeyboardHookMessages)wParam;
                KeyboardLowLevelHookStruct data = (KeyboardLowLevelHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardLowLevelHookStruct));
                bool handled = false;
                if (message == KeyboardHookMessages.KeyDown || message == KeyboardHookMessages.SysKeyDown)
                {
                    handled = processKeyDown(data);
                }
                else if (message == KeyboardHookMessages.KeyUp || message == KeyboardHookMessages.SysKeyUp)
                {
                    handled = processKeyUp(data);
                }
                if (handled)
                {
                    return -1;
                }
            }
            return CallNextHookEx(this.myHhk, nCode, wParam, lParam);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int LowLevelKeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        struct KeyboardLowLevelHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        enum KeyboardHookMessages : uint
        {
            KeyDown = 0x100,
            KeyUp = 0x101,
            SysKeyDown = 0x104,
            SysKeyUp = 0x105,
        }

        enum WindowsHookEnum : int
        {
            KeyboardLowLevel = 13,
        }

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(WindowsHookEnum idHook, LowLevelKeyboardHookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        public void Dispose()
        {
            this.Enabled = false;
        }

        ~KeyboardHook()
        {
            this.Dispose();
        }
    }
}
