using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TouchPadHandwriting
{
    internal static class TextInputHelper
    {
        class InputSender
        {
            [DllImport("user32.dll")]
            static extern uint SendInput(
                uint nInputs,
                [MarshalAs(UnmanagedType.LPArray), In] Input[] pInputs,
                int cbSize
            );

            internal static uint SendInput(Input[] inputs)
            {
                return SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(Input)));
            }

            [StructLayout(LayoutKind.Explicit)]
            internal struct Input
            {
                [FieldOffset(0)]
                internal InputType type;
                //[FieldOffset(4)]
                //internal MouseInput mi;
                [FieldOffset(4)]
                internal KeyboardInput ki;
                //[FieldOffset(4)]
                //internal HARDWAREINPUT hi;
            }

            internal enum InputType : uint
            {
                //Mouse = 0,
                Keyboard = 1,
                //Hardware = 2,
            }

            [StructLayout(LayoutKind.Explicit, Size = 24)]
            internal struct KeyboardInput
            {
                [FieldOffset(0)]
                internal ushort wVk;
                [FieldOffset(2)]
                internal ushort wScan;
                [FieldOffset(4)]
                internal KeyboardEventFlags dwFlags;
                [FieldOffset(8)]
                internal uint time;
                [FieldOffset(16)]
                IntPtr dwExtraInfo;
            }

            [Flags]
            internal enum KeyboardEventFlags : uint
            {
                ExtendedKey = 0x0001,
                KeyUp = 0x0002,
                ScanCode = 0x0008,
                Unicode = 0x0004,
            }
        }

        //[DllImport("user32.dll")]
        //static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll")]
        //static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr p2);

        //[DllImport("user32.dll")]
        //static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        //[DllImport("user32.dll")]
        //static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        //struct GUITHREADINFO
        //{
        //    public int cbSize;
        //    public int flags;
        //    public IntPtr hwndActive;
        //    public IntPtr hwndFocus;
        //    public IntPtr hwndCapture;
        //    public IntPtr hwndMenuOwner;
        //    public IntPtr hwndMoveSize;
        //    public IntPtr hwndCaret;
        //    public RECT rcCaret;
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //struct RECT
        //{
        //    public int left, top, right, bottom;
        //}

        //internal static IntPtr GetActiveWindowControl()
        //{
        //    IntPtr activeWin = GetForegroundWindow();
        //    if (activeWin == IntPtr.Zero)
        //    {
        //        return IntPtr.Zero;
        //    }
        //    uint threadId = GetWindowThreadProcessId(activeWin, IntPtr.Zero);
        //    GUITHREADINFO guiThreadInfo = new GUITHREADINFO();
        //    guiThreadInfo.cbSize = Marshal.SizeOf(guiThreadInfo);
        //    GetGUIThreadInfo(threadId, ref guiThreadInfo);
        //    if (guiThreadInfo.hwndFocus == IntPtr.Zero)
        //    {
        //        return activeWin; // Example: console
        //    }
        //    else
        //    {
        //        return guiThreadInfo.hwndFocus;
        //    }
        //}

        internal static void PressBackspace()
        {
            InputSender.Input[] inputs = new InputSender.Input[2];
            // Key down
            inputs[0].type = InputSender.InputType.Keyboard;
            inputs[0].ki.wVk = (ushort)0x08;
            // Key up
            inputs[1].type = InputSender.InputType.Keyboard;
            inputs[1].ki.wVk = (ushort)0x08;
            inputs[1].ki.dwFlags = InputSender.KeyboardEventFlags.KeyUp;
            InputSender.SendInput(inputs);
        }

        internal static void InputChar(char c)
        {
            ushort ch = (ushort)c;
            InputSender.Input[] inputs = new InputSender.Input[2];
            // Key down
            inputs[0].type = InputSender.InputType.Keyboard;
            inputs[0].ki.wScan = ch;
            inputs[0].ki.dwFlags = InputSender.KeyboardEventFlags.Unicode;
            // Key up
            inputs[1].type = InputSender.InputType.Keyboard;
            inputs[1].ki.wScan = ch;
            inputs[1].ki.dwFlags = InputSender.KeyboardEventFlags.Unicode | InputSender.KeyboardEventFlags.KeyUp;
            InputSender.SendInput(inputs);
        }

        internal static void InputString(string str)
        {
            char[] chars = str.ToCharArray();
            InputSender.Input[] inputs = new InputSender.Input[chars.Length * 2];

            for (int i = 0; i < chars.Length; i++)
            {
                ushort ch = (ushort)chars[i];
                // Key down
                inputs[i * 2].type = InputSender.InputType.Keyboard;
                inputs[i * 2].ki.wScan = ch;
                inputs[i * 2].ki.dwFlags = InputSender.KeyboardEventFlags.Unicode;
                // Key up
                inputs[i * 2 + 1].type = InputSender.InputType.Keyboard;
                inputs[i * 2 + 1].ki.wScan = ch;
                inputs[i * 2 + 1].ki.dwFlags = InputSender.KeyboardEventFlags.Unicode | InputSender.KeyboardEventFlags.KeyUp;
            }
            InputSender.SendInput(inputs);
            //System.Windows.Forms.SendKeys.Send(str);
            //const uint WM_IME_STARTCOMPOSITION = 0x010D;
            //const uint WM_IME_COMPOSITION = 0x010F;
            //const uint WM_IME_ENDCOMPOSITION = 0x010E;
            //const uint WM_IME_NOTIFY = 0x0282;
            //IntPtr active = GetActiveWindowControl();
            //if (active != IntPtr.Zero)
            //{
            //    for (int i = 0; i < chars.Length; i++)
            //    {
            //        ushort ch = (ushort)chars[i];
            //        SendMessage(active, WM_IME_STARTCOMPOSITION, (IntPtr)0, (IntPtr)0);
            //        //SendMessage(active, WM_IME_COMPOSITION, (IntPtr)(uint)ch, (IntPtr)0x0800);
            //        SendMessage(active, WM_IME_COMPOSITION, (IntPtr)0xA7DA, (IntPtr)0x0800);
            //        SendMessage(active, WM_IME_NOTIFY, (IntPtr)0x010D, (IntPtr)0);
            //        SendMessage(active, WM_IME_ENDCOMPOSITION, (IntPtr)0, (IntPtr)0);
            //        SendMessage(active, WM_IME_NOTIFY, (IntPtr)0x010E, (IntPtr)0);
            //    }
            //}
        }
    }
}
