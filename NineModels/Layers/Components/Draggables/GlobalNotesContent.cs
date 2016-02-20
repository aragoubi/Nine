using System;
using Nine.Lessons.Holders;

namespace Nine.Lessons.Contents
{
    /// <summary>
    ///     A GlobalNotesContent is a Content with a constructor that force a NamedContentHolder (BasicHolder is not enough).
    /// </summary>
    [Serializable]
    public class GlobalNotesContent : ParallelContent
    {
        public GlobalNotesContent(ParallelHolder holder, string name)
            : base(holder, name)
        {
        }
    }
}