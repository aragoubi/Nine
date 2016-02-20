using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nine.Layers;
using Nine.Layers.Components;
using Nine.Lessons.Contents.Processing;
using Nine.Lessons.Holders;

namespace Nine.Lessons.Contents
{
    [Serializable]
    public class QuizContent : ExerciseContent
    {
        private List<int> _bullets;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExerciseContent" /> class.
        /// </summary>
        /// <param name="holder">The holder.</param>
        /// <param name="name">The name.</param>
        /// <param name="processing">The processing.</param>
        internal QuizContent(BasicHolder holder, string name, ExerciseProcessing processing, QuizMode mode)
            : base(holder, name, processing)
        {
            Mode = mode;
        }

        /// <summary>
        ///     Define the mode of the quiz : QCM or QCU
        /// </summary>
        /// <value>
        ///     The mode.
        /// </value>
        public QuizMode Mode { get; private set; }

        /// <summary>
        ///     Gets the existing bullets.
        /// </summary>
        /// <value>
        ///     The bullets.
        /// </value>
        public List<int> Bullets
        {
            get
            {
                if (_bullets == null)
                    Bullets = new List<int>();
                return _bullets;
            }
            internal set { _bullets = value; }
        }

        public void AddBullet(int offset, Point position)
        {
            Bullets.Add(offset);
            foreach (QuizLayer layer in Layers)
                layer.AddBullet(offset, position, Mode);
        }

        public override int AddLayer(string name = "", bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
        {
            if (name == "")
                name = GetFallbackName();
            var layer = new QuizLayer(this, name, isRenameable, isHideable, isDeletable, isShareable, isInkable);
            Layers.Add(layer);

            foreach (var bullet in (Layers[0] as QuizLayer).Bullets)
                layer.AddBullet(bullet.Offset, bullet.Position, Mode);

            return layer.UID;
        }

        public override int AddAnswerLayer(string name, bool isRenameable = false, bool isHideable = true,
            bool isDeletable = false, bool isShareable = false, bool isInkable = true)
        {
            var layer = new QuizAnswerLayer(this, name,
                (Processing as QuizProcessing).GetResults() as Collection<KeyValuePair<int, int>>, isRenameable,
                isHideable, isDeletable, isShareable, isInkable);
            Layers.Add(layer);
            AnswerLayer = layer;
            return layer.UID;
        }

        public override int AddQuestionLayer(string name, bool isRenameable = false, bool isHideable = true,
            bool isDeletable = false, bool isShareable = false, bool isInkable = true)
        {
            var layer = new QuizQuestionLayer(this, name, isRenameable, isHideable, isDeletable, isShareable, isInkable);
            Layers.Add(layer);
            QuestionLayer = layer;
            return layer.UID;
        }

        public override void ComputeResults()
        {
            Processing.Process();
            var barChart = ((QuizAnswerLayer) AnswerLayer).BarChart;
            barChart.Answers = (Collection<KeyValuePair<int, int>>) Processing.GetResults();
        }

        public void RemoveBullet(int offset)
        {
            Bullets.Remove(offset);
            foreach (QuizLayer layer in Layers)
                layer.RemovingBullet(offset);
        }
    }
}