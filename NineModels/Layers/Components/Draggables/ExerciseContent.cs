using System;
using System.Collections.ObjectModel;
using Nine.Layers;
using Nine.Layers.Components;
using Nine.Lessons.Contents.Processing;
using Nine.Lessons.Holders;

namespace Nine.Lessons.Contents
{
    /// <summary>
    ///     Describe the components of an Exercise
    /// </summary>
    [Serializable]
    public class ExerciseContent : BasicContent
    {
        public const int QuestionLayerIndex = 0;
        public const int AnswerLayerIndex = 1;
        private ExerciseProcessing _processing;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExerciseContent" /> class.
        ///     Default access to create a ExerciseContent (we don't break the object-related inheritance).
        ///     But, exercises should only be created by their Factory!
        /// </summary>
        /// <param name="holder">The holder.</param>
        /// <param name="name">The name.</param>
        public ExerciseContent(BasicHolder holder, string name) : this(holder, name, new QuizProcessing())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExerciseContent" /> class.
        /// </summary>
        /// <param name="holder">The holder.</param>
        /// <param name="name">The name.</param>
        /// <param name="processing">The processing.</param>
        protected ExerciseContent(BasicHolder holder, string name, ExerciseProcessing processing)
            : base(holder, name)
        {
            Processing = processing;
            Answered = false;
            Accept = false;
            UID = Guid.NewGuid().GetHashCode();
            HasBeenCollected = false;
        }

        /// <summary>
        ///     UID allows teacher and students to be undesrstood
        /// </summary>
        /// <value>
        /// </value>
        public int UID { get; internal set; }

        /// <summary>
        ///     For student, permits to change the view if he has already sent this exercise
        /// </summary>
        /// <value>
        ///     <c>true</c> if answered; otherwise, <c>false</c>.
        /// </value>
        public bool Answered { get; set; }

        /// <summary>
        ///     For teacher, permit to say if we want to accept answers from students
        /// </summary>
        /// <value>
        ///     <c>true</c> if answered; otherwise, <c>false</c>.
        /// </value>
        public bool Accept { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this instance has been collected.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has been collected; otherwise, <c>false</c>.
        /// </value>
        public bool HasBeenCollected { get; set; }

        /// <summary>
        ///     Gets the processing.
        /// </summary>
        /// <value>
        ///     The processing.
        /// </value>
        public ExerciseProcessing Processing
        {
            get { return _processing; }
            private set { _processing = value; }
        }

        public IAnswerLayer AnswerLayer { get; protected set; }
        public IQuestionLayer QuestionLayer { get; protected set; }

        public string Kind
        {
            get
            {
                var procName = Processing.GetType().Name;
                if (procName == "GraphicalProcessing")
                    return "Graphic";
                if (procName == "UnrestrictedProcessing")
                    return "Free";
                return "Quiz";
            }
        }

        /// <summary>
        ///     Adds the answer layer, default is graphical answer layer
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public virtual int AddAnswerLayer(string name, bool isRenameable = false, bool isHideable = true,
            bool isDeletable = false, bool isShareable = false, bool isInkable = true)
        {
            var layer = new GraphicalAnswerLayer(this, name,
                (Processing as GraphicalProcessing).GetResults() as Collection<Point>, isRenameable, isHideable,
                isDeletable, isShareable, isInkable);
            Layers.Add(layer);
            AnswerLayer = layer;
            return layer.UID;
        }

        /// <summary>
        ///     Adds the question layer, default is graphical question layer
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="src">The source.</param>
        /// <returns></returns>
        public virtual int AddQuestionLayer(string name, bool isRenameable = false, bool isHideable = true,
            bool isDeletable = false, bool isShareable = false, bool isInkable = true)
        {
            var layer = new GraphicalQuestionLayer(this, name, isRenameable, isHideable, isDeletable, isShareable,
                isInkable);
            Layers.Add(layer);
            QuestionLayer = layer;
            return layer.UID;
        }

        public virtual void ComputeResults()
        {
            Processing.Process();
            ((GraphicalAnswerLayer) AnswerLayer).SaliencyMap.Points = (Collection<Point>) Processing.GetResults();
        }

        /// <summary>
        ///     The Factory should be the only way to create a new Exercise (auto binding of the Processing).
        /// </summary>
        [Serializable]
        public class ExerciseFactory
        {
            private readonly ExerciseHolder _holder;

            public ExerciseFactory(ExerciseHolder holder)
            {
                _holder = holder;
            }

            public ExerciseContent GetGraphical(string name)
            {
                return new ExerciseContent(_holder, name, new GraphicalProcessing());
            }

            public QuizContent GetQCM(string name)
            {
                return new QuizContent(_holder, name, new QuizProcessing(QuizMode.QCM), QuizMode.QCM);
            }

            public QuizContent GetQCU(string name)
            {
                return new QuizContent(_holder, name, new QuizProcessing(QuizMode.QCU), QuizMode.QCU);
            }

            public ExerciseContent GetUnrestricted(string name)
            {
                return new ExerciseContent(_holder, name, new UnrestrictedProcessing());
            }
        }
    }
}