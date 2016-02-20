using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : UserControl
    {
        public SideBar()
        {
            InitializeComponent();
            DataContext = new ViewModels.Controls.SideBar();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFlyoutMenu();
        }

        /// <summary>
        ///     Opens the left flyout menu.
        /// </summary>
        private void OpenFlyoutMenu()
        {
            var mainwindow = Application.Current.MainWindow as MetroWindow;
            var flyout = mainwindow.Flyouts.Items[0] as Flyout;
            flyout.IsOpen = !flyout.IsOpen;
        }
    }
}