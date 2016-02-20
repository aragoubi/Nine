using System;

namespace Nine.Layers.Components.Draggables
{
    /// <summary>
    ///     DraggableComponent who has a Text attribute.
    /// </summary>
    [Serializable]
    public abstract class TextualComponent : DraggableComponent
    {
        public TextualComponent(Point position, string text = "") : base(position)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}