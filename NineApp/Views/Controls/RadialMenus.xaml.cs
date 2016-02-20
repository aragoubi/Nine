using System.Windows.Controls;
using System.Windows.Input;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for RadialMenus.xaml
    /// </summary>
    public partial class RadialMenus : UserControl
    {
        public RadialMenus()
        {
            InitializeComponent();
        }

        private void RadialMenuItem_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var radialMenuItem = sender as Button;
            if (radialMenuItem.Command != null)
            {
                radialMenuItem.Command.Execute(radialMenuItem.CommandParameter);
            }
        }

        private void RadialMenuItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}