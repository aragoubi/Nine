using System;
using System.Collections.Generic;
using Nine.Layers;
using Nine.Lessons.Contents;

namespace Nine.Lessons.Holders
{
    /// <summary>
    ///     Abstract class that centralise the management of the layers (name & visibility) as well as their containers
    ///     (Content class).
    ///     This intermediate allows every layers to have only their UID as identification to simplify the naming process (cf.
    ///     see the property "Name" from Layer).
    /// </summary>
    [Serializable]
    public abstract class ParallelHolder : Holder
    {
        private int _createdLayers;
        private int _importedLayers;
        private Dictionary<int, string> _layerNames;
        private Dictionary<int, bool> _layerVisibility;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasicHolder" /> class.
        ///     Has to be reused
        /// </summary>
        protected ParallelHolder()
        {
            _createdLayers = 1;
            _importedLayers = 1;
        }

        /// <summary>
        ///     The correspondence dictionary between UID and layers' name
        /// </summary>
        public Dictionary<int, string> LayerNames
        {
            get
            {
                if (_layerNames == null)
                {
                    LayerNames = new Dictionary<int, string>();
                }
                return _layerNames;
            }
            private set { _layerNames = value; }
        }

        /// <summary>
        ///     Associate, for each level of layer, if they have to be shown or not
        /// </summary>
        /// <value>
        ///     The layer visibility.
        /// </value>
        public Dictionary<int, bool> LayerVisibility
        {
            get
            {
                if (_layerVisibility == null)
                {
                    LayerVisibility = new Dictionary<int, bool>();
                }
                return _layerVisibility;
            }
            private set { _layerVisibility = value; }
        }

        /// <summary>
        ///     Create a layer (user-POV)
        ///     Invoke Layer instanciation on each slide
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Slide name already referenced.</exception>
        internal virtual int AddLayer(string name = "", bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
        {
            if (LayerNames.ContainsValue(name))
                throw new ArgumentException("Slide name already referenced.", "name");

            // If unnamed, we name it
            if (name == "")
            {
                do
                {
                    name = "Calque " + _createdLayers;
                    _createdLayers++;
                } while (LayerNames.ContainsValue(name));
            }

            var uid = Guid.NewGuid().GetHashCode();
            LayerNames.Add(uid, name);
            LayerVisibility.Add(uid, true);

            foreach (var content in Contents)
                (content as ParallelContent).LayerCreation(uid, isRenameable, isHideable, isDeletable, isShareable,
                    isInkable);

            return uid;
        }

        /// <summary>
        ///     Renames a layer (user-POV)
        /// </summary>
        /// <param name="uid">The uid.</param>
        /// <param name="newName">The new name.</param>
        /// <exception cref="System.ArgumentException">
        ///     UID not found
        ///     or
        ///     Slide name already referenced.;newName
        /// </exception>
        internal virtual void RenameLayer(int uid, string newName)
        {
            if (!LayerNames.ContainsKey(uid))
                throw new ArgumentException("UID not found.", "uid");

            if (LayerNames.ContainsValue(newName) && LayerNames[uid] != newName)
                throw new ArgumentException("Slide name already referenced.", "newName");

            LayerNames[uid] = newName;
        }

        /// <summary>
        ///     Removes a layer (user-POV).
        ///     Invoke Layer deletion on each slide
        /// </summary>
        /// <param name="uid">The UID.</param>
        /// <exception cref="System.ArgumentException">Slide name isn't referenced.</exception>
        internal virtual void DeleteLayer(int uid)
        {
            if (!LayerNames.ContainsKey(uid))
                throw new ArgumentException("Slide UID isn't referenced.", "uid");

            foreach (var content in Contents)
                (content as ParallelContent).LayerDeletion(uid);

            LayerNames.Remove(uid);
            LayerVisibility.Remove(uid);
        }

        /// <summary>
        ///     Gets the UID from a name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">No UID for that name.</exception>
        public int GetUid(string name)
        {
            foreach (var p in LayerNames)
                if (p.Value == name)
                    return p.Key;

            throw new ArgumentException("No UID for that name.", "name");
        }

        public override int ImportLayers(string name, List<ParallelLayer> layers)
        {
            if (Contents.Count != layers.Count)
                throw new InvalidOperationException(
                    "Both lesson of origine and destination have different number of pages.");

            var uid = Guid.NewGuid().GetHashCode();
            string newName;
            do
            {
                newName = "Partage " + _importedLayers;
                _importedLayers++;
            } while (LayerNames.ContainsValue(newName));

            for (var i = 0; i < Contents.Count; i++)
                (Contents[i] as ParallelContent).LayerImportation(layers[i], uid);

            LayerNames.Add(uid, newName);
            LayerVisibility.Add(uid, true);

            return uid;
        }
    }
}