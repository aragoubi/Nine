using System;
using Nine.Lessons.Contents;

namespace Nine.Layers
{
    /// <summary>
    ///     A BasicLayer is how all layer in Exercises works.
    /// </summary>
    [Serializable]
    public class BasicLayer : AbstractLayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelLayer" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="name">The name.</param>
        /// <param name="isRenameable">if set to <c>true</c> [is renameable].</param>
        /// <param name="isHideable">if set to <c>true</c> [is hideable].</param>
        /// <param name="isDeletable">if set to <c>true</c> [is deletable].</param>
        /// <param name="isShareable">if set to <c>true</c> [is shareable].</param>
        /// <param name="isInkable">if set to <c>true</c> [is inkable].</param>
        public BasicLayer(BasicContent content, string name, bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
            : base(content, Guid.NewGuid().GetHashCode(), isRenameable, isHideable, isDeletable, isShareable, isInkable)
        {
            Name = name;
        }
    }
}