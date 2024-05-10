using System;
using System.Drawing;

namespace Cybercraft.Common.WinForms.Gdi
{
    public class GdiBrush : IDisposable
    {
        public IntPtr HBrush { get; private set; }

        private bool IsCopyOfOtherBrush { get; }

        public GdiBrush()
        {
            HBrush = IntPtr.Zero;
            IsCopyOfOtherBrush = false;
        }

        public GdiBrush(IntPtr hBrush)
        {
            HBrush = hBrush;
            IsCopyOfOtherBrush = true;
        }

        public GdiBrush(uint crColor)
        {
            HBrush = Gdi32.CreateSolidBrush(crColor);
            IsCopyOfOtherBrush = false;
        }

        public GdiBrush(Color color)
        {
            //int rgb = (int)(((uint)color.ToArgb()) & 0x00FFFFFF);
            uint rgb = (uint)color.ToArgb();
            HBrush = Gdi32.CreateSolidBrush(Gdi32.RGB((int)(rgb & 0xFF0000) >> 16, (int)(rgb & 0x00FF00) >> 8, (int)(rgb & 0x0000FF) >> 0));
            IsCopyOfOtherBrush = false;
        }

        public GdiBrush(byte r, byte g, byte b) : this(Gdi32.RGB(r, g, b)) {}

        public static GdiBrush FromHandle(IntPtr hBrush) => new GdiBrush(hBrush);

        public static implicit operator IntPtr(GdiBrush brush) => brush.HBrush;

        private void ReleaseUnmanagedResources()
        {
            if (!IsCopyOfOtherBrush)
            {
                Gdi32.DeleteObject(HBrush);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GdiBrush()
        {
            ReleaseUnmanagedResources();
        }
    }
}
