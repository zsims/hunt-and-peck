using System.ComponentModel;
using System.Windows.Media;
using hap.NativeMethods;
using System;
using System.Windows;
using System.Windows.Interop;
using hap.ViewModels;

namespace hap.Views
{
    /// <summary>
    /// Interaction logic for OverlayView.xaml
    /// </summary>
    public partial class OverlayView : Window 
    {
        private bool _closing;

        public OverlayView()
        {
            InitializeComponent();
        }

        private void HintWindow_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            // We could have lost focus because we're already closing, make sure this doesn't call close twice
            if (!_closing)
            {
                Close();
            }
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
                if(targetThread == appThread)
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

        private void OverlayView_OnClosing(object sender, CancelEventArgs e)
        {
            _closing = true;
        }

        private void OverlayView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var m = PresentationSource.FromVisual(this).CompositionTarget.TransformToDevice;
            var scaleX = m.M11;
            var scaleY = m.M22;

            // scale the items for non-96 DPIs
            layoutGrid.LayoutTransform = new ScaleTransform(1/scaleX, 1/scaleY);

            // resize the window for non-96 DPIs
            var vm = DataContext as OverlayViewModel;
            Left = vm.Bounds.Left / scaleX;
            Top = vm.Bounds.Top / scaleY;
            Width = vm.Bounds.Width / scaleX;
            Height = vm.Bounds.Height / scaleY;

            // We always want this on top. SetForegroundWindow has a few conditions:
            // https://msdn.microsoft.com/en-us/library/ms633539(VS.85).aspx
            ForceForeground();
        }
    }
}
