using System;
using System.Collections.ObjectModel;

namespace Nine.Layers.Components.Charts
{
    [Serializable]
    public class SaliencyMap : BaseComponent
    {
        public SaliencyMap(Collection<Point> points)
        {
            Points = points;
        }

        public Collection<Point> Points { get; internal set; }
    }
}