using System;
using System.Windows;
using System.Windows.Input;
using Nine.MVVM;
using Nine.Tools;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    ///     The ViewModel associated to MasterSwitch control.
    /// </summary>
    public class MasterSwitch : BaseViewModel
    {
        private ICommand _changePageCommand;
        private string _currentPage;

        public string CurrentPage
        {
            get
            {
                if (_currentPage == null)
                {
                    _currentPage = Application.Current.MainWindow.Content.GetType().Name;
                }
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new ParametrizedRelayCommand(page => ChangePage((string) page));
                }
                return _changePageCommand;
            }
        }

        /// <summary>
        ///     Changes the page.
        /// </summary>
        /// <param name="targetPage">The tag corresponding to the target page.</param>
        /// <remarks>If the target page is equal to the current page, nothing is done.</remarks>
        private void ChangePage(string targetPage)
        {
            if (targetPage != CurrentPage)
            {
                Catalog.Instance.NavigateTo(targetPage);
            }
        }

        internal void ResetSwitch()
        {
            throw new NotImplementedException();
        }
    }
}