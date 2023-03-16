using HuntAndPeck.Models;
using HuntAndPeck.Properties;

namespace HuntAndPeck.ViewModels
{
    public class HintViewModel : NotifyPropertyChanged
    {
        private string _label;
        private bool _active;
        private string _fontSizeReadValue;
        private System.Windows.Media.Brush _fontBackroundColorReadValue;
        private System.Windows.Media.Brush _fontColorReadValue;

        public HintViewModel(Hint hint)
        {
            Hint = hint;
            FontSizeReadValue = Settings.Default.FontSize;
            FontBackroundColorReadValue = new System.Windows.Media.SolidColorBrush(Settings.Default.FontBackroundColor);
            FontColorReadValue = new System.Windows.Media.SolidColorBrush(Settings.Default.FontColor);
        }

        public Hint Hint { get; set; }

        public bool Active
        {
            get { return _active; }
            set { _active = value; NotifyOfPropertyChange(); }
        }

        public string Label
        {
            get { return _label; }
            set { _label = value; NotifyOfPropertyChange(); }
        }

        public string FontSizeReadValue
        {
            get { return _fontSizeReadValue; }
            set { _fontSizeReadValue = value; NotifyOfPropertyChange(); }
        }

        public System.Windows.Media.Brush FontBackroundColorReadValue
        {
            get { return _fontBackroundColorReadValue;}
            set { _fontBackroundColorReadValue = value; NotifyOfPropertyChange(); }
        }

        public System.Windows.Media.Brush FontColorReadValue
        { 
            get { return _fontColorReadValue; }
            set { _fontColorReadValue = value; NotifyOfPropertyChange(); }
        }
    }
}
