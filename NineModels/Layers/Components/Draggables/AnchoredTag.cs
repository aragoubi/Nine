using System;
using Nine.Layers.Components.Links;
using Nine.Lessons;

namespace Nine.Layers.Components.Draggables
{
    /// <summary>
    ///     Permit to put a tag (the representation) on a Layer.
    ///     Contains the reference to the Tag (Lesson-bind).
    /// </summary>
    [Serializable]
    public class AnchoredTag : Anchor
    {
        private Tag _tag;

        public AnchoredTag(Point point, LayerLink link, Tag tag) : base(point, link)
        {
            Tag = tag;
            Tag.AddLayer(link.Layer);
        }

        public Tag Tag
        {
            get { return _tag; }
            private set { _tag = value; }
        }

        /// <summary>
        ///     Unsets this anchored tag properly
        /// </summary>
        public void Unset()
        {
            Tag.RemoveMe(this);
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="AnchoredTag" /> class.
        ///     When we delete an AnchoredTag, we need to inform the Tag (Lesson-bind).
        /// </summary>
        ~AnchoredTag()
        {
            Tag.RemoveMe(this);
            var link = (LayerLink) Link;
            if (!link.IsDead())
                Tag.RemoveLayer(link.Layer);
        }
    }
}