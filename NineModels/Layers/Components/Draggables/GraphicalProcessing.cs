using System;
using System.Collections.ObjectModel;
using Nine.Layers;
using Nine.Layers.Components;

namespace Nine.Lessons.Contents.Processing
{
    /// <summary>
    ///     Define how to process a graphical exercise.
    /// </summary>
    [Serializable]
    public class GraphicalProcessing : ExerciseProcessing
    {
        private readonly Collection<Point> points;

        public GraphicalProcessing()
        {
            points = new Collection<Point>();
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public override void CollectLayer(BasicLayer layer)
        {
            foreach (var stroke in layer.Strokes)
                foreach (var point in stroke.Points)
                    points.Add(point);
        }

        public override void Process()
        {
        }

        public override object GetResults()
        {
            return points;
        }
    }
}