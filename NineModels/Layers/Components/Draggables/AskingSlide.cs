using System;

namespace Nine.Sharing
{
    [Serializable]
    public class AskingSlide
    {
        public AskingSlide(int userId)
        {
            User = userId;
        }

        public int User { get; private set; }
    }
}