using System;
using Nine.Layers;
using Nine.Lessons.Contents;
using Nine.Lessons.Holders;

namespace Nine.Sharing
{
    [Serializable]
    public class SharedExerciseLayer : SharedBasicLayer
    {
        public SharedExerciseLayer(ExerciseContent content, int uid, string userName, bool isTeacher)
            : base(content, uid, userName, isTeacher)
        {
            // Nom of the exercise
            Name = content.Name;
            Layer = content.GetLayerByUid(uid) as BasicLayer;
        }

        /// <summary>
        ///     Adds the received layers to the specified holder.
        /// </summary>
        /// <param name="holder">The holder.</param>
        public override void AddThem(Holder holder)
        {
            ExerciseContent content = null;
            try
            {
                content = (holder as ExerciseHolder).GetContentByUID(UID);
                content.ImportLayer(Layer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}