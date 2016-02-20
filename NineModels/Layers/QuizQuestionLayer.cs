using System;
using Nine.Lessons.Contents;

namespace Nine.Layers
{
    /// <summary>
    /// The QuizQuestionLayer contains a Quiz
    /// </summary>
    [Serializable]
    public class QuizQuestionLayer : QuizLayer, IQuestionLayer
    {
        public QuizQuestionLayer(BasicContent content, string name, bool isRenameable = false, bool isHideable = true,
            bool isDeletable = false, bool isShareable = false, bool isInkable = true)
            : base(content, name, isRenameable, isHideable, isDeletable, isShareable, isInkable)
        {
        }
    }
}