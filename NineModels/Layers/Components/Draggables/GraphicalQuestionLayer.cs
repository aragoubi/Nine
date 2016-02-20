using System;
using Nine.Lessons.Contents;

namespace Nine.Layers
{
    /// <summary>
    /// Describe the layer who know the QUESTION of a graphical exercise.
    /// </summary>
    [Serializable]
    public class GraphicalQuestionLayer : BasicLayer, IQuestionLayer
    {
        public GraphicalQuestionLayer(BasicContent content, string name, bool isRenameable = false,
            bool isHideable = true, bool isDeletable = false, bool isShareable = false, bool isInkable = true)
            : base(content, name, isRenameable, isHideable, isDeletable, isShareable, isInkable)
        {
        }
    }
}