using System;
using System.Collections.ObjectModel;
using Nine.Layers.Components;
using Nine.Layers.Components.Draggables;
using Nine.Lessons.Contents;
using Nine.Lessons.Contents.Processing;

namespace Nine.Layers
{
    /// <summary>
    /// The QuizLayer is a specific BasicLayer with bullets
    /// </summary>
    [Serializable]
    public class QuizLayer : BasicLayer
    {
        private Collection<QuizBullet> _bullets;

        public QuizLayer(BasicContent content, string name, bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
            : base(content, name, isRenameable, isHideable, isDeletable, isShareable, isInkable)
        {
        }

        public Collection<QuizBullet> Bullets
        {
            get
            {
                if (_bullets == null)
                    Bullets = new Collection<QuizBullet>();
                return _bullets;
            }
            private set { _bullets = value; }
        }

        internal void AddBullet(int offset, Point position, QuizMode mode)
        {
            if (mode == QuizMode.QCM)
                Bullets.Add(new QCMBullet(position, offset));
            else
                Bullets.Add(new QCUBullet(position, offset));
        }

        public void RemoveBullet(QuizBullet bullet)
        {
            (Content as QuizContent).RemoveBullet(bullet.Offset);
        }

        internal void RemovingBullet(int offset)
        {
            foreach (var bullet in Bullets)
                if (bullet.Offset == offset)
                {
                    Bullets.Remove(bullet);
                    break;
                }
        }
    }
}