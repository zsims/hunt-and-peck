using System.ComponentModel;
using hap.NativeMethods;
using System;
using System.Windows;
using System.Windows.Interop;

namespace hap.WpfClient.Views
{
    public class HintWindow : Window
    {
        private static readonly DependencyProperty LogicalScreenBoundsProperty = DependencyProperty.Register(
            "LogicalScreenBounds",
            typeof(Rect),
            typeof(HintWindow),
            new FrameworkPropertyMetadata(new Rect(), OnLogicalScreenBoundsChanged));

        private static void OnLogicalScreenBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dt = (HintWindow)d;
            var bounds = (Rect)e.NewValue;

            dt.Left = bounds.Left;
            dt.Top = bounds.Top;
            dt.Width = bounds.Width;
            dt.Height = bounds.Height;
        }

        public Rect LogicalScreenBounds
        {
            get { return (Rect)GetValue(LogicalScreenBoundsProperty); }
            set { SetValue(LogicalScreenBoundsProperty, value); }
        }
    }

    /// <summary>
    /// Interaction logic for OverlayView.xaml
    /// </summary>
    public partial class OverlayView : HintWindow 
    {
        private bool _closing;

        public OverlayView()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            // We always want this on top. SetForegroundWindow has a few conditions:
            // https://msdn.microsoft.com/en-us/library/ms633539(VS.85).aspx
            ForceForeground();
            base.OnActivated(e);
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
    }
}
