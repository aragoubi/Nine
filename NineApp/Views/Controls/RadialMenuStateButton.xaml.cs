using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Nine.ViewModels.Controls;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for RadialMenuStateButton.xaml
    /// </summary>
    public partial class RadialMenuStateButton : UserControl
    {
        public RadialMenuStateButton()
        {
            InitializeComponent();
        }

        private void OpenRadialMenu(object sender)
        {
            var buttonRadius = 22.5;
            var button = sender as Button;
            var lessonContainer = Tag as TouchScrollViewer;
            var position = button.TranslatePoint(new Point(buttonRadius, buttonRadius), lessonContainer);
            var layerStack = DataContext as ViewModels.Controls.LayerStack;
            var viewport = new Rect(lessonContainer.HorizontalOffset, lessonContainer.VerticalOffset,
                lessonContainer.ViewportWidth, lessonContainer.ViewportHeight);
            layerStack.OpenRadialMenu(viewport, position, RadialMenuState.Levels.Main);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenRadialMenu(sender);
        }

        private void Button_TouchDown(object sender, TouchEventArgs e)
        {
            //FrameworkElement button = sender as FrameworkElement;
            //button.CaptureTouch(e.TouchDevice);
            //e.Handled = true;
        }

        private void Button_TouchUp(object sender, TouchEventArgs e)
        {
            //FrameworkElement button = sender as FrameworkElement;
            //button.ReleaseTouchCapture(e.TouchDevice);
            //e.Handled = true;

            //Rect bounds = new Rect(new Point(0, 0), button.RenderSize);
            //if (bounds.Contains(e.GetTouchPoint(button).Position))
            //{
            //    OpenRadialMenu(sender);
            //}
        }
    }
}