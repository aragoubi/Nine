using System;

namespace Nine.Layers.Components.Strokes.DA
{
    /// <summary>
    ///     Representation of an ARGB color
    /// </summary>
    [Serializable]
    public class Color : BaseModel
    {
        private byte _a;
        private byte _b;
        private byte _g;
        private byte _r;

        public Color()
        {
            //Black by default
            R = 0;
            G = 0;
            B = 0;
            A = 1;
        }

        public Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R
        {
            get { return _r; }
            set { _r = value; }
        }

        public byte G
        {
            get { return _g; }
            set { _g = value; }
        }

        public byte B
        {
            get { return _b; }
            set { _b = value; }
        }

        public byte A
        {
            get { return _a; }
            set { _a = value; }
        }

        public void SetColor(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }
    }
}