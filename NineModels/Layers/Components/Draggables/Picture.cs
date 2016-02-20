using System;
using Nine.Layers.Components.Links;

namespace Nine.Layers.Components.Draggables
{
    /// <summary>
    ///     Special anchor who is represented by it's content
    /// </summary>
    [Serializable]
    public class Picture : Anchor
    {
        public Picture(Point position, ILink link)
            : base(position, link)
        {
        }
    }
}