using HuntAndPeck.Models;
using HuntAndPeck.Properties;

namespace HuntAndPeck.ViewModels
{
    public class HintViewModel : NotifyPropertyChanged
    {
        private string _label;
        private bool _active;
        private string _fontSizeReadValue;

        public HintViewModel(Hint hint)
        {
            Hint = hint;
            FontSizeReadValue = Settings.Default.FontSize;
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
    }
}
