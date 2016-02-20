using System;

namespace Nine.Layers.Components.Draggables
{
    /// <summary>
    ///     A draggable component is the most generic component who can be added to a Layer
    ///     It can be drawable or not and must be positionnable (Position property)
    /// </summary>
    [Serializable]
    public abstract class DraggableComponent : BaseComponent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DraggableComponent" /> class.
        ///     Must be positionned to be instanciated.
        /// </summary>
        /// <param name="position">The original position.</param>
        protected DraggableComponent(Point position)
        {
            Position = position;
        }

        /// <summary>
        ///     The position of the component, where the top right corner should be fixed
        /// </summary>
        /// <value>
        ///     The position.
        /// </value>
        public Point Position { get; set; }
    }
}