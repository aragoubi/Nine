using System;

namespace Nine.Layers.Components.Draggables
{
    /// <summary>
    ///     A bullet to be represented on a ExerciseContent
    ///     We DON'T make the difference with a checkbox/radio at this point
    ///     It's made by the context (on Lessons.Contents.Exercices)
    /// </summary>
    [Serializable]
    public class QuizBullet : DraggableComponent
    {
        public QuizBullet(Point position, int offset) : base(position)
        {
            Checked = false;
            Offset = offset;
        }

        public int Offset { get; private set; }
        public bool Checked { get; set; }
    }
}