using MahApps.Metro.Controls;
using Nine.ViewModels.Windows;


namespace Nine.Views.Windows
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new NineManagement();
            Closing += (DataContext as NineManagement).MainClose;
        }
    }
}