using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using AlvinHoChun.SynapticsTouchPad;
using alvinhc.TouchPadInterface;

namespace alvinhc.SynapticsTouchPadIpcClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length != 3)
            {
                Console.WriteLine("Expected 2 arguments.");
                Environment.Exit(1);
            }
            switch (args[1])
            {
                case "run":
                    Run(args[2]);
                    break;
                case "print":
                    Print(args[2]);
                    break;
                default:
                    Console.WriteLine("Unexpected argument.");
                    break;
            }
        }

        static void Run(string pipeName)
        {
            using (var pipe = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
            {
                pipe.WaitForConnection();
                // Write "Protocol Version" message (" TPV0001").
                pipe.Write(new byte[] { 0x20, 0x54, 0x50, 0x56, 0x30, 0x30, 0x30, 0x31 }, 0, 8);
                var touchPad = new TouchPad();
                touchPad.Enabled = false;
                void tryWrite(byte[] data)
                {
                    try
                    {
                        pipe.WriteAsync(data, 0, data.Length);
                    }
                    catch (System.IO.IOException ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to write to pipe: {0}", ex);
                        touchPad.ShouldRaiseEvents = false;
                        touchPad.Enabled = false;
                    }
                }
                touchPad.FingerDown += (sender, e) =>
                {
                    tryWrite(MakeFingerEventMsg(State.Pressed, e));
                };
                touchPad.FingerMove += (sender, e) =>
                {
                    tryWrite(MakeFingerEventMsg(State.Move, e));
                };
                touchPad.FingerUp += (sender, e) =>
                {
                    tryWrite(MakeFingerEventMsg(State.Released, e));
                };
                var task = Task.Run(async () =>
                {
                    while (true)
                    {
                        var buffer = new byte[2];
                        var offset = 0;
                        while (offset < 2)
                        {
                            var read = await pipe.ReadAsync(buffer, offset, 2 - offset);
                            if (read == 0)
                            {
                                throw new System.IO.EndOfStreamException();
                            }
                            offset += read;
                        }
                        if (buffer[0] == 0xa1)
                        {
                            switch (buffer[1])
                            {
                                case 0x00:
                                    touchPad.Enabled = false;
                                    break;
                                case 0x01:
                                    touchPad.Enabled = true;
                                    break;
                                default:
                                    throw new System.IO.InvalidDataException("Invalid value in Set Enable message");
                            }
                        }
                        else if (buffer[0] == 0xa2)
                        {
                            switch (buffer[1])
                            {
                                case 0x00:
                                    touchPad.ExclusiveCapture = false;
                                    break;
                                case 0x01:
                                    touchPad.ExclusiveCapture = true;
                                    break;
                                default:
                                    throw new System.IO.InvalidDataException("Invalid value in Set Exclusive Capture message");
                            }
                        }
                        else
                        {
                            throw new System.IO.InvalidDataException("Invalid message type");
                        }
                    }
                });
                try
                {
                    task.Wait();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Pipe reading task threw exception: {0}", ex);
                }
            }
        }

        enum State : byte
        {
            Released = 0x00,
            Pressed = 0x01,
            Move = 0x02,
        }

        static byte[] MakeFingerEventMsg(State state, FingerEventArgs args)
        {
            var x = (ushort)Math.Round(args.X * 65535.0);
            var y = (ushort)Math.Round(args.Y * 65535.0);
            return new byte[] {
                0x01,
                // state
                (byte)state,
                // finger_id
                0,
                // finger_x
                (byte)(x & 0xff), (byte)(x >> 8),
                // finger_y
                (byte)(y & 0xff), (byte)(y >> 8),
            };
        }

        static void Print(string prop)
        {
            if (prop == "supports_v1")
            {
                Console.WriteLine("1");
            }
            else if (prop == "display_name" || prop.StartsWith("display_name_"))
            {
                Console.WriteLine("Synaptics TouchPad");
            }
            else
            {
                Console.WriteLine("???");
                Environment.Exit(2);
                return;
            }
            Environment.Exit(0);
        }
    }
}
