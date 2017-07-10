using HuntAndPeck.NativeMethods;
using System;
using System.Windows.Forms;
using HuntAndPeck.Services.Interfaces;

namespace HuntAndPeck.Services
{
    internal class KeyListenerService : Form, IKeyListenerService, IDisposable
    {
        public event EventHandler OnHotKeyActivated;
        public event EventHandler OnDebugHotKeyActivated;

        /// <summary>
        /// Global counter for assigning ids to identiy the hot key registration
        /// </summary>
        private int _hotkeyIdCounter = 0;

        private HotKey _hotKey;
        private HotKey _debugHotKey;

        /// <summary>
        /// Re-registers the current hotkey, unregistering any previous key
        /// </summary>
        private void ReRegisterHotKey(HotKey hotKey)
        {
            // Already registered, have to unregister first
            if (hotKey.RegistrationId > 0)
            {
                User32.UnregisterHotKey(Handle, hotKey.RegistrationId);
            }

            hotKey.RegistrationId = _hotkeyIdCounter++;
            User32.RegisterHotKey(Handle, hotKey.RegistrationId, (uint)hotKey.Modifier, (uint)hotKey.Keys);
        }

        /// <summary>
        /// Gets/sets the current hotkey
        /// </summary>
        /// <remarks>Changing this will cause the current hotkey to be unregistered</remarks>
        public HotKey HotKey
        {
            get
            {
                return _hotKey;
            }
            set
            {
                _hotKey = value;
                ReRegisterHotKey(_hotKey);
            }
        }

        public HotKey DebugHotKey
        {
            get
            {
                return _debugHotKey;
            }
            set
            {
                _debugHotKey = value;
                ReRegisterHotKey(_debugHotKey);
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY)
            {
                var e = new HotKeyEventArgs(m.LParam);

                // Normal hotkey
                if (_hotKey != null &&
                    e.Key == _hotKey.Keys &&
                    e.Modifiers == _hotKey.Modifier &&
                    OnHotKeyActivated != null)
                {
                    OnHotKeyActivated(this, new EventArgs());
                }

                // Debug hotkey
                if (_debugHotKey != null &&
                    e.Key == _debugHotKey.Keys &&
                    e.Modifiers == _debugHotKey.Modifier &&
                    OnDebugHotKeyActivated != null)
                {
                    OnDebugHotKeyActivated(this, new EventArgs());
                }
            }

            base.WndProc(ref m);
        }

        protected override void SetVisibleCore(bool value)
        {
            // Ensures that the window will never be displayed
            base.SetVisibleCore(false);
        }
    }
}
