using System;

namespace Nine.Layers.Components.Strokes
{
    /// <summary>
    ///     Represents a point included in a stroke (double-precision 2D point + float-precision pressure).
    /// </summary>
    [Serializable]
    public class StrokePoint : Point
    {
        private float _pressure;

        public StrokePoint(double x, double y, float p = 0) : base(x, y)
        {
            Pressure = p;
        }

        public StrokePoint(Point p, float pressure = 0)
            : this(p.X, p.Y, pressure)
        {
        }

        public float Pressure
        {
            get { return _pressure; }
            set { _pressure = value; }
        }
    }
}