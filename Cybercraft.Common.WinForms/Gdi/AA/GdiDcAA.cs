using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Cybercraft.Common.WinForms.Gdi
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GdiDcAA : IDisposable
    {
        public IntPtr HDC { get; private set; }

        private bool IsCopyOfOtherDC { get; }

        private Graphics GraphicsObject { get; }

        private int AntiAliasLevel { get; }

        public GdiDcAA(int antiAliasLevel)
        {
            AntiAliasLevel = antiAliasLevel;
            HDC = IntPtr.Zero;
            IsCopyOfOtherDC = false;
        }

        public GdiDcAA(IntPtr hDC, int antiAliasLevel)
        {
            HDC = hDC;
            AntiAliasLevel = antiAliasLevel;
            IsCopyOfOtherDC = true;
        }

        public GdiDcAA(Graphics g, int antiAliasLevel)
        {
            HDC = g.GetHdc();
            IsCopyOfOtherDC = true;
            GraphicsObject = g;
            AntiAliasLevel = antiAliasLevel;
        }

        public static GdiDcAA FromHandle(IntPtr hDC, int antiAliasLevel) => new GdiDcAA(hDC, antiAliasLevel);

        public static GdiDcAA FromGraphics(Graphics g, int antiAliasLevel) => new GdiDcAA(g, antiAliasLevel);

        public static implicit operator IntPtr(GdiDcAA dc) => dc.HDC;

        public bool CreateCompatibleDC(IntPtr hDC)
        {
            HDC = Gdi32.CreateCompatibleDC(hDC);
            return HDC != IntPtr.Zero;
        }

        public GdiPen CreatePen(int width, uint crColor, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID) => new GdiPen(width * AntiAliasLevel, crColor, penStyle);

        public GdiPen CreatePen(int width, Color color, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID) => new GdiPen(width * AntiAliasLevel, color, penStyle);

        public GdiPen CreatePen(int width, byte r, byte g, byte b, Gdi32.PenStyle penStyle = Gdi32.PenStyle.PS_SOLID) => new GdiPen(width * AntiAliasLevel, r, g, b, penStyle);


        public GdiPen SelectObject(GdiPen pen)
        {
            IntPtr hOldPen = Gdi32.SelectObject(HDC, pen);
            return new GdiPen(hOldPen);
        }

        public GdiFont CreateFont(Font font) => new GdiFont(font.Height * AntiAliasLevel, 0, font.Bold ? Gdi32.FontWeight.FW_BOLD : Gdi32.FontWeight.FW_NORMAL, font.Italic, font.Underline, font.Strikeout, Gdi32.FontQuality.ANTIALIASED_QUALITY, Gdi32.FontPitchAndFamily.DEFAULT_PITCH, font.Name);

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
            if (AntiAliasLevel == 1)
            {
                return Gdi32.BitBlt(HDC, nXDest, nYDest, nWidth, nHeight, srcDc, nXSrc, nYSrc, dwRop);
            }
            else
            {
                return Gdi32.StretchBlt(HDC, nXDest * AntiAliasLevel, nYDest * AntiAliasLevel, nWidth * AntiAliasLevel, nHeight * AntiAliasLevel, srcDc, nXSrc, nYSrc, nWidth, nHeight, dwRop);
            }
        }

        public bool Rectangle(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
        {
            return Gdi32.Rectangle(HDC, nLeftRect * AntiAliasLevel, nTopRect * AntiAliasLevel, nRightRect * AntiAliasLevel, nBottomRect * AntiAliasLevel);
        }

        public bool Rectangle(Rectangle rect)
        {
            return Gdi32.Rectangle(HDC, rect.Left * AntiAliasLevel, rect.Top * AntiAliasLevel, rect.Right * AntiAliasLevel, rect.Bottom * AntiAliasLevel);
        }

        public bool RoundRect(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int cornerWidth, int cornerHeight = -1)
        {
            return Gdi32.RoundRect(HDC, nLeftRect * AntiAliasLevel, nTopRect * AntiAliasLevel, nRightRect * AntiAliasLevel, nBottomRect * AntiAliasLevel, cornerWidth * AntiAliasLevel, cornerHeight == -1 ? cornerWidth * AntiAliasLevel : cornerHeight * AntiAliasLevel);
        }

        public bool RoundRect(Rectangle rect, int cornerWidth, int cornerHeight = -1)
        {
            return Gdi32.RoundRect(HDC, rect.Left * AntiAliasLevel, rect.Top * AntiAliasLevel, rect.Right * AntiAliasLevel, rect.Bottom * AntiAliasLevel, cornerWidth * AntiAliasLevel, cornerHeight == -1 ? cornerWidth * AntiAliasLevel : cornerHeight * AntiAliasLevel);
        }

        public bool FillRect(ref RECT rect, GdiBrush brush)
        {
            if (AntiAliasLevel > 1)
            {
                var aaRect = new RECT(rect.Left * AntiAliasLevel, rect.Top * AntiAliasLevel, rect.Right * AntiAliasLevel, rect.Bottom * AntiAliasLevel);
                return Gdi32.FillRect(HDC, ref aaRect, brush.HBrush);
            }
            else
            {
                return Gdi32.FillRect(HDC, ref rect, brush.HBrush);
            }
        }

        public bool FillRect(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, GdiBrush brush)
        {
            var rect = new RECT(nLeftRect * AntiAliasLevel, nTopRect * AntiAliasLevel, nRightRect * AntiAliasLevel, nBottomRect * AntiAliasLevel);
            return Gdi32.FillRect(HDC, ref rect, brush.HBrush);
        }

        public bool FillRect(Rectangle rect, GdiBrush brush)
        {
            var gdiRect = new RECT(rect.Left * AntiAliasLevel, rect.Top * AntiAliasLevel, rect.Right * AntiAliasLevel, rect.Bottom * AntiAliasLevel);
            return Gdi32.FillRect(HDC, ref gdiRect, brush.HBrush);
        }

        public bool Ellipse(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect)
        {
            return Gdi32.Ellipse(HDC, nLeftRect * AntiAliasLevel, nTopRect * AntiAliasLevel, nRightRect * AntiAliasLevel, nBottomRect * AntiAliasLevel);
        }

        public bool Ellipse(Rectangle rect)
        {
            return Gdi32.Ellipse(HDC, rect.Left * AntiAliasLevel, rect.Top * AntiAliasLevel, rect.Right * AntiAliasLevel, rect.Bottom * AntiAliasLevel);
        }

        public bool MoveToEx(int x, int y)
        {
            return Gdi32.MoveToEx(HDC, x * AntiAliasLevel, y * AntiAliasLevel, IntPtr.Zero);
        }

        public bool LineTo(int x, int y)
        {
            return Gdi32.LineTo(HDC, x * AntiAliasLevel, y * AntiAliasLevel);
        }

        // ReSharper disable once IdentifierTypo
        public bool PolyBezier(POINT[] points)
        {
            if (AntiAliasLevel > 1)
            {
                var aaPoints = points.Select(p => new POINT(p.X * AntiAliasLevel, p.Y * AntiAliasLevel)).ToArray();
                return Gdi32.PolyBezier(HDC, aaPoints, (uint)points.Length);
            }
            else
            {
                return Gdi32.PolyBezier(HDC, points, (uint)points.Length);
            }
        }
        
        // ReSharper disable once IdentifierTypo
        public bool PolyBezier(Point[] points)
        {
            return Gdi32.PolyBezier(HDC, points.Select(p => new POINT(p.X * AntiAliasLevel, p.Y * AntiAliasLevel)).ToArray(), (uint)points.Length);
        }

        public bool Polygon(POINT[] points)
        {
            if (AntiAliasLevel > 1)
            {
                var aaPoints = points.Select(p => new POINT(p.X * AntiAliasLevel, p.Y * AntiAliasLevel)).ToArray();
                return Gdi32.Polygon(HDC, aaPoints, (uint)points.Length);
            }
            else
            {
                return Gdi32.Polygon(HDC, points, (uint)points.Length);
            }
        }

        // ReSharper disable once IdentifierTypo
        public bool Polygon(Point[] points)
        {
            return Gdi32.Polygon(HDC, points.Select(p => new POINT(p.X * AntiAliasLevel, p.Y * AntiAliasLevel)).ToArray(), (uint)points.Length);
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
            return Gdi32.TextOut(HDC, nXStart * AntiAliasLevel, nYStart * AntiAliasLevel, text, text.Length);
        }

        public int DrawText(string text, ref RECT rect, uint format)
        {
            if (AntiAliasLevel > 1)
            {
                var aaRect = new RECT(rect.Left * AntiAliasLevel, rect.Top * AntiAliasLevel, rect.Right * AntiAliasLevel, rect.Bottom * AntiAliasLevel);
                return Gdi32.DrawText(HDC, text, -1, ref aaRect, format);
            }
            else
            {
                return Gdi32.DrawText(HDC, text, -1, ref rect, format);
            }
        }

        public int DrawText(string text, ref RECT rect, Gdi32.DrawTextFormat format)
        {
            if (AntiAliasLevel > 1)
            {
                var aaRect = new RECT(rect.Left * AntiAliasLevel, rect.Top * AntiAliasLevel, rect.Right * AntiAliasLevel, rect.Bottom * AntiAliasLevel);
                return Gdi32.DrawText(HDC, text, -1, ref aaRect, (uint)format);
            }
            else
            {
                return Gdi32.DrawText(HDC, text, -1, ref rect, (uint)format);
            }
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

        ~GdiDcAA()
        {
            ReleaseUnmanagedResources();
        }
    }
}
