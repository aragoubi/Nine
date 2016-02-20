using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using Nine.MVVM;

namespace Nine.ViewModels.Controls
{
    /// <summary>
    /// Left side-bar (precisely, a flyout), this class handle the opening/closing
    /// </summary>
    public class SideBar : BaseViewModel
    {
        private ICommand openMenu;

        public ICommand OpenMenu
        {
            get
            {
                if (openMenu == null)
                {
                    openMenu = new RelayCommand(() =>
                    {
                        var mainwindow = Application.Current.MainWindow as MetroWindow;
                        var flyout = mainwindow.Flyouts.Items[0] as Flyout;
                        flyout.IsOpen = !flyout.IsOpen;
                    });
                }
                return openMenu;
            }
            set
            {
                openMenu = value;
                RaisePropertyChanged();
            }
        }
    }
}