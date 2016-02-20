using System;
using System.Collections.Generic;
using Nine.Layers;
using Nine.Lessons.Holders;

namespace Nine.Lessons.Contents
{
    [Serializable]
    public class BasicContent : Content
    {
        public BasicContent(BasicHolder holder, string name)
            : base(holder, name)
        {
        }

        public override int AddLayer(string name = "", bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
        {
            if (name == "")
                name = GetFallbackName();
            var layer = new BasicLayer(this, name, isRenameable, isHideable, isDeletable, isShareable, isInkable);
            Layers.Add(layer);
            return layer.UID;
        }

        public override void RenameLayer(int uid, string name)
        {
            foreach (var layer in Layers)
                if (layer.UID == uid)
                {
                    (layer as BasicLayer).Name = name;
                    return;
                }
            throw new KeyNotFoundException("Layer not found.");
        }

        public override void DeleteLayer(int uid)
        {
            Layers.Remove(GetLayerByUid(uid));
        }

        public override void ImportLayer(AbstractLayer layer)
        {
            layer.UID = Guid.NewGuid().GetHashCode();
            Layers.Add(layer);
        }
    }
}