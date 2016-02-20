using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nine.Layers;

namespace Nine.Lessons.Contents.Processing
{
    /// <summary>
    ///     Define how to process a Quiz.
    /// </summary>
    [Serializable]
    public class QuizProcessing : ExerciseProcessing
    {
        private readonly Dictionary<int, int> result;
        private QuizMode _mode;

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuizProcessing" /> class.
        ///     Default QuizProcessing is a Checkbox Quiz
        /// </summary>
        public QuizProcessing() : this(QuizMode.QCM)
        {
            result = new Dictionary<int, int>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="QuizProcessing" /> class.
        /// </summary>
        /// <param name="mode">The mode.</param>
        public QuizProcessing(QuizMode mode)
        {
            Mode = mode;
            result = new Dictionary<int, int>();
        }

        /// <summary>
        ///     Gets the mode.
        /// </summary>
        /// <value>
        ///     The mode.
        /// </value>
        public QuizMode Mode
        {
            get { return _mode; }
            private set { _mode = value; }
        }

        public override void CollectLayer(BasicLayer layer)
        {
            var goodLayer = (QuizLayer) layer;
            foreach (var bullet in goodLayer.Bullets)
            {
                if (result.ContainsKey(bullet.Offset))
                {
                    result[bullet.Offset] += (bullet.Checked ? 1 : 0);
                }
                else
                {
                    result.Add(bullet.Offset, (bullet.Checked ? 1 : 0));
                }
            }
        }

        public override void Process()
        {
        }

        public override object GetResults()
        {
            var collection = new Collection<KeyValuePair<int, int>>();
            foreach (var p in result)
                collection.Add(new KeyValuePair<int, int>(p.Key, p.Value));

            return collection;
        }
    }
}