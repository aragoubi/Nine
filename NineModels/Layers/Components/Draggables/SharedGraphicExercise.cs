using System;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.Lessons.Holders;

namespace Nine.Sharing
{
    [Serializable]
    public class SharedGraphicExercise : SharedExercise
    {
        public SharedGraphicExercise(BasicLayer layer, int uid, string exerciseName, string userName, int width,
            int height)
            : base(layer, uid, exerciseName, userName)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public override void AddIt(BasicHolder holder)
        {
            var factory = new ExerciseContent.ExerciseFactory(holder as ExerciseHolder);
            var content = factory.GetGraphical(ExerciseName /*, Width, Height*/);
            content.ImportLayer(Layer);
            content.AddLayer("Votre réponse");
            content.UID = UID;

            holder.Contents.Add(content);
        }
    }
}