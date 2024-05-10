using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Cybercraft.Common.WinForms.Gdi
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GdiPen : IDisposable
    {
        public IntPtr HPen { get; private set; }

        private bool IsCopyOfOtherPen { get; }

        public GdiPen()
        {
            HPen = IntPtr.Zero;
            IsCopyOfOtherPen = false;
        }

        public GdiPen(int width, uint crColor, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID)
        {
            HPen = Gdi32.CreatePen(penStyle, width, crColor);
            IsCopyOfOtherPen = false;
        }

        public GdiPen(int width, Color color, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID)
        {
            //int rgb = (int)(((uint)color.ToArgb()) & 0x00FFFFFF);
            uint rgb = (uint)color.ToArgb();
            HPen = Gdi32.CreatePen(penStyle, width, Gdi32.RGB((int)(rgb & 0xFF0000) >> 16, (int)(rgb & 0x00FF00) >> 8, (int)(rgb & 0x0000FF) >> 0));
            IsCopyOfOtherPen = false;
        }

        public GdiPen(int width, byte r, byte g, byte b, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID) : this(width, Gdi32.RGB(r, g, b), penStyle) { }

        public GdiPen(IntPtr hPen)
        {
            HPen = hPen;
            IsCopyOfOtherPen = true;
        }

        public static GdiPen FromHandle(IntPtr hPen) => new GdiPen(hPen);

        public static implicit operator IntPtr(GdiPen pen) => pen.HPen;

        private void ReleaseUnmanagedResources()
        {
            if (!IsCopyOfOtherPen)
            {
                Gdi32.DeleteObject(HPen);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GdiPen()
        {
            ReleaseUnmanagedResources();
        }
    }
}
