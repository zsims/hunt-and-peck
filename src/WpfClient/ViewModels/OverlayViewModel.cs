using Caliburn.Micro;
using hap.Engine.Hints;
using hap.Engine.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace hap.WpfClient.ViewModels
{
    internal class OverlayViewModel : Screen
    {
        private HintSession _session;
        private Rect _bounds;
        private ObservableCollection<Hint> _hints;
        private readonly IHintLabelService _hintLabelService;

        public OverlayViewModel(
            HintSession session,
            IHintLabelService hintLabelService)
        {
            _session = session;
            _bounds = session.OwningWindowBounds;
            _hintLabelService = hintLabelService;

            _hints = new ObservableCollection<Hint>();
            foreach(var hint in session.Hints)
            {
                _hints.Add(hint);
            }
            hintLabelService.LabelHints(_hints);
        }

        /// <summary>
        /// Bounds in logical screen coordiantes
        /// </summary>
        public Rect Bounds
        {
            get
            {
                return _bounds;
            }
            set
            {
                _bounds = value;
                NotifyOfPropertyChange();
            }
        }

        public ObservableCollection<Hint> Hints
        {
            get
            {
                return _hints;
            }
            set
            {
                _hints = value;
                NotifyOfPropertyChange();
            }
        }

        public string MatchString
        {
            set
            {
                var matching = _hintLabelService.FindMatchingHints(value, _session.Hints);

                if (matching.Count() == 1)
                {
                    matching.First().Invoke();
                    TryClose();
                }
            }
        }
    }
}
