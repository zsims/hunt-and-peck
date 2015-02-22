using System.Windows;

namespace hap.WpfClient.Views
{
    public class HintWindow : Window
    {
        private static DependencyProperty LogicalScreenBoundsProperty = DependencyProperty.Register(
            "LogicalScreenBounds",
            typeof(Rect),
            typeof(HintWindow),
            new FrameworkPropertyMetadata(new Rect(), OnLogicalScreenBoundsChanged));

        private static void OnLogicalScreenBoundsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dt = d as HintWindow;
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
        public OverlayView()
        {
            InitializeComponent();

            // We always want this on top. SetForegroundWindow has a few conditions:
            // https://msdn.microsoft.com/en-us/library/ms633539(VS.85).aspx
        }

        private void HintWindow_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            Close();
        }
    }
}
