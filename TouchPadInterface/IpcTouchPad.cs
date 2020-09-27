using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace alvinhc.TouchPadInterface
{
    public class IpcTouchPad : ITouchPad, IDisposable
    {
        enum Request
        {
            Enable,
            Disable,
            SetExclusiveCapture,
            UnsetExclusiveCapture,
            SetShouldRaiseEvents,
            UnsetShouldRaiseEvents,
        }

        static class Messages
        {
            internal struct FingerEvent
            {
                internal enum State : byte
                {
                    Released = 0x00,
                    Pressed = 0x01,
                    Move = 0x02,
                }

                internal FingerEvent(State state, byte fingerId, ushort fingerX, ushort fingerY)
                {
                    this.state = state;
                    this.fingerId = fingerId;
                    this.fingerX = fingerX;
                    this.fingerY = fingerY;
                }

                internal readonly State state;
                internal readonly byte fingerId;
                internal readonly ushort fingerX;
                internal readonly ushort fingerY;
            }
        }

        static async Task<Messages.FingerEvent> readMessageAsync(NamedPipeClientStream stream, CancellationToken cancellationToken)
        {
            var buffer = new byte[6];
            {
                var read = await stream.ReadAsync(buffer, 0, 1, cancellationToken);
                if (read != 1)
                {
                    throw new System.IO.EndOfStreamException();
                }
                if (buffer[0] != 0x01)
                {
                    throw new System.IO.InvalidDataException($"Unexpected byte {buffer[0]} received");
                }
            }
            var offset = 0;
            while (offset < 6)
            {
                var read = await stream.ReadAsync(buffer, offset, 6 - offset, cancellationToken);
                if (read == 0)
                {
                    throw new System.IO.EndOfStreamException();
                }
                offset += read;
            }
            if (!Enum.IsDefined(typeof(Messages.FingerEvent.State), buffer[0]))
            {
                throw new System.IO.InvalidDataException($"Unexpected value {buffer[0]} received for `state`");
            }
            var state = (Messages.FingerEvent.State)buffer[0];
            var fingerId = buffer[1];
            var fingerX = (ushort)(buffer[2] | buffer[3] << 8);
            var fingerY = (ushort)(buffer[4] | buffer[5] << 8);
            return new Messages.FingerEvent(state, fingerId, fingerX, fingerY);
        }

        private CancellationTokenSource cancellationSource;
        private System.Diagnostics.Process process = null;
        private Task task;
        private NamedPipeClientStream pipe;
        private bool _shouldRaiseEvents = true;
        private bool _exclusiveCapture = false;
        private bool _enabled = false;

        public IpcTouchPad(string exePath)
        {
            var pipeName = $"ipcTouchPad-pid{System.Diagnostics.Process.GetCurrentProcess().Id}-{exePath.GetHashCode()}";
            var pipe = new NamedPipeClientStream(".", pipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
            var startInfo = new System.Diagnostics.ProcessStartInfo(exePath, "run " + pipeName);
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            this.process = System.Diagnostics.Process.Start(startInfo);
            // Give it at most 5 seconds to connect...
            pipe.Connect(5000);
            this.InitWithPipe(pipe);
        }

        void InitWithPipe(NamedPipeClientStream pipe)
        {
            {
                int offset = 0;
                const int COUNT = 8;
                var buffer = new byte[COUNT];
                while (offset < COUNT)
                {
                    var read = pipe.Read(buffer, offset, COUNT - offset);
                    if (read == 0)
                    {
                        pipe.Dispose();
                        throw new ArgumentException("Pipe is not connected to a supported client", nameof(pipe));
                    }
                    offset += read;
                }
                // Check for the correct "Protocol Version" message (" TPV0001").
                if (!buffer.SequenceEqual(new byte[] { 0x20, 0x54, 0x50, 0x56, 0x30, 0x30, 0x30, 0x31 }))
                {
                    pipe.Dispose();
                    throw new ArgumentException("Pipe is not connected to a supported client", nameof(pipe));
                }
            }
            this.cancellationSource = new CancellationTokenSource();
            var cancellationToken = this.cancellationSource.Token;
            this.task = Task.Run(async () =>
            {
                var readMessageTask = readMessageAsync(pipe, cancellationToken);
                while (true)
                {
                    var fingerEventMsg = await readMessageTask;
                    readMessageTask = readMessageAsync(pipe, cancellationToken);
                    // Insert memory barrier so that we can get the latest _shouldRaiseEvents and event handlers.
                    Interlocked.MemoryBarrier();
                    if (_shouldRaiseEvents && fingerEventMsg.fingerId == 0)
                    {
                        double x = fingerEventMsg.fingerX / 65535.0;
                        double y = fingerEventMsg.fingerY / 65535.0;
                        switch (fingerEventMsg.state)
                        {
                            case Messages.FingerEvent.State.Pressed:
                                this.FingerDown?.Invoke(this, new FingerEventArgs(true, x, y));
                                break;
                            case Messages.FingerEvent.State.Move:
                                this.FingerMove?.Invoke(this, new FingerEventArgs(true, x, y));
                                break;
                            case Messages.FingerEvent.State.Released:
                                this.FingerUp?.Invoke(this, new FingerEventArgs(false, 0.0, 0.0));
                                break;
                        }
                    }
                }
            });
            this.pipe = pipe;
            this.Enabled = true;
        }

        bool TryWritePipe(byte[] data)
        {
            if (this.pipe == null)
            {
                return false;
            }
            try
            {
                this.pipe.WriteAsync(data, 0, data.Length);
                return true;
            }
            catch (System.IO.IOException ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to write to pipe: {0}", ex);
                Interlocked.Exchange(ref this.pipe, null)?.Dispose();
                return false;
            }
        }

        public bool ExclusiveCapture
        {
            get => _exclusiveCapture;
            set
            {
                if (value == _exclusiveCapture)
                {
                    return;
                }
                if (value)
                {
                    this.TryWritePipe(new byte[] { 0xa2, 0x01 });
                }
                else
                {
                    this.TryWritePipe(new byte[] { 0xa2, 0x00 });
                }
                _exclusiveCapture = value;
            }
        }
        public bool ShouldRaiseEvents
        {
            get => _shouldRaiseEvents;
            set
            {
                if (value == _shouldRaiseEvents)
                {
                    return;
                }
                _shouldRaiseEvents = value;
            }
        }
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == _enabled)
                {
                    return;
                }
                if (value)
                {
                    this.TryWritePipe(new byte[] { 0xa1, 0x01 });
                }
                else
                {
                    this.TryWritePipe(new byte[] { 0xa1, 0x00 });
                }
                _enabled = value;
            }
        }

        public event FingerEventHandler FingerDown;
        public event FingerEventHandler FingerMove;
        public event FingerEventHandler FingerUp;

        public void Close()
        {
            this.cancellationSource.Cancel();
            this.task.Wait();
            this.pipe?.Close();
            if (this.process != null)
            {
                if (!this.process.WaitForExit(1000))
                {
                    this.process.Kill();
                }
                this.process.Close();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Close();
                    this.cancellationSource.Dispose();
                    this.task.Dispose();
                    Interlocked.Exchange(ref this.pipe, null)?.Dispose();
                    this.process?.Dispose();
                    this.process = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
