using hap.NativeMethods;
using System;
using System.Windows.Forms;

namespace hap.WpfClient.Services
{
    internal class HotKey
    {
        public KeyModifier Modifier { get; set; }
        public Keys Keys { get; set; }
    }

    /// <summary>
    /// Service for listening to global keyboard shortcuts
    /// </summary>
    internal interface IKeyListenerService
    {
        event EventHandler OnHotKeyActivated;

        HotKey HotKey { get; set; }
    }
}
