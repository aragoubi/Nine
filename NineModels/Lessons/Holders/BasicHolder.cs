using System;
using System.Collections.Generic;
using Nine.Layers;

namespace Nine.Lessons.Holders
{
    /// <summary>
    ///     Abstract class that centralise the management of the layers (name & visibility) as well as their containers
    ///     (Content class).
    ///     This intermediate allows every layers to have only their UID as identification to simplify the naming process (cf.
    ///     see the property "Name" from Layer).
    /// </summary>
    [Serializable]
    public abstract class BasicHolder : Holder
    {
        public override int ImportLayers(string name, List<ParallelLayer> layers)
        {
            throw new NotImplementedException();
        }
    }
}