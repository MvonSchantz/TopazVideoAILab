using System;
using System.Diagnostics.CodeAnalysis;

namespace Cybercraft.Common.WinForms.Gdi
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GdiRegion : IDisposable
    {
        public IntPtr HRegion { get; private set; }

        private bool IsCopyOfOtherRegion { get; }

        public GdiRegion()
        {
            HRegion = IntPtr.Zero;
            IsCopyOfOtherRegion = false;
        }

        public GdiRegion(IntPtr hRegion)
        {
            HRegion = hRegion;
            IsCopyOfOtherRegion = true;
        }

        public static GdiRegion FromHandle(IntPtr hRegion) => new GdiRegion(hRegion);

        public static implicit operator IntPtr(GdiRegion Region) => Region.HRegion;

        private void ReleaseUnmanagedResources()
        {
            if (!IsCopyOfOtherRegion)
            {
                Gdi32.DeleteObject(HRegion);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GdiRegion()
        {
            ReleaseUnmanagedResources();
        }
    }
}
