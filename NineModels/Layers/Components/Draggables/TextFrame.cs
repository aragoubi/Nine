using System;

namespace Nine.Layers.Components.Draggables
{
    /// <summary>
    ///     Describe a text positionned into a component (Layer).
    /// </summary>
    [Serializable]
    public class TextFrame : TextualComponent
    {
        public TextFrame(Point position, string text = "", double width = 250, double height = 150)
            : base(position, text)
        {
            Width = width;
            Height = height;
        }

        public double Width { get; set; }
        public double Height { get; set; }
    }
}