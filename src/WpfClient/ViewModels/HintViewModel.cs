using Caliburn.Micro;
using hap.Engine.Hints;

namespace hap.WpfClient.ViewModels
{
    public class HintViewModel : PropertyChangedBase
    {
        private string _label;
        private bool _active;

        public HintViewModel(Hint hint)
        {
            Hint = hint;
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
    }
}
