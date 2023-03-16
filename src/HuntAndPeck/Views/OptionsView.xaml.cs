using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HuntAndPeck.Views
{
    /// <summary>
    /// Interaction logic for OptionsView.xaml
    /// </summary>
    public partial class OptionsView : Window
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        private void BtnKbdShortWin_Click(object sender, RoutedEventArgs e)
        {
            tBKbdShortWin.Focusable = true;
            tBKbdShortWin.Focus();
        }

        private void BtnKbdShortTray_Click(object sender, RoutedEventArgs e)
        {
            tBKbdShortTray.Focusable = true;
            tBKbdShortTray.Focus();
        }

        private void TbKbdShort_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Focusable= false;
        }

        private void TbKbdShort_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            (sender as TextBox).Text = "";
            // The text box grabs all input.
            e.Handled = true;
            // Fetch the actual shortcut key.
            Key key = (e.Key == Key.System ? e.SystemKey : e.Key);
            // Ignore modifier keys.
            if (    key == Key.LeftShift || key == Key.RightShift
                ||  key == Key.LeftCtrl  || key == Key.RightCtrl
                ||  key == Key.LeftAlt   || key == Key.RightAlt
                ||  key == Key.LWin      || key == Key.RWin )
            {
                return;
            }
            // Build the shortcut key name.
            StringBuilder shortcutText = new StringBuilder();
            if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                shortcutText.Append("Ctrl+");
            }
            if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                shortcutText.Append("Shift+");
            }
            if ((Keyboard.Modifiers & ModifierKeys.Alt) != 0)
            {
                shortcutText.Append("Alt+");
            }
            shortcutText.Append(key.ToString());
            // Update the text box.
            (sender as TextBox).Text = shortcutText.ToString();
        }

        
    }
}
