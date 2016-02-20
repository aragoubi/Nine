using System;
using System.Collections.Generic;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.Lessons.Contents.Processing;
using Nine.Lessons.Holders;

namespace Nine.Sharing
{
    [Serializable]
    public class SharedQuizExercise : SharedExercise
    {
        public SharedQuizExercise(QuizLayer layer, int uid, List<int> bullets, QuizMode mode, string exerciseName,
            string userName)
            : base(layer, uid, exerciseName, userName)
        {
            Bullets = bullets;
            Mode = mode;
        }

        public List<int> Bullets { get; private set; }
        public QuizMode Mode { get; private set; }

        public override void AddIt(BasicHolder holder)
        {
            var factory = new ExerciseContent.ExerciseFactory(holder as ExerciseHolder);
            QuizContent content;
            if (Mode == QuizMode.QCM)
                content = factory.GetQCM(ExerciseName);
            else
                content = factory.GetQCU(ExerciseName);

            content.ImportLayer(Layer);
            content.Bullets = Bullets;
            content.UID = UID;

            holder.Contents.Add(content);
        }
    }
}