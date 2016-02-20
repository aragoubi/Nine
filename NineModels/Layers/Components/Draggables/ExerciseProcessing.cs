using System;
using System.Collections.Generic;
using Nine.Layers;

namespace Nine.Lessons.Contents.Processing
{
    /// <summary>
    ///     Define the frame to describe processing about exercises.
    /// </summary>
    [Serializable]
    public abstract class ExerciseProcessing
    {
        public abstract void CollectLayer(BasicLayer layer);

        public void CollectLayers(IEnumerable<BasicLayer> layers)
        {
            foreach (var layer in layers)
                CollectLayer(layer);
        }

        public abstract void Process();
        public abstract object GetResults();
    }
}