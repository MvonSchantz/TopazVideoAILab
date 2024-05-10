using System;

namespace VideoProcessingLib.AviSynth
{
    public class Rgb
    {
        public int R;
        public int G;
        public int B;

        public override string ToString()
        {
            return AvsString;
        }

        public string AvsString => $"{R},{G},{B}";

        public Rgb(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }

        public Rgb(float r, float g, float b)
        {
            R = Math.Min((int)Math.Round(r), 255);
            G = Math.Min((int)Math.Round(g), 255);
            B = Math.Min((int)Math.Round(b), 255);
        }

        public Rgb(Rgb rgb)
        {
            if (rgb != null)
            {
                R = rgb.R;
                G = rgb.G;
                B = rgb.B;
            }
        }

        public Rgb Blend(Rgb blendColor, float thisWeight)
        {
            var newColor = new Rgb(Math.Min(255, Math.Max(0, R * thisWeight + blendColor.R * (1.0f - thisWeight))),
                Math.Min(255, Math.Max(0, G * thisWeight + blendColor.G * (1.0f - thisWeight))),
                Math.Min(255, Math.Max(0, B * thisWeight + blendColor.B * (1.0f - thisWeight))));
            return newColor;
        }
    }
}
