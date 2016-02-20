using MahApps.Metro.Controls;
using Nine.ViewModels.Controls;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Logique d'interaction pour ShareFlyout.xaml
    /// </summary>
    public partial class ShareFlyout : Flyout
    {
        public ShareFlyout()
        {
            InitializeComponent();
            DataContext = new ShareContent();
        }
    }
}