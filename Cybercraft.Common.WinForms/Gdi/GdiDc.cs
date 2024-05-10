using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.VisualStyles;

namespace Cybercraft.Common.WinForms.Gdi
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GdiDc : IDisposable
    {
        public IntPtr HDC { get; private set; }

        private bool IsCopyOfOtherDC { get; }

        private Graphics GraphicsObject { get; }

        public GdiDc()
        {
            HDC = IntPtr.Zero;
            IsCopyOfOtherDC = false;
        }

        public GdiDc(IntPtr hDC)
        {
            HDC = hDC;
            IsCopyOfOtherDC = true;
        }

        public GdiDc(Graphics g)
        {
            HDC = g.GetHdc();
            IsCopyOfOtherDC = true;
            GraphicsObject = g;
        }

        public static GdiDc FromHandle(IntPtr hDC) => new GdiDc(hDC);

        public static GdiDc FromGraphics(Graphics g) => new GdiDc(g);

        public static implicit operator IntPtr(GdiDc dc) => dc.HDC;

        public bool CreateCompatibleDC(IntPtr hDC)
        {
            HDC = Gdi32.CreateCompatibleDC(hDC);
            return HDC != IntPtr.Zero;
        }

        public GdiPen CreatePen(int width, uint crColor, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID) => new GdiPen(width, crColor, penStyle);

        public GdiPen CreatePen(int width, Color color, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID) => new GdiPen(width, color, penStyle);

        public GdiPen CreatePen(int width, byte r, byte g, byte b, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID) => new GdiPen(width, r, g, b, penStyle);

        public GdiPen SelectObject(GdiPen pen)
        {
            IntPtr hOldPen = Gdi32.SelectObject(HDC, pen);
            return new GdiPen(hOldPen);
        }

        public GdiFont CreateFont(Font font) => new GdiFont(font.Height, 0, font.Bold ? Gdi32.FontWeight.FW_BOLD : Gdi32.FontWeight.FW_NORMAL, font.Italic, font.Underline, font.Strikeout, Gdi32.FontQuality.CLEARTYPE_NATURAL_QUALITY, Gdi32.FontPitchAndFamily.DEFAULT_PITCH, font.Name);

        public GdiFont SelectObject(GdiFont font)
        {
            IntPtr hOldPen = Gdi32.SelectObject(HDC, font);
            return new GdiFont(hOldPen);
        }

        public GdiBrush SelectObject(GdiBrush brush)
        {
            IntPtr hOldBrush = Gdi32.SelectObject(HDC, brush);
            return new GdiBrush(hOldBrush);
        }

        public GdiBitmap SelectObject(GdiBitmap bitmap)
        {
            IntPtr hOldBitmap = Gdi32.SelectObject(HDC, bitmap);
            return new GdiBitmap(hOldBitmap);
        }

        public GdiRegion SelectObject(GdiRegion region)
        {
            IntPtr hOldRegion = Gdi32.SelectObject(HDC, region);
            return new GdiRegion(hOldRegion);
        }

        public bool BitBlt(int nXDest, int nYDest, int nWidth, int nHeight, GdiDc srcDc, int nXSrc, int nYSrc, Gdi32.TernaryRasterOperations dwRop)
        {
            return Gdi32.BitBlt(HDC, nXDest, nYDest, nWidth, nHeight, srcDc, nXSrc, nYSrc, dwRop);
        }

        public bool Rectangle(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
        {
            return Gdi32.Rectangle(HDC, nLeftRect, nTopRect, nRightRect, nBottomRect);
        }

        public bool Rectangle(Rectangle rect)
        {
            return Gdi32.Rectangle(HDC, rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public bool RoundRect(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int cornerWidth, int cornerHeight = -1)
        {
            return Gdi32.RoundRect(HDC, nLeftRect, nTopRect, nRightRect, nBottomRect, cornerWidth, cornerHeight == -1 ? cornerWidth : cornerHeight);
        }

        public bool RoundRect(Rectangle rect, int cornerWidth, int cornerHeight = -1)
        {
            return Gdi32.RoundRect(HDC, rect.Left, rect.Top, rect.Right, rect.Bottom, cornerWidth, cornerHeight == -1 ? cornerWidth : cornerHeight);
        }

        public bool FillRect(ref RECT rect, GdiBrush brush)
        {
            return Gdi32.FillRect(HDC, ref rect, brush.HBrush);
        }

        public bool FillRect(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, GdiBrush brush)
        {
            var rect = new RECT(nLeftRect, nTopRect, nRightRect, nBottomRect);
            return Gdi32.FillRect(HDC, ref rect, brush.HBrush);
        }

        public bool FillRect(Rectangle rect, GdiBrush brush)
        {
            var gdiRect = new RECT(rect);
            return Gdi32.FillRect(HDC, ref gdiRect, brush.HBrush);
        }

        public bool Ellipse(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
        {
            return Gdi32.Ellipse(HDC, nLeftRect, nTopRect, nRightRect, nBottomRect);
        }

        public bool Ellipse(Rectangle rect)
        {
            return Gdi32.Ellipse(HDC, rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        public bool MoveToEx(int x, int y)
        {
            return Gdi32.MoveToEx(HDC, x, y, IntPtr.Zero);
        }

        public bool LineTo(int x, int y)
        {
            return Gdi32.LineTo(HDC, x, y);
        }

        // ReSharper disable once IdentifierTypo
        public bool PolyBezier(POINT[] points)
        {
            return Gdi32.PolyBezier(HDC, points, (uint)points.Length);
        }
        
        // ReSharper disable once IdentifierTypo
        public bool PolyBezier(Point[] points)
        {
            return Gdi32.PolyBezier(HDC, points.Select(p => new POINT(p)).ToArray(), (uint)points.Length);
        }

        public bool Polygon(POINT[] points)
        {
            return Gdi32.Polygon(HDC, points, (uint)points.Length);
        }

        // ReSharper disable once IdentifierTypo
        public bool Polygon(Point[] points)
        {
            return Gdi32.Polygon(HDC, points.Select(p => new POINT(p)).ToArray(), (uint)points.Length);
        }

        public uint SetTextColor(uint color)
        {
            return Gdi32.SetTextColor(HDC, color);
        }
        public uint SetTextColor(Color color)
        {
            uint rgb = ((uint)color.ToArgb()) & 0x00FFFFFF;
            return Gdi32.SetTextColor(HDC, rgb);
        }

        public int SetBkMode(int backgroundMode)
        {
            return Gdi32.SetBkMode(HDC, backgroundMode);
        }

        public Gdi32.BackgroundMode SetBkMode(Gdi32.BackgroundMode backgroundMode)
        {
            return (Gdi32.BackgroundMode)Gdi32.SetBkMode(HDC, (int)backgroundMode);
        }

        public bool TextOut(int nXStart, int nYStart, string text)
        {
            return Gdi32.TextOut(HDC, nXStart, nYStart, text, text.Length);
        }

        public int DrawText(string text, ref RECT rect, uint format)
        {
            return Gdi32.DrawText(HDC, text, -1, ref rect, format);
        }

        public int DrawText(string text, ref RECT rect, Gdi32.DrawTextFormat format)
        {
            return Gdi32.DrawText(HDC, text, -1, ref rect, (uint)format);
        }

        private void ReleaseUnmanagedResources()
        {
            if (!IsCopyOfOtherDC)
            {
                Gdi32.DeleteDC(HDC);
            } else if (GraphicsObject != null)
            {
                GraphicsObject.ReleaseHdc(HDC);
            }
        }

        public void Dispose()
        {
            ReleaseUnmanagedResources();
            GC.SuppressFinalize(this);
        }

        ~GdiDc()
        {
            ReleaseUnmanagedResources();
        }
    }
}
