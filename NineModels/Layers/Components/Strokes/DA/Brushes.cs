using System;

namespace Nine.Layers.Components.Strokes.DA
{
    /// <summary>
    ///     Represents the two existing brushes in the application.
    /// </summary>
    [Serializable]
    public sealed class Brushes : BaseModel
    {
        public static readonly Brushes Ellipse = new Brushes("Ellipse");
        public static readonly Brushes Rectangle = new Brushes("Rectangle");
        public readonly string Name;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Brushes" /> class.
        ///     Constructor is private: values are defined within this class only!
        /// </summary>
        /// <param name="name">The name.</param>
        private Brushes(string name)
        {
            Name = name;
        }
    }
}