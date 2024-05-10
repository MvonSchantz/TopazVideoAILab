using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Cybercraft.Common.WinForms.Gdi;

namespace Cybercraft.Common.WinForms
{
    public class PaintGdiEventArgs : EventArgs {
        public IntPtr Dc { get; }
        public int Width { get; }
        public int Height { get; }
        public int AntiAliasingLevel { get; }

        public PaintGdiEventArgs(IntPtr dc, int width, int height, int antiAliasingLevel)
        {
            Dc = dc;
            Width = width;
            Height = height;
            AntiAliasingLevel = antiAliasingLevel;
        }
    }

    public class PaintPanel : Panel
    {
        private int antiAliasLevel = 1;

        public int AntiAliasLevel
        {
            get => antiAliasLevel;
            set => antiAliasLevel = Math.Max(1, Math.Min(4, value));
        }

        //  protected bool leftMouseButtonDown;

        public PaintPanel() : base()
        {
            //(this as Control).MouseWheel += new KeyEventHandler(OnMouseWheel);
        }

        
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (MouseWheelScroll != null)
            {
                MouseWheelScroll(this, e);
            }
        }

        public delegate void MouseWheelEventHandler(object sender, MouseEventArgs e);

        public event MouseWheelEventHandler MouseWheelScroll;


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Site != null)
            {
                if (Site.GetService(typeof(IDesignerHost)) is IDesignerHost host)
                {
                    if (host.RootComponent.Site.DesignMode)
                        base.OnPaintBackground(e);
                }
            }
        }

        public event PaintEventHandler PaintDoubleBuffer;

        protected virtual void OnPaintDoubleBuffer(PaintEventArgs e)
        {
            PaintEventHandler handler = PaintDoubleBuffer;
            handler?.Invoke(this, e);
        }

        private Bitmap Backbuffer { get; set; } = new Bitmap(1, 1, PixelFormat.Format32bppRgb);

        private int GdiBackbufferWidth { get; set; }
        private int GdiBackbufferHeight { get; set; }

        private IntPtr GdiBackbufferBitmap { get; set; } = IntPtr.Zero;
 
        protected override void OnPaint(PaintEventArgs e)
        {
            if (PaintGdi != null)
            {
                IntPtr dc = e.Graphics.GetHdc();

                IntPtr gdiBackbufferDc = Gdi32.CreateCompatibleDC(dc);

                if (GdiBackbufferBitmap == IntPtr.Zero || GdiBackbufferWidth != e.ClipRectangle.Width * AntiAliasLevel || GdiBackbufferHeight != e.ClipRectangle.Height * AntiAliasLevel)
                {
                    GdiBackbufferBitmap = Gdi32.CreateCompatibleBitmap(dc, e.ClipRectangle.Width * AntiAliasLevel, e.ClipRectangle.Height * AntiAliasLevel);
                    GdiBackbufferWidth = e.ClipRectangle.Width * AntiAliasLevel;
                    GdiBackbufferHeight = e.ClipRectangle.Height * AntiAliasLevel;
                }

                IntPtr oldBackbufferBitmap = Gdi32.SelectObject(gdiBackbufferDc, GdiBackbufferBitmap);

                OnPaintGdi(new PaintGdiEventArgs(gdiBackbufferDc, e.ClipRectangle.Width, e.ClipRectangle.Height, AntiAliasLevel));

                if (AntiAliasLevel <= 1)
                {
                    Gdi32.BitBlt(dc, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height, gdiBackbufferDc, 0, 0, Gdi32.TernaryRasterOperations.SRCCOPY);
                }
                else
                {
                    var oldMode = Gdi32.SetStretchBltMode(dc, Gdi32.StretchBltMode.STRETCH_HALFTONE);

                    Gdi32.StretchBlt(dc, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height, gdiBackbufferDc, 0, 0, e.ClipRectangle.Width * AntiAliasLevel, e.ClipRectangle.Height * AntiAliasLevel, Gdi32.TernaryRasterOperations.SRCCOPY);

                    Gdi32.SetStretchBltMode(gdiBackbufferDc, oldMode);
                }

                Gdi32.SelectObject(gdiBackbufferDc, oldBackbufferBitmap);
                Gdi32.DeleteObject(gdiBackbufferDc);

                e.Graphics.ReleaseHdc(dc);
            }
            else
            {
                base.OnPaint(e);
                if (PaintDoubleBuffer != null)
                {
                    if (Backbuffer.Width != e.ClipRectangle.Width || Backbuffer.Height != e.ClipRectangle.Height)
                    {
                        Backbuffer = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height, PixelFormat.Format32bppRgb);
                    }

                    using (Graphics g = Graphics.FromImage(Backbuffer))
                    {
                        OnPaintDoubleBuffer(new PaintEventArgs(g, e.ClipRectangle));
                    }

                    e.Graphics.DrawImage(Backbuffer, 0, 0);
                }
            }
        }

        public delegate void PaintGdiEventHandler(object sender, PaintGdiEventArgs e);

        public event PaintGdiEventHandler PaintGdi;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        private new event PaintEventHandler Paint;

        protected virtual void OnPaintGdi(PaintGdiEventArgs e)
        {
            PaintGdiEventHandler handler = PaintGdi;
            handler?.Invoke(this, e);
        }



        /*     protected override void OnMouseDown(MouseEventArgs e)
             {
                 if (e.Button == MouseButtons.Left)
                 {
                     leftMouseButtonDown = true;
                     Invalidate();
                 }
                 base.OnMouseDown(e); 
             }
     
             protected override void OnMouseMove(MouseEventArgs e)
             {
                 if (leftMouseButtonDown)
                 {
                     Invalidate();
                 }
                 base.OnMouseMove(e);
             }
     
             protected override void OnMouseUp(MouseEventArgs e)
             {
                 leftMouseButtonDown = false;
                 Invalidate();
                 base.OnMouseUp(e);
             } */

    }
}
