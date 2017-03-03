using System.Collections.Generic;
using System.Windows;
using hap.Models;
using System.Linq;

namespace hap.ViewModels
{
    public class DebugOverlayViewModel : NotifyPropertyChanged
    {
        private Rect _bounds;

        public DebugOverlayViewModel(HintSession session)
        {
            Bounds = session.OwningWindowBounds;
            Hints = session.Hints.OfType<DebugHint>().Select(x => new DebugHintViewModel(x)).ToList();
        }

        public List<DebugHintViewModel> Hints { get; set; }

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
    }
}
