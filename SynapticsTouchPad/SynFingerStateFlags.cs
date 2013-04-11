using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlvinHoChun.SynapticsTouchPad
{
    /// <summary>
    /// A replacement of <code>SYNCTRLLib.SynFingerFlags</code> which supports real flags.
    /// </summary>
    [Flags]
    public enum SynFingerStateFlags
    {
        None = 0,
        FingerTap3 = -2147483648,
        FingerAllTap = -822083584,
        FingerAsButton = -818413568,
        FingerAllGest = -817950720,
        FingerAll = -805306880,
        FingerProx = 512,
        FingerTouch = 1024,
        FingerHeavy = 2048,
        FingerPress = 4096,
        FingerPresent = 8192,
        FingerPossTap = 16384,
        FingerStylus = 32768,
        FingerTap = 65536,
        FingerDrag = 131072,
        FingerDragLock = 262144,
        FingerPrimGest = 524288,
        FingerSecGest = 1048576,
        FingerAuxGest = 2097152,
        FingerMotion = 4194304,
        FingerMoving = 8388608,
        FingerTopLeftTap = 16777216,
        FingerTopRightTap = 33554432,
        FingerBottomLeftTap = 67108864,
        FingerBottomRightTap = 134217728,
        FingerAllCorner = 251658240,
        FingerTap2 = 1073741824,
    }
}
