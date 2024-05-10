using System;

namespace Cybercraft.Common.WinForms.Gdi
{
    public class GdiBitmap : IDisposable
    {
        public IntPtr HBitmap { get; private set; }

        private bool IsCopyOfOtherBitmap { get; }

        public GdiBitmap()
        {
            HBitmap = IntPtr.Zero;
            IsCopyOfOtherBitmap = false;
        }

        public GdiBitmap(IntPtr hBitmap)
        {
            HBitmap = hBitmap;
            IsCopyOfOtherBitmap = true;
        }

        public static GdiBitmap FromHandle(IntPtr hBitmap) => new GdiBitmap(hBitmap);

        public static implicit operator IntPtr(GdiBitmap bitmap) => bitmap.HBitmap;

        private void ReleaseUnmanagedResources()
        {
            if (!IsCopyOfOtherBitmap)
            {
                Gdi32.DeleteObject(HBitmap);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GdiBitmap()
        {
            ReleaseUnmanagedResources();
        }
    }
}
