using System.PInvoke.Windows.Structs;
using Cybercraft.Common.WinForms.Gdi;

namespace TopazVideoLab2
{
    public class GraphDc : IDisposable
    {
        public int OffsetX { get; }
        public int OffsetY { get; }
        
        public GdiDc Dc { get; }

        public GdiBrush ComponentBackgroundBrush { get; }
        public GdiBrush ComponentSelectedBackgroundBrush { get; }
        public GdiPen ComponentOutlinePen { get; }
        public GdiPen ComponentHighlightedOutlinePen { get; }
        public GdiFont ComponentMainFont { get; }

        public GdiPen ArrowPen { get; }
        public GdiBrush ArrowBoxBackgroundBrush { get; }
        public GdiBrush ArrowBoxSelectedBackgroundBrush { get; }
        public GdiPen ArrowBoxOutlinePen { get; }
        public GdiFont ArrowBoxMainFont { get; }

        public GraphDc(GdiDc dc, int offsetX, int offsetY)
        {
            Dc = dc;
            OffsetX = offsetX;
            OffsetY = offsetY;

            ComponentBackgroundBrush = new GdiBrush(Color.LightGray);
            ComponentSelectedBackgroundBrush = new GdiBrush(Color.White);
            ComponentOutlinePen = new GdiPen(2, Color.Black);
            ComponentHighlightedOutlinePen = new GdiPen(2, Color.Blue);
            ComponentMainFont = new GdiFont(new Font("Arial", 10.5f, FontStyle.Bold));

            ArrowPen = new GdiPen(2, Color.Black);
            ArrowBoxBackgroundBrush = new GdiBrush(Color.LightGray);
            ArrowBoxSelectedBackgroundBrush = new GdiBrush(Color.White);
            ArrowBoxOutlinePen = new GdiPen(1, Color.Black);
            ArrowBoxMainFont = new GdiFont(new Font("Arial", 8.5f, FontStyle.Regular));
        }

        public void Dispose()
        {
            ComponentBackgroundBrush.Dispose();
            ComponentSelectedBackgroundBrush.Dispose();
            ComponentOutlinePen.Dispose();
            ComponentHighlightedOutlinePen.Dispose();
            ComponentMainFont.Dispose();

            ArrowPen.Dispose();
            ArrowBoxBackgroundBrush.Dispose();
            ArrowBoxSelectedBackgroundBrush.Dispose();
            ArrowBoxOutlinePen.Dispose();
            ArrowBoxMainFont.Dispose();
        }
    }
}
