using System.Windows;
using System.Windows.Controls;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Logique d'interaction pour MasterSwitch.xaml
    /// </summary>
    public partial class MasterSwitch : UserControl
    {
        public MasterSwitch()
        {
            InitializeComponent();
            DataContext = new ViewModels.Controls.MasterSwitch();
        }

        private void MasterSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModels.Controls.MasterSwitch).CurrentPage =
                Application.Current.MainWindow.Content.GetType().Name;
        }
    }
}