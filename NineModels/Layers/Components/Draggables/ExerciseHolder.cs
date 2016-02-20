using System;
using System.Collections.Generic;
using Nine.Lessons.Contents;

namespace Nine.Lessons.Holders
{
    /// <summary>
    ///     ExerciseHolder allows to encapsulate ExerciseContent.
    /// </summary>
    [Serializable]
    public class ExerciseHolder : BasicHolder
    {
        private ExerciseContent.ExerciseFactory _exerciceFactory;

        public ExerciseContent.ExerciseFactory Factory
        {
            get
            {
                if (_exerciceFactory == null)
                    Factory = new ExerciseContent.ExerciseFactory(this);
                return _exerciceFactory;
            }
            private set { _exerciceFactory = value; }
        }

        public override void AddNewContent(string name = "")
        {
            // If unnamed, we name it
            if (name == "")
                name = "Nouvel exercice";

            Contents.Add(Factory.GetQCM(name));
        }

        public int GetPositionOfUID(int uid)
        {
            var position = 0;
            foreach (ExerciseContent content in Contents)
            {
                if (content.UID == uid)
                    return position;
                position++;
            }

            throw new KeyNotFoundException("UID not found (content position)");
        }

        public ExerciseContent GetContentByUID(int uid)
        {
            foreach (ExerciseContent content in Contents)
                if (content.UID == uid)
                    return content;

            throw new KeyNotFoundException("Content not found by UID");
        }
    }
}