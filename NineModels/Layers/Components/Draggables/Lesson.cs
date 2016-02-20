using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nine.Lessons.Contents;
using Nine.Lessons.Holders;

namespace Nine.Lessons
{
    /// <summary>
    ///     Lesson is the main ressource of the application.
    ///     It holds every peaces of the current lesson and could be saved as it (by serialisation).
    /// </summary>
    [Serializable]
    public class Lesson : BaseModel
    {
        private ExerciseHolder _exercises;
        private GlobalNotesHolder _globalNotes;
        private string _name;
        private SlideHolder _slides;
        private Collection<Tag> _tags;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lesson" /> class.
        ///     To be properly instanciated, the lesson need a name, the path of the PDF and the current number of pages of this
        ///     document.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="nbPages">The nb pages.</param>
        public Lesson(string name, int nbPages)
        {
            Name = name;

            // Preparation of the slides
            Slides = new SlideHolder();
            for (var i = 0; i < nbPages; i++)
                Slides.AddNewContent();

            Slides.AddLayer("Mes notes");

            // Preparation of the global notes (free note zone)
            GlobalNotes = new GlobalNotesHolder();
            GlobalNotes.AddNewContent("Default");
            GlobalNotes.AddLayer("Notes libres");

            // Preparation of the exercice holder
            Exercises = new ExerciseHolder();

            // Test, adding one quiz layer
            var exFactory = new ExerciseContent.ExerciseFactory(Exercises);
            ExerciseContent ex = exFactory.GetQCM("QCM Démonstration");
            (ex as QuizContent).AddQuestionLayer("Question");
            (ex as QuizContent).AddAnswerLayer("Réponse");
            Exercises.Contents.Add(ex);
        }

        /// <summary>
        ///     Gets or sets the name of that lesson.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     Gets the referenced tags.
        /// </summary>
        /// <value>
        ///     The tags.
        /// </value>
        public Collection<Tag> Tags
        {
            get
            {
                if (_tags == null)
                {
                    Tags = Tag.TagFactory.GetTags();
                }
                return _tags;
            }
            private set { _tags = value; }
        }

        /// <summary>
        ///     Gets the slides, passing through the dedicated Holder.
        /// </summary>
        /// <value>
        ///     The slides.
        /// </value>
        public SlideHolder Slides
        {
            get { return _slides; }
            private set { _slides = value; }
        }

        /// <summary>
        ///     Gets the global notes, passing through the dedicated Holder.
        /// </summary>
        /// <value>
        ///     The global notes.
        /// </value>
        public GlobalNotesHolder GlobalNotes
        {
            get { return _globalNotes; }
            private set { _globalNotes = value; }
        }

        /// <summary>
        ///     Gets the exercises (all kind), passing through the dedicated Holder
        /// </summary>
        /// <value>
        ///     The exercises.
        /// </value>
        public ExerciseHolder Exercises
        {
            get { return _exercises; }
            private set { _exercises = value; }
        }

        /// <summary>
        ///     Gets the tag by passing it's name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Tag not found</exception>
        public Tag GetTag(string name)
        {
            foreach (var tag in Tags)
                if (tag.Name == name)
                    return tag;

            throw new KeyNotFoundException("Tag not found");
        }
    }
}