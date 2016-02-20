using System;
using Nine.Lessons.Holders;

namespace Nine.Lessons.Contents
{
    /// <summary>
    ///     A SlideContent is a Content with a constructor that force a SlideHolder (BasicHolder is not enough).
    /// </summary>
    [Serializable]
    public class SlideContent : ParallelContent
    {
        public SlideContent(SlideHolder holder, string name = "") : base(holder, name)
        {
        }
    }
}