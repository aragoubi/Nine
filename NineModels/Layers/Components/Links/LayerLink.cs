using System;

namespace Nine.Layers.Components.Links
{
    /// <summary>
    ///     Permit the creation of link between layers.
    /// </summary>
    [Serializable]
    public class LayerLink : BaseModel, ILink
    {
        private bool _dead;
        private AbstractLayer _linkedLayer;

        public LayerLink(AbstractLayer layer)
        {
            Layer = layer;
            Layer.References.Add(this);
            _dead = false;
        }

        /// <summary>
        ///     The linked layer
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Layer doesn't longer exist.</exception>
        public AbstractLayer Layer
        {
            get
            {
                if (IsDead())
                    throw new InvalidOperationException("Layer doesn't longer exist.");
                return _linkedLayer;
            }
            private set { _linkedLayer = value; }
        }

        /// <summary>
        ///     If the link is broken
        /// </summary>
        public bool IsDead()
        {
            return _dead;
        }

        /// <summary>
        ///     Layers the doesnt exist anymore (called when a Layer is deleted)
        ///     We keep the LayerLink instance but we can tell the user that the link is broken
        /// </summary>
        public void LayerDoesntExistAnymore()
        {
            Layer = null;
            _dead = true;
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="LayerLink" /> class.
        ///     If the LayerLink is deleted, we want to delete our presence from the Layer
        /// </summary>
        ~LayerLink()
        {
            if (!IsDead())
                Layer.References.Remove(this);
        }
    }
}