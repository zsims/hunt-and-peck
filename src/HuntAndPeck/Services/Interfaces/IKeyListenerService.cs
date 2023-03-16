using System;
using System.Windows.Forms;
using HuntAndPeck.NativeMethods;
using EnumsNET;

namespace HuntAndPeck.Services.Interfaces
{
    internal class HotKey
    {
        public KeyModifier Modifier { get; set; }
        public Keys Keys { get; set; }

        public HotKey(string pomString)
        {
            int i = pomString.LastIndexOf('+');
            Keys = Enums.Parse<Keys>(pomString.Substring(i + 1));
            Modifier = FlagEnums.ParseFlags<KeyModifier>(pomString.Substring(0, i),
                                                         ignoreCase: true,
                                                         delimiter: "+");
    }

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
        event EventHandler OnTaskbarHotKeyActivated;
        event EventHandler OnDebugHotKeyActivated;

        HotKey TaskbarHotKey { get; set; }
        HotKey HotKey { get; set; }
        HotKey DebugHotKey { get; set; }
    }
}
