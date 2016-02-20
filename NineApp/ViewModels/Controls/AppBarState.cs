using System.Windows.Input;
using Nine.MVVM;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Describe the state of the AppBar (bottom bar contextualized with the current layer).
    /// </summary>
    public class AppBarState : BaseViewModel
    {
        private ICommand _close;
        private bool _isOpen;
        private ICommand _open;

        public ICommand Open
        {
            get
            {
                if (_open == null)
                {
                    _open = new RelayCommand(() => IsOpen = true);
                }
                return _open;
            }
        }

        public ICommand Close
        {
            get
            {
                if (_close == null)
                {
                    _close = new RelayCommand(() => IsOpen = false);
                }
                return _close;
            }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            private set
            {
                _isOpen = value;
                RaisePropertyChanged();
            }
        }
    }
}