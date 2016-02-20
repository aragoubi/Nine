using System;
using System.Collections.ObjectModel;
using System.Linq;
using Nine.Layers;
using Nine.Lessons.Holders;

namespace Nine.Lessons.Contents
{
    [Serializable]
    public abstract class Content : BaseModel
    {
        protected int _createdLayers;
        private Collection<AbstractLayer> _layers;

        public Content(Holder holder, string name)
        {
            Holder = holder;
            Name = name;
            _createdLayers = 1;
        }

        /// <summary>
        ///     Gets or sets the name (no need to forward it with the Holder).
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Refer to the parent lesson
        ///     Useful to resolve layers' name
        /// </summary>
        /// <value>
        ///     The holder.
        /// </value>
        public Holder Holder { get; protected set; }

        /// <summary>
        ///     Gets the collection of Layer.
        /// </summary>
        /// <value>
        ///     The layers.
        /// </value>
        public Collection<AbstractLayer> Layers
        {
            get
            {
                if (_layers == null)
                    Layers = new Collection<AbstractLayer>();
                return _layers;
            }
            protected set { _layers = value; }
        }

        protected string GetFallbackName()
        {
            var name = "";
            do
            {
                name = "Calque " + _createdLayers++;
            } while (NameExist(name));
            return name;
        }

        public bool NameExist(string name)
        {
            foreach (var layer in Layers)
                if (name == layer.Name)
                    return true;
            return false;
        }

        public AbstractLayer GetLayerByUid(int uid)
        {
            return Layers.First(l => l.UID == uid);
        }

        public abstract int AddLayer(string name = "", bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true);

        public abstract void RenameLayer(int uid, string name);
        public abstract void DeleteLayer(int uid);
        public abstract void ImportLayer(AbstractLayer layer);
    }
}