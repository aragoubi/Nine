using System;

namespace Nine.Sharing
{
    /// <summary>
    ///     This class provide an encapsulation to share one lesson (at User-POV).
    /// </summary>
    [Serializable]
    public class SharedLesson
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SharedLesson" /> class.
        /// </summary>
        /// <param name="name">Lesson's name.</param>
        /// <param name="pdf">Lesson's PDF.</param>
        public SharedLesson(string name, byte[] pdf) : this(name, pdf, "")
        {
        }

        public SharedLesson(string lessonName, byte[] pdf, string ownerName)
        {
            // TODO: Complete member initialization
            Name = lessonName;
            PDF = pdf;
            OwnerName = ownerName;
        }

        public string Name { get; private set; }
        public byte[] PDF { get; private set; }
        public string OwnerName { get; private set; }
    }
}