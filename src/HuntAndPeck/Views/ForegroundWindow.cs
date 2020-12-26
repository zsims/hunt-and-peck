using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using HuntAndPeck.NativeMethods;

namespace HuntAndPeck.Views
{
    /// <summary>
    /// Window that is always foreground, and closes when it's not
    /// </summary>
    public class ForegroundWindow : Window
    {
        private bool _closing;
        private bool _initialized;

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (!_initialized)
            {
                // Always want this on top. SetForegroundWindow has a few conditions:
                // https://msdn.microsoft.com/en-us/library/ms633539(VS.85).aspx
                if (!User32.SetForegroundWindow(new WindowInteropHelper(this).Handle))
                {
                    ForceForeground();
                }
                _initialized = true;
            }
            base.OnRender(drawingContext);
        }

        protected override void OnDeactivated(EventArgs e)
        {
            // We could have lost focus because we're already closing, make sure this doesn't call close twice
            if (_initialized && !_closing)
            {
                Close();
            }
            base.OnDeactivated(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _closing = true;
            base.OnClosing(e);
        }

        /// <summary>
        /// Forces the window to the foreground by attaching to the foreground window thread
        /// </summary>
        private void ForceForeground()
        {
            // This is required as there's a few restrictions on when this can be called
            // Per https://msdn.microsoft.com/en-us/library/windows/desktop/ms633539%28v=vs.85%29.aspx

            var targetThread = User32.GetWindowThreadProcessId(User32.GetForegroundWindow(), IntPtr.Zero);
            var appThread = Kernel32.GetCurrentThreadId();
            var attached = false;

            try
            {
                if (targetThread == appThread)
                {
                    // already attached
                    return;
                }

                attached = User32.AttachThreadInput(targetThread, appThread, true);

                if (!attached)
                {
                    // hmm
                    Close();
                    return;
                }

                var ourHandle = new WindowInteropHelper(this).Handle;

                // force us to the forground
                User32.BringWindowToTop(ourHandle);
                User32.SetFocus(ourHandle);
            }
            finally
            {
                if (attached)
                {
                    // unattach
                    User32.AttachThreadInput(targetThread, appThread, false);
                }
            }
        }
    }
}
