using System;
using Nine.Lessons.Contents;

namespace Nine.Lessons.Holders
{
    /// <summary>
    ///     GlobalNotesHolder allows to encapsulate GlobalNotesContent.
    /// </summary>
    [Serializable]
    public class GlobalNotesHolder : ParallelHolder
    {
        public override void AddNewContent(string name)
        {
            Contents.Add(new GlobalNotesContent(this, name));
        }
    }
}