using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TouchPadAbsoluteMouseControl
{
    class InputSender
    {
        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall)]
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
            [FieldOffset(4)]
            internal MouseInput mi;
            //[FieldOffset(4)]
            //internal KEYBDINPUT ki;
            //[FieldOffset(4)]
            //internal HARDWAREINPUT hi;
        }

        internal enum InputType : uint
        {
            Mouse = 0,
            //Keyboard = 1,
            //Hardware = 2,
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MouseInput
        {
            [FieldOffset(0)]
            internal int dx;
            [FieldOffset(4)]
            internal int dy;
            [FieldOffset(8)]
            internal MouseEventDataXButtons mouseDataXButtons;
            [FieldOffset(8)]
            internal int mouseDataWheel;
            [FieldOffset(12)]
            internal MouseEventFlags flags;
            [FieldOffset(16)]
            internal uint time;
            [FieldOffset(20)]
            internal IntPtr extraInfo;
        }

        [Flags]
        internal enum MouseEventDataXButtons : uint
        {
            Nothing = 0x00000000,
            XButton1 = 0x00000001,
            XButton2 = 0x00000002,
        }

        [Flags]
        internal enum MouseEventFlags : uint
        {
            Absolute = 0x8000,
            HWheel = 0x01000,
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            XDown = 0x0080,
            XUp = 0x0100,
        }
    }
}
