using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nine.Layers.Components.Draggables;

namespace Nine.Layers.Components.Charts
{
    [Serializable]
    public class BarChart : DraggableComponent
    {
        public BarChart(Point position, Collection<KeyValuePair<int, int>> answers)
            : base(position)
        {
            Answers = answers;
        }

        public Collection<KeyValuePair<int, int>> Answers { get; internal set; }
    }
}