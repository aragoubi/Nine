using System;

namespace Nine.Layers.Components
{
    /// <summary>
    ///     Describe a double-precision 2D point.
    /// </summary>
    [Serializable]
    public class Point : BaseModel
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }
    }
}