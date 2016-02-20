using System;
using Nine.Lessons.Contents;

namespace Nine.Lessons.Holders
{
    /// <summary>
    ///     SlideHolder allows to encapsulate SlideContent.
    /// </summary>
    [Serializable]
    public class SlideHolder : ParallelHolder
    {
        public override void AddNewContent(string name = "")
        {
            Contents.Add(new SlideContent(this));
        }
    }
}