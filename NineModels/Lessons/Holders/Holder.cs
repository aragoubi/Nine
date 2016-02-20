using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Nine.Layers;
using Nine.Lessons.Contents;

namespace Nine.Lessons.Holders
{
    [Serializable]
    public abstract class Holder : BaseModel
    {
        private Collection<Content> _contents;

        /// <summary>
        ///     The "pages" (can be slides, exerices...) who compose that lesson.
        /// </summary>
        public Collection<Content> Contents
        {
            get
            {
                if (_contents == null)
                {
                    Contents = new Collection<Content>();
                }
                return _contents;
            }
            private set { _contents = value; }
        }

        /// <summary>
        ///     Create a new instance of a *Content
        /// </summary>
        public abstract void AddNewContent(string name);

        /// <summary>
        ///     Imports a list of layers (could be not implemented)
        /// </summary>
        public abstract int ImportLayers(string name, List<ParallelLayer> layers);
    }
}