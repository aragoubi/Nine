using System;
using Nine.Layers.Components.Links;

namespace Nine.Layers.Components.Draggables
{
    /// <summary>
    ///     Define a anchor who is a link to something (layer, url, fs...)
    ///     Represented by an icon
    /// </summary>
    [Serializable]
    public abstract class Anchor : DraggableComponent
    {
        private ILink _link;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Anchor" /> class.
        ///     An Anchor is constituted of a Point and an ILink instance.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="link">The link.</param>
        protected Anchor(Point position, ILink link) : base(position)
        {
            Link = link;
        }

        public ILink Link
        {
            get { return _link; }
            private set { _link = value; }
        }
    }
}