using System.Windows.Controls;
using Nine.ViewModels.Pages;

namespace Nine.Views.Pages
{
    /// <summary>
    ///     Interaction logic for StartPage.xaml
    /// </summary>
    public partial class UserConnectionPage : Page
    {
        public UserConnectionPage()
        {
            InitializeComponent();
            DataContext = new UserConnection();
        }
    }
}