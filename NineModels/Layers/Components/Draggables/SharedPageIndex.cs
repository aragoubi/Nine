using System;

namespace Nine.Sharing
{
    [Serializable]
    public class SharedPageIndex
    {
        public SharedPageIndex(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
}