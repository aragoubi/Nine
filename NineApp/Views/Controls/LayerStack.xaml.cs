using System.Windows.Controls;
using Nine.Tools;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for LayerStack.xaml
    /// </summary>
    public partial class LayerStack : UserControl
    {
        public LayerStack()
        {
            InitializeComponent();
        }

        private void PointerCanvas_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
            PointerManager.Instance.StrokeAdded(e.Stroke);
        }

        private void PointerCanvas_Gesture(object sender, InkCanvasGestureEventArgs e)
        {
            e.Handled = true;
            e.Cancel = true;
        }
    }
}