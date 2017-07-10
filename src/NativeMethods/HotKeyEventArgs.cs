using System;
using System.Windows.Forms;

namespace HuntAndPeck.NativeMethods
{
    public class HotKeyEventArgs : EventArgs
    {
        public readonly Keys Key;
        public readonly KeyModifier Modifiers;

        public HotKeyEventArgs(Keys key, KeyModifier modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public HotKeyEventArgs(IntPtr hotKeyParam)
        {
            var param = (uint)hotKeyParam.ToInt64();
            Key = (Keys)((param & 0xffff0000) >> 16);
            Modifiers = (KeyModifier)(param & 0x0000ffff);
        }
    }
}
