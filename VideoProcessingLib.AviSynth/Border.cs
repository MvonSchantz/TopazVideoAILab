
namespace VideoProcessingLib.AviSynth
{
    public class Border
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public bool IsSet => Left != 0 || Top != 0 || Right != 0 || Bottom != 0;

        public override string ToString()
        {
            return $"L:{Left} T:{Top} R:{Right} B:{Bottom}";
        }

        public string AvsString => $"{Left},{Top},{Right},{Bottom}";

        public Border() { }

        public Border(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public Border(Border border)
        {
            if (border != null)
            {
                Left = border.Left;
                Top = border.Top;
                Right = border.Right;
                Bottom = border.Bottom;
            }
        }
    }
}
