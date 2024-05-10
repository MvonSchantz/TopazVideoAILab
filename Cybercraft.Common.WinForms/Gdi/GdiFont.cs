using System;
using System.Drawing;

namespace Cybercraft.Common.WinForms.Gdi
{
    public class GdiFont : IDisposable
    {
        public IntPtr HFont { get; private set; }

        private bool IsCopyOfOtherFont { get; }

        public GdiFont()
        {
            HFont = IntPtr.Zero;
            IsCopyOfOtherFont = false;
        }

        public GdiFont(IntPtr hFont)
        {
            HFont = hFont;
            IsCopyOfOtherFont = true;
        }

        public GdiFont(Font font)
        {
            HFont = font.ToHfont();
            IsCopyOfOtherFont = false;
        }

        public GdiFont(int height, int orientation, Gdi32.FontWeight weight, bool italic, bool underline, bool strikeout, Gdi32.FontQuality fontQuality, Gdi32.FontPitchAndFamily pitchAndFamily, string fontName)
        {
            HFont = Gdi32.CreateFont(height, 0, orientation, orientation, weight, italic ? 1u : 0, underline ? 1u : 0,
                strikeout ? 1u : 0, Gdi32.FontCharSet.ANSI_CHARSET, Gdi32.FontPrecision.OUT_DEFAULT_PRECIS,
                Gdi32.FontClipPrecision.CLIP_DEFAULT_PRECIS, fontQuality, pitchAndFamily, fontName);
            IsCopyOfOtherFont = false;
        }

        public static GdiFont FromHandle(IntPtr hFont) => new GdiFont(hFont);

        public static GdiFont FromFont(Font font) => new GdiFont(font);

        public static implicit operator IntPtr(GdiFont font) => font.HFont;

        private void ReleaseUnmanagedResources()
        {
            if (!IsCopyOfOtherFont)
            {
                Gdi32.DeleteObject(HFont);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GdiFont()
        {
            ReleaseUnmanagedResources();
        }
    }
}
