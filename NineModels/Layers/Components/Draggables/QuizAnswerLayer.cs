using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nine.Layers.Components;
using Nine.Layers.Components.Charts;
using Nine.Lessons.Contents;

namespace Nine.Layers
{
    /// <summary>
    /// The QuizAnswerLayer handle the analysis of a Quiz
    /// </summary>
    [Serializable]
    public class QuizAnswerLayer : QuizLayer, IAnswerLayer
    {
        public QuizAnswerLayer(QuizContent content, string name, Collection<KeyValuePair<int, int>> answers,
            bool isRenameable = false, bool isHideable = true, bool isDeletable = false, bool isShareable = false,
            bool isInkable = true)
            : base(content, name, isRenameable, isHideable, isDeletable, isShareable, isInkable)
        {
            BarChart = new BarChart(new Point(0, 0), answers);
            NbParticipants = 0;
        }

        public BarChart BarChart { get; internal set; }
        public int NbParticipants { get; set; }
    }
}