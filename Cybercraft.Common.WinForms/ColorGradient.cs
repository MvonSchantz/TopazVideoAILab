using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Cybercraft.Common.WinForms
{
    public struct GradientRgb
    {
        public float Value;
        public Rgb Color;

        public GradientRgb(float value, int r, int g, int b)
        {
            Value = value;
            Color = new Rgb(r, g, b);
        }

        public GradientRgb(float value, Color color)
        {
            Value = value;
            Color = new Rgb(color.R, color.G, color.B);
        }

        public GradientRgb(float value, Rgb color)
        {
            Value = value;
            Color = new Rgb(color.R, color.G, color.B);
        }
    }

    public class ColorGradient : List<GradientRgb>
    {
        private GradientRgb[] orderedGradient;

        private void OrderGradient()
        {
            if (orderedGradient == null && Count > 0)
            {
                orderedGradient = this.OrderBy(g => g.Value).ToArray();
            }
        }

        public Rgb GetRgb(float position)
        {
            OrderGradient();
            if (orderedGradient == null || orderedGradient.Length == 0)
                return null;
            if (position <= orderedGradient.First().Value)
                return orderedGradient.First().Color;
            if (position >= orderedGradient.Last().Value)
                return orderedGradient.Last().Color;

            for (int n = 1; n < orderedGradient.Length; n++)
            {
                if (orderedGradient[n-1].Value <= position && orderedGradient[n].Value >= position)
                {
                    float span = orderedGradient[n].Value - orderedGradient[n - 1].Value;
                    if (span <= 0.0f)
                        return orderedGradient[n].Color;
                    float progress = (position - orderedGradient[n - 1].Value) / span;
                    if (progress <= 0.0f)
                        return orderedGradient[n - 1].Color;
                    return orderedGradient[n].Color.Blend(orderedGradient[n - 1].Color, progress);
                }
            }
            return null;
        }

        public Color GetColor(float position, float factor = 1.0f)
        {
            var rgb = GetRgb(position);
            if (rgb != null)
            {
                return Color.FromArgb((int)(rgb.R * factor), (int)(rgb.G * factor), (int)(rgb.B * factor));
            }
            return Color.FromArgb(0, 0, 0, 0);
        }
    }
}
