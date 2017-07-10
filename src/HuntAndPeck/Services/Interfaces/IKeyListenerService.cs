using System;
using System.Windows.Forms;
using HuntAndPeck.NativeMethods;

namespace HuntAndPeck.Services.Interfaces
{
    internal class HotKey
    {
        public KeyModifier Modifier { get; set; }
        public Keys Keys { get; set; }

        /// <summary>
        /// Id of the hot key registration
        /// </summary>
        public int RegistrationId { get; set; }
    }

    /// <summary>
    /// Service for listening to global keyboard shortcuts
    /// </summary>
    internal interface IKeyListenerService
    {
        event EventHandler OnHotKeyActivated;
        event EventHandler OnDebugHotKeyActivated;

        HotKey HotKey { get; set; }
        HotKey DebugHotKey { get; set; }
    }
}
