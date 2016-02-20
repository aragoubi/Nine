using System;
using Nine.Lessons.Holders;

namespace Nine.Sharing
{
    [Serializable]
    public abstract class SharedLayer
    {
        public SharedLayer(int uid, string userName, bool isTeacher)
        {
            UID = uid;
            UserName = userName;
            IsTeacher = isTeacher; // We can't refuse a layer provided by the teacher
        }

        public string Name { get; protected set; }
        public string UserName { get; protected set; }
        public bool IsTeacher { get; protected set; }
        public int UID { get; set; }
        public abstract void AddThem(Holder holder);
    }
}