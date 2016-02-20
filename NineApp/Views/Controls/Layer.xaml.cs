using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace Nine.Views.Controls
{
    /// <summary>
    ///     Interaction logic for Layer.xaml
    /// </summary>
    public partial class Layer : InkCanvas
    {
        public static readonly DependencyProperty EraserShapeProperty = DependencyProperty.Register(
            "EraserShape",
            typeof (StylusShape),
            typeof (Layer),
            new UIPropertyMetadata(null, OnEraserShapePropertyChanged)
            );

        public Layer()
        {
            InitializeComponent();
        }

        public new StylusShape EraserShape
        {
            get { return (StylusShape) GetValue(EraserShapeProperty); }
            set { SetValue(EraserShapeProperty, value); }
        }

        private static void OnEraserShapePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Applies the changes to the original dependency property
            var layer = d as InkCanvas;
            layer.EraserShape = e.NewValue as StylusShape;

            // Simulates a refresh to the View, needed to perform the new stylus representation
            layer.RenderTransform = new MatrixTransform();
        }
    }
}