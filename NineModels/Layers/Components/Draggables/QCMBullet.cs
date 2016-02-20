using System;

namespace Nine.Layers.Components.Draggables
{
    [Serializable]
    public class QCMBullet : QuizBullet
    {
        public QCMBullet(Point position, int offset) : base(position, offset)
        {
        }
    }
}