using System;
using Nine.Lessons.Contents;
using Nine.Lessons.Holders;

namespace Nine.Layers
{
    /// <summary>
    ///     Class Layer that has to be linked to a Lessons.Contents instance
    ///     Be careful about the distinction between a layer at the user point-of-view and this Layer which represents only ONE
    ///     page
    ///     At the user POV, a layer is the same thing on every page of the lesson,
    ///     BUT, at the dev POV, an user-layer is reprensented by several Layer instance, one for each page
    /// </summary>
    [Serializable]
    public class ParallelLayer : AbstractLayer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParallelLayer" /> class.
        /// </summary>
        /// <param name="lc">The Content</param>
        /// <param name="id">The unique identifier to be capable to bind a user-level layer with our Layer implementation</param>
        public ParallelLayer(ParallelContent content, int uid, bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
            : base(content, uid, isRenameable, isHideable, isDeletable, isShareable, isInkable)
        {
        }

        /// <summary>
        ///     Tell if this *Content should be displayed
        /// </summary>
        public override bool IsDisplayed
        {
            get { return (Content.Holder as ParallelHolder).LayerVisibility[UID]; }
            set { (Content.Holder as ParallelHolder).LayerVisibility[UID] = value; }
        }

        /// <summary>
        ///     Layers doesn't know their name, only Lesson can do that kind of association
        ///     See uid/class summary to understand how it's working
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public override string Name
        {
            get { return (Content.Holder as ParallelHolder).LayerNames[UID]; }
            internal set { throw new NotImplementedException(); }
        }
    }
}