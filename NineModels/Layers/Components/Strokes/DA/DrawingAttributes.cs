using System;

namespace Nine.Layers.Components.Strokes.DA
{
    /// <summary>
    ///     Describe the DrawingAttributes (similar to the .NET)
    /// </summary>
    [Serializable]
    public class DrawingAttributes : BaseModel
    {
        private Brushes _brush;
        private Color _color;
        private double _height;
        private bool _isHighlighter;
        private double _width;

        public DrawingAttributes()
        {
            Color = new Color();
            IsHighlighter = false;
            Height = 2;
            Width = 2;
            Brush = Brushes.Ellipse;
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public bool IsHighlighter
        {
            get { return _isHighlighter; }
            set { _isHighlighter = value; }
        }

        public double Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public double Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Brushes Brush
        {
            get { return _brush; }
            set { _brush = value; }
        }
    }
}