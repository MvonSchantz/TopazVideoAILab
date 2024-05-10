using Cybercraft.Common.WinForms;
using Cybercraft.Common.WinForms.Gdi;

namespace TopazVideoLab2
{
    public class GraphArrow : GraphComponent
    {
        private const int BoxWidth = 64;
        private const int BoxHeight = 48;

        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }

        public bool HasBox { get; }

        public string Text { get; set; } = "";

        public GraphArrow(MainForm mainForm, int startX, int startY, int endX, int endY, bool hasBox = true) : base(mainForm, BoxWidth, BoxHeight, 0, 0, false)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            HasBox = hasBox;
            CalculateBox();
        }

        private void CalculateBox()
        {
            int midX = (StartX + EndX) / 2;
            int midY = (StartY + EndY) / 2;

            Left = midX - BoxWidth / 2;
            Top = midY - BoxHeight / 2;
        }

        public void MoveStart(int startX, int startY)
        {
            StartX = startX;
            StartY = startY;
            CalculateBox();
        }

        public void MoveEnd(int endX, int endY)
        {
            EndX = endX;
            EndY = endY;
            CalculateBox();
        }

        public override void Draw(GraphDc graph)
        {
            graph.Dc.SelectObject(graph.ArrowPen);
            graph.Dc.MoveToEx(graph.OffsetX + StartX, graph.OffsetY + StartY);
            graph.Dc.LineTo(graph.OffsetX + EndX, graph.OffsetY + EndY);

            if (HasBox)
            {
                graph.Dc.SelectObject(graph.ArrowBoxBackgroundBrush);
                graph.Dc.SelectObject(graph.ArrowBoxOutlinePen);

                graph.Dc.Rectangle(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);

                graph.Dc.SelectObject(graph.ArrowBoxMainFont);
                graph.Dc.SetBkMode(Gdi32.BackgroundMode.TRANSPARENT);
                var area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
                graph.Dc.DrawText(Text, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_VCENTER | Gdi32.DrawTextFormat.DT_SINGLELINE);
            }
        }
    }
}
