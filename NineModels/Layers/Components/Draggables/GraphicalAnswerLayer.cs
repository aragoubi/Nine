using System;
using System.Collections.ObjectModel;
using Nine.Layers.Components;
using Nine.Layers.Components.Charts;
using Nine.Lessons.Contents;

namespace Nine.Layers
{
    /// <summary>
    /// Describe the layer who know the answer of a graphical exercise.
    /// </summary>
    [Serializable]
    public class GraphicalAnswerLayer : BasicLayer, IAnswerLayer
    {
        public GraphicalAnswerLayer(ExerciseContent content, string name, Collection<Point> points,
            bool isRenameable = false, bool isHideable = true, bool isDeletable = false, bool isShareable = false,
            bool isInkable = true)
            : base(content, name, isRenameable, isHideable, isDeletable, isShareable, isInkable)
        {
            SaliencyMap = new SaliencyMap(points);
            NbParticipants = 0;
        }

        public SaliencyMap SaliencyMap { get; internal set; }
        public int NbParticipants { get; set; }
    }
}