using hap.NativeMethods;
using System;
using System.Windows.Forms;
using hap.Services.Interfaces;

namespace hap.Services
{
    internal class KeyListenerService : Form, IKeyListenerService, IDisposable
    {
        public event EventHandler OnHotKeyActivated;

        /// <summary>
        /// Current hotkey reference id
        /// </summary>
        private int _hotKeyId = 0;

        /// <summary>
        /// Whether a hotkey has been currently registered
        /// </summary>
        private bool _currentlyRegistered;

        /// <summary>
        /// The hotkey
        /// </summary>
        private HotKey _hotKey;

        /// <summary>
        /// Re-registers the current hotkey, unregistering any previous key
        /// </summary>
        private void ReRegisterHotkey()
        {
            if (_currentlyRegistered)
            {
                User32.UnregisterHotKey(Handle, _hotKeyId);
                _currentlyRegistered = false;
            }

            _hotKeyId++;
            User32.RegisterHotKey(Handle, _hotKeyId, (uint)_hotKey.Modifier, (uint)_hotKey.Keys);
            _currentlyRegistered = true;
        }

        /// <summary>
        /// Gets/sets the current hotkey
        /// </summary>
        /// <remarks>Changing this will cause the current hotkey to be unregistered</remarks>
        public HotKey HotKey
        {
            set
            {
                _hotKey = value;
                ReRegisterHotkey();
            }
            get
            {
                return _hotKey;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY)
            {
                var e = new HotKeyEventArgs(m.LParam);

                if (e.Key == _hotKey.Keys &&
                    e.Modifiers == _hotKey.Modifier &&
                    OnHotKeyActivated != null)
                {
                    OnHotKeyActivated(this, new EventArgs());
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
