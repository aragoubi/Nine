using System;
using Nine.Layers;
using Nine.Lessons.Contents;

namespace Nine.Sharing
{
    [Serializable]
    public abstract class SharedBasicLayer : SharedLayer
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SharedBasicLayer" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="uid">The uid of the Layer.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="isTeacher">if set to <c>true</c> [is teacher].</param>
        public SharedBasicLayer(BasicContent content, int uid, string userName, bool isTeacher)
            : base(uid, userName, isTeacher)
        {
            // Nom of the exercise
            Name = content.Name;

            Layer = content.GetLayerByUid(uid) as BasicLayer;
        }

        public BasicLayer Layer { get; protected set; }
    }
}