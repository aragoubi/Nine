using System;
using System.Collections.Generic;
using Nine.Layers;
using Nine.Lessons.Holders;

namespace Nine.Sharing
{
    /// <summary>
    ///     This class provide an encapsulation to share one layer (at User-POV).
    /// </summary>
    [Serializable]
    public class SharedParallelLayer : SharedLayer
    {
        private List<ParallelLayer> _layers;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SharedParallelLayer" /> class.
        /// </summary>
        /// <param name="holder">The holder.</param>
        /// <param name="uid">The uid of the Layer.</param>
        /// <param name="userName">Name of the user sending the layer.</param>
        /// <param name="isTeacher">if set to <c>true</c> [is teacher].</param>
        public SharedParallelLayer(ParallelHolder holder, int uid, string userName, bool isTeacher)
            : base(uid, userName, isTeacher)
        {
            Name = holder.LayerNames[uid];
            foreach (var content in holder.Contents)
                Layers.Add(content.GetLayerByUid(uid) as ParallelLayer);
        }

        public List<ParallelLayer> Layers
        {
            get
            {
                if (_layers == null)
                    Layers = new List<ParallelLayer>();
                return _layers;
            }
            private set { _layers = value; }
        }

        /// <summary>
        ///     Adds the received layers to the specified holder.
        /// </summary>
        /// <param name="holder">The holder.</param>
        public override void AddThem(Holder holder)
        {
            (holder as ParallelHolder).ImportLayers(Name, Layers);
        }
    }
}