using System;
using System.Linq;
using Nine.Layers;
using Nine.Lessons.Holders;

namespace Nine.Lessons.Contents
{
    /// <summary>
    ///     The Content class describe the minimal objects that a Content should contain.
    ///     A Content doesn't have a name because a Slide mustn't be nammed, it's called by the page offset.
    /// </summary>
    [Serializable]
    public abstract class ParallelContent : Content
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ParallelContent" /> class.
        ///     Require an Holder (his instanciator)
        /// </summary>
        /// <param name="holder">The holder.</param>
        protected ParallelContent(ParallelHolder holder, string name)
            : base(holder, name)
        {
            // Create the already existing layers to keep integrity
            foreach (var layer in holder.LayerNames)
            {
                var layerModel = Layers[layer.Key];
                LayerCreation(layer.Key, layerModel.IsRenameable, layerModel.IsHideable, layerModel.IsDeletable,
                    layerModel.IsShareable, layerModel.IsInkable);
            }
        }

        public override int AddLayer(string name, bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
        {
            return (Holder as ParallelHolder).AddLayer(name, isRenameable, isHideable, isDeletable, isShareable,
                isInkable);
        }

        public override void RenameLayer(int uid, string name)
        {
            (Holder as ParallelHolder).RenameLayer(uid, name);
        }

        public override void DeleteLayer(int uid)
        {
            (Holder as ParallelHolder).DeleteLayer(uid);
        }

        /// <summary>
        ///     Create a Layer with the right UID and Content reference.
        ///     Internal, because they only should be called by the associated holder.
        /// </summary>
        /// <param name="uid">The uid.</param>
        internal virtual void LayerCreation(int uid = -1, bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
        {
            if (uid == -1)
                throw new ArgumentException("UID should be properly generated", "uid");

            Layers.Add(new ParallelLayer(this, uid, isRenameable, isHideable, isDeletable, isShareable, isInkable));
        }

        /// <summary>
        ///     Imports an existing layer and adjust the right UID
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="uid">The uid.</param>
        internal void LayerImportation(ParallelLayer layer, int uid)
        {
            layer.UID = uid;
            layer.Content = this;
            Layers.Add(layer);
        }

        /// <summary>
        ///     Delete a layer by the UID.
        ///     Internal, because they only should be called by the associated holder.
        /// </summary>
        /// <param name="uid">The uid.</param>
        internal void LayerDeletion(int uid)
        {
            Layers.Remove(Layers.First(layer => layer.UID == uid));
        }

        public override void ImportLayer(AbstractLayer layer)
        {
            throw new NotImplementedException();
        }
    }
}