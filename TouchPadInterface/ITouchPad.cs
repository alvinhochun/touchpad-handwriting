using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace alvinhc.TouchPadInterface
{
    public interface ITouchPad
    {
        /// <summary>
        /// Gets the value indicating whether a finger is presented on the TouchPad.
        /// </summary>
        bool IsFingerDown { get; }

        /// <summary>
        /// Gets the absolute horizontal position of the finger, where 0 represents leftmost and 1 represents rightmost.
        /// </summary>
        double X { get; }

        /// <summary>
        /// Gets the absolute vertical position of the finger, where 0 represents topmost and 1 represents downmost.
        /// </summary>
        double Y { get; }

        /// <summary>
        /// Gets or sets the value indicating whether this object captures the TouchPad exclusively.
        /// If true, the device will not send mouse events to Windows.
        /// </summary>
        bool ExclusiveCapture { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether this object should raise events when finger movements are detected on the TouchPad.
        /// </summary>
        bool ShouldRaiseEvents { get; set; }

        bool Enabled { get; set; }

        /// <summary>
        /// Occurs when a finger touches the touchpad.
        /// </summary>
        /// <remarks>This event may not be raised on the main thread.</remarks>
        event FingerEventHandler FingerDown;

        /// <summary>
        /// Occurs when a finger moves on the touchpad.
        /// </summary>
        /// <remarks>This event may not be raised on the main thread.</remarks>
        event FingerEventHandler FingerMove;

        /// <summary>
        /// Occurs when a finger leaves the touchpad. The event args does not contain valid coordinates.
        /// </summary>
        /// <remarks>This event may not be raised on the main thread.</remarks>
        event FingerEventHandler FingerUp;

        /// <summary>
        /// Occurs when the ExclusiveCapture property has changed.
        /// </summary>
        /// <remarks>This event is raised on the same thread which changes the ExclusiveCapture property.</remarks>
        event ExclusiveCaptureChangedEventHandler ExclusiveCaptureChanged;
    }
}
