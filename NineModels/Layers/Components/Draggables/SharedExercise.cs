using System;
using Nine.Layers;
using Nine.Lessons.Holders;

namespace Nine.Sharing
{
    /// <summary>
    ///     SharedExercise allows the teacher to send a BasicLayer to be answered by students
    /// </summary>
    [Serializable]
    public abstract class SharedExercise
    {
        protected SharedExercise(BasicLayer layer, int uid, string exerciseName, string userName)
        {
            UID = uid;
            Layer = layer;
            ExerciseName = exerciseName;
            UserName = userName;
        }

        public BasicLayer Layer { get; private set; }
        public string ExerciseName { get; private set; }
        public string UserName { get; private set; }
        public int UID { get; set; }
        public abstract void AddIt(BasicHolder holder);
    }
}