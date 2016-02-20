using System;
using Nine.Layers;

namespace Nine.Sharing
{
    [Serializable]
    public class AnswerLayer<TLayer> where TLayer : BasicLayer
    {
        public AnswerLayer(int exerciseUID, TLayer layer)
        {
            UID = exerciseUID;
            Layer = layer;
        }

        public int UID { get; private set; }
        public TLayer Layer { get; private set; }
    }
}