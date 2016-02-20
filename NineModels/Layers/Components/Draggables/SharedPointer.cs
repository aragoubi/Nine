using System;
using Nine.Layers.Components.Strokes;

namespace Nine.Sharing
{
    /// <summary>
    ///     SharedPointer is a dedicated class to send a stroke in pointer mode with full context
    /// </summary>
    [Serializable]
    public class SharedPointer : BaseModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SharedPointer" /> class.
        /// </summary>
        /// <param name="isSlide">if set to <c>true</c> [is slide].</param>
        /// <param name="offset">The offset.</param>
        /// <param name="stroke">The stroke.</param>
        public SharedPointer(int offset, Stroke stroke)
        {
            Offset = offset;
            Stroke = stroke;
        }

        /// <summary>
        ///     Gets the offset of the content into the Holder (slide or exercise)
        /// </summary>
        /// <value>
        ///     The offset.
        /// </value>
        public int Offset { get; private set; }

        /// <summary>
        ///     Gets the stroke to draw.
        /// </summary>
        /// <value>
        ///     The stroke.
        /// </value>
        public Stroke Stroke { get; private set; }
    }
}