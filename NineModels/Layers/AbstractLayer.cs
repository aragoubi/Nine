using System;
using System.Collections.ObjectModel;
using Nine.Layers.Components.Draggables;
using Nine.Layers.Components.Links;
using Nine.Layers.Components.Strokes;
using Nine.Lessons.Contents;

namespace Nine.Layers
{
    /// <summary>
    ///     AbstractLayer is an abstract class who describe the minimal attributes of a Layer.
    ///     A layer should be composed of Strokes.
    /// </summary>
    [Serializable]
    public abstract class AbstractLayer : BaseModel
    {
        private Collection<DraggableComponent> _components;
        private LayerLink _link;
        private Collection<LayerLink> _references;
        private Collection<Stroke> _strokes;

        public AbstractLayer(Content content, int uid, bool isRenameable = true, bool isHideable = true,
            bool isDeletable = true, bool isShareable = true, bool isInkable = true)
        {
            Content = content;
            UID = uid;
            IsDisplayed = true;
            IsRenameable = isRenameable;
            IsHideable = isHideable;
            IsDeletable = isDeletable;
            IsShareable = isShareable;
            IsInkable = isInkable;
        }

        /// <summary>
        ///     The lesson content (who store all the layers)
        /// </summary>
        /// <value>
        ///     The Content class
        /// </value>
        public Content Content { get; internal set; }

        /// <summary>
        ///     In case of Parallel management: the UID permit to make the translation UID -&gt; Lesson.SlideLayerNames[UID]
        ///     Because of distinction of user/dev POV, we can't store the Name of the layer in the Layer instance (can lead to
        ///     failure on integrity)
        ///     We've decided to store it into the lesson and add a property to reach it (see Layer.Name property summary).
        ///     Setter is internal to allow importing layers.
        /// </summary>
        /// <value>
        ///     The uid.
        /// </value>
        public int UID { get; internal set; }

        /// <summary>
        ///     The Strokes that compose our layer.
        /// </summary>
        public Collection<Stroke> Strokes
        {
            get
            {
                if (_strokes == null)
                    Strokes = new Collection<Stroke>();
                return _strokes;
            }
            set { _strokes = value; }
        }

        /// <summary>
        ///     Tell if this layer should be displayed
        /// </summary>
        public virtual bool IsDisplayed { get; set; }

        /// <summary>
        ///     Tell if this layer can be renamed
        /// </summary>
        public virtual bool IsRenameable { get; internal set; }

        /// <summary>
        ///     Tell if this layer can be hidden
        /// </summary>
        public virtual bool IsHideable { get; internal set; }

        /// <summary>
        ///     Tell if this layer can be deleted
        /// </summary>
        public virtual bool IsDeletable { get; internal set; }

        /// <summary>
        ///     Tell if this layer can be shared
        /// </summary>
        public virtual bool IsShareable { get; internal set; }

        /// <summary>
        ///     Tell if this layer can be edited (strokes, tags, bullets, texts...)
        /// </summary>
        public virtual bool IsInkable { get; internal set; }

        /// <summary>
        ///     ExerciseLayer have their own name
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public virtual string Name { get; internal set; }

        /// <summary>
        ///     The draggable components of our layer (in opposition of strokes which are not draggable)
        /// </summary>
        /// <value>
        ///     The components.
        /// </value>
        public Collection<DraggableComponent> Components
        {
            get
            {
                if (_components == null)
                    Components = new Collection<DraggableComponent>();
                return _components;
            }
            private set { _components = value; }
        }

        /// <summary>
        ///     The References we know for that layer to every other layer
        /// </summary>
        /// <value>
        ///     The references.
        /// </value>
        public Collection<LayerLink> References
        {
            get
            {
                if (_references == null)
                    References = new Collection<LayerLink>();
                return _references;
            }
            private set { _references = value; }
        }

        /// <summary>
        ///     Gets the link of the Layer (management of references).
        /// </summary>
        /// <value>
        ///     The link.
        /// </value>
        public LayerLink Link
        {
            get
            {
                if (_link == null)
                {
                    Link = new LayerLink(this);
                }
                return _link;
            }
            private set { _link = value; }
        }

        /// <summary>
        ///     Finalizes an instance of the <see cref="ParallelLayer" /> class.
        ///     When a Layer is destroyed, we have to prevent other layer that could references us that we no longer exist
        /// </summary>
        ~AbstractLayer()
        {
            if (_references != null)
                foreach (var link in References)
                    link.LayerDoesntExistAnymore();
        }
    }
}