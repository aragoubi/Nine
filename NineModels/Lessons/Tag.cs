using System;
using System.Collections.ObjectModel;
using Nine.Layers;
using Nine.Layers.Components.Draggables;
using Nine.Layers.Components.Links;
using Nine.Lessons.Contents;

namespace Nine.Lessons
{
    /// <summary>
    ///     Tag is a dedicated class to reference links between a specific tag and a layer.
    ///     It permits to simply the access to the tagged layers : our current data structure can be expensive to make a search
    ///     One instance of this class represent ONE tag. It contain a factory as a subclass who permit to safely create the
    ///     full collection of existing tags. This collection should be bound to the Lesson.
    /// </summary>
    [Serializable]
    public class Tag : BaseModel
    {
        private Collection<AbstractLayer> _concernedLayers;
        private string _name;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Tag" /> class.
        ///     The name represent the resource (not user centric)
        /// </summary>
        /// <param name="name">The name.</param>
        public Tag(string name)
        {
            Name = name;
        }

        /// <summary>
        ///     Gets the concerned layers.
        /// </summary>
        /// <value>
        ///     The concerned layers.
        /// </value>
        public Collection<AbstractLayer> ConcernedLayers
        {
            get
            {
                if (_concernedLayers == null)
                {
                    ConcernedLayers = new Collection<AbstractLayer>();
                }
                // TODO: Should be read-only
                return _concernedLayers;
            }
            private set { _concernedLayers = value; }
        }

        /// <summary>
        ///     Gets or sets the name of the tag.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    Name = "Undefined tag";
                }
                return _name;
            }
            private set { _name = value; }
        }

        /// <summary>
        ///     Adds the layer (internal as it only should be called by the AnchoredTag)
        /// </summary>
        /// <param name="l">The layer.</param>
        internal void AddLayer(AbstractLayer l)
        {
            ConcernedLayers.Add(l);
        }

        /// <summary>
        ///     Removes the layer. (internal as it only should be called by the AnchoredTag)
        /// </summary>
        /// <param name="l">The layer.</param>
        internal void RemoveLayer(AbstractLayer l)
        {
            ConcernedLayers.Remove(l);
        }

        /// <summary>
        ///     Related slides of the Tag.
        ///     Layer-level is too deep, we prefer explore the slides first.
        /// </summary>
        /// <returns></returns>
        public Collection<Content> RelatedSlides()
        {
            var slides = new Collection<Content>();

            foreach (var layer in ConcernedLayers)
                slides.Add(layer.Content);

            // TODO: Careful to duplicates!!

            return slides;
        }

        /// <summary>
        ///     Remove an AnchoredTag from a Layer, so we recompute the presence of this AnchoredTag on that Layer
        /// </summary>
        /// <param name="anchoredTag">The anchored tag.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        internal void RemoveMe(AnchoredTag anchor)
        {
            if (anchor.Link.IsDead())
                return;

            var nbLinksOfThatTagOnThatLayer = 0;
            var layer = (anchor.Link as LayerLink).Layer;
            foreach (var component in layer.Components)
                if (component.GetType() == typeof (AnchoredTag) && (component as AnchoredTag).Tag == this)
                    nbLinksOfThatTagOnThatLayer++;


            if (nbLinksOfThatTagOnThatLayer == 0)
                ConcernedLayers.Remove(layer);
        }

        /// <summary>
        ///     The Factory who permit to instanciate the good Tag() with the right name.
        /// </summary>
        public static class TagFactory
        {
            /// <summary>
            ///     Gets the Tags existing as ressources.
            /// </summary>
            /// <returns></returns>
            public static Collection<Tag> GetTags()
            {
                var tags = new Collection<Tag>
                {
                    new Tag("alarm"),
                    new Tag("battery"),
                    new Tag("bug"),
                    new Tag("conversation"),
                    new Tag("earth"),
                    new Tag("exclamation"),
                    new Tag("flash"),
                    new Tag("game_controller"),
                    new Tag("gears"),
                    new Tag("heart"),
                    new Tag("idea"),
                    new Tag("info"),
                    new Tag("lock"),
                    new Tag("paint_palette"),
                    new Tag("pin"),
                    new Tag("reload"),
                    new Tag("save"),
                    new Tag("share"),
                    new Tag("star"),
                    new Tag("tick"),
                    new Tag("wrench")
                };
                return tags;
            }
        }
    }
}