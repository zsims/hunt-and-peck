using System;
using System.Windows.Forms;
using HuntnPeck.NativeMethods;
using HuntnPeck.Engine.NativeMethods;

namespace HuntnPeck.Forms
{
    public partial class TrayListener : Form
    {
        /// <summary>
        /// Event that's fired when the current hotkey is activated
        /// </summary>
        public delegate void OnKeyActivatedDelegate();
        public event OnKeyActivatedDelegate OnHotKeyActivated;

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
        private Tuple<KeyModifier, Keys> _hotKey;

        /// <summary>
        /// Ctor
        /// </summary>
        public TrayListener()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Re-registers the current hotkey, unregistering any previous key
        /// </summary>
        private void ReRegisterHotkey()
        {
            if (_currentlyRegistered)
            {
                User32.UnregisterHotKey(this.Handle, _hotKeyId);
                _currentlyRegistered = false;
            }

            _hotKeyId++;
            User32.RegisterHotKey(this.Handle, _hotKeyId, (uint)_hotKey.Item1, (uint)_hotKey.Item2);
            _currentlyRegistered = true;
        }

        /// <summary>
        /// Gets/sets the current hotkey
        /// </summary>
        /// <remarks>Changing this will cause the current hotkey to be unregistered</remarks>
        public Tuple<KeyModifier, Keys> HotKey
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
                HotKeyEventArgs e = new HotKeyEventArgs(m.LParam);

                if (e.Key == _hotKey.Item2 &&
                    e.Modifiers == _hotKey.Item1 &&
                    OnHotKeyActivated != null)
                {
                    OnHotKeyActivated();
                }
            }

            base.WndProc(ref m);
        }

        protected override void SetVisibleCore(bool value)
        {
            // Ensures that the window will never be displayed
            base.SetVisibleCore(false);
        }

        #region Event Handlers

        private void menuItemOptions_Click(object sender, System.EventArgs e)
        {
            // TODO
        }

        private void menuItemExit_Click(object sender, System.EventArgs e)
        {
            Application.Exit();
        }

        #endregion
    }
}
