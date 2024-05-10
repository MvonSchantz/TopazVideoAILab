using System;

namespace VideoProcessingLib.AviSynth
{
    public class Size : IEquatable<Size>
    {
        public int Width;
        public int Height;

        public long Memory => Width * Height;

        public override string ToString()
        {
            return $"{Width.ToInvariant()}x{Height.ToInvariant()}";
        }

        public string AvsString => $"{Width.ToInvariant()},{Height.ToInvariant()}";

        public Size(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Size(Size size)
        {
            if (size != null)
            {
                Width = size.Width;
                Height = size.Height;
            }
        }

        public Size(System.Drawing.Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }

        public static Size operator /(Size s, int div)
        {
            return new Size(s.Width / div, s.Height / div);
        }

        public static Size operator *(Size s, int div)
        {
            return new Size(s.Width * div, s.Height * div);
        }

        public static Size operator +(Size s1, Size s2)
        {
            return new Size(s1.Width + s2.Width, s1.Height + s2.Height);
        }

        public void Swap()
        {
            int t = Width;
            Width = Height;
            Height = t;
        }

        public static Size Fit(Size inside, Size outside)
        {
            if (inside.Width == 0)
                return new Size(outside);

            int width = outside.Width;
            int height = (width * inside.Height) / inside.Width;

            if (height > outside.Height)
            {
                height = outside.Height;
                width = (height * inside.Width) / inside.Height;
            }

            width = (width / 2) * 2;
            height = (height / 2) * 2;

            return new Size(width, height);
        }

        public bool Equals(Size other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Size)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width * 397) ^ Height;
            }
        }
    }
}
