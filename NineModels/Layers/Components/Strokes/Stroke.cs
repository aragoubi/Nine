using System;
using System.Collections.Generic;
using Nine.Layers.Components.Strokes.DA;

namespace Nine.Layers.Components.Strokes
{
    /// <summary>
    ///     A Stroke is a set of StrokePoint (2D-Point + Pressure) with drawing context.
    /// </summary>
    [Serializable]
    public class Stroke : BaseModel
    {
        private DrawingAttributes _drawingAttributes;
        private List<StrokePoint> _points;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Stroke" /> class.
        ///     Require a List of Points and the DrawingAttributes. No other method are available.
        /// </summary>
        /// <param name="pts">The PTS.</param>
        /// <param name="drw">The DRW.</param>
        public Stroke(List<StrokePoint> pts, DrawingAttributes drw)
        {
            Points = pts;
            DrawingAttributes = drw;
        }

        /// <summary>
        ///     Gets the StrokePoint of the Stroke.
        /// </summary>
        /// <value>
        ///     The points.
        /// </value>
        public List<StrokePoint> Points
        {
            get { return _points; }
            private set { _points = value; }
        }

        /// <summary>
        ///     Gets or sets the drawing attributes.
        /// </summary>
        /// <value>
        ///     The drawing attributes.
        /// </value>
        public DrawingAttributes DrawingAttributes
        {
            get { return _drawingAttributes; }
            private set { _drawingAttributes = value; }
        }
    }
}