using System;

namespace HuntAndPeck.NativeMethods
{
    [Flags]
    public enum KeyModifier
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Windows = 8,
        NoRepeat = 0x4000
    }
}
