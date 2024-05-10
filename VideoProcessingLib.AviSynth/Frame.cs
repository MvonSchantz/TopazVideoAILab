
namespace VideoProcessingLib.AviSynth
{
    public class Frame
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;

        public override string ToString() => $"{X},{Y} {Width}x{Height}";

        public string AvsString => $"{X},{Y},{Width},{Height}";

        public Frame(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Frame(Frame frame)
        {
            if (frame != null)
            {
                X = frame.X;
                Y = frame.Y;
                Width = frame.Width;
                Height = frame.Height;
            }
        }
    }
}
