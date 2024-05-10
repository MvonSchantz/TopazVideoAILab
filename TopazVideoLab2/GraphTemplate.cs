using Cybercraft.Common.WinForms;
using Cybercraft.Common.WinForms.Gdi;
using TopazVideoLab.Project;

namespace TopazVideoLab2
{
    public class GraphTemplate : GraphComponent
    {
        public string Text => IsSource ? "src" : GraphNode.UpscaleAlgorithmToName(UpscaleAlgorithm, UpscaleFactor);

        public bool IsSelected { get; set; }

        public bool IsSource { get; set; }

        public UpscaleAlgorithm UpscaleAlgorithm { get; set; }

        public int UpscaleFactor { get; set; }

        public GraphTemplate(MainForm mainForm, int left, int top) : base(mainForm, 96+24, 64, left, top)
        {
        }

        public override void Draw(GraphDc graph)
        {
            graph.Dc.SelectObject(IsSelected ? graph.ComponentSelectedBackgroundBrush : graph.ComponentBackgroundBrush);
            graph.Dc.SelectObject(graph.ComponentOutlinePen);
            graph.Dc.Rectangle(Left, Top, Right + 1, Bottom + 1);

            graph.Dc.SelectObject(graph.ComponentMainFont);
            graph.Dc.SetBkMode(Gdi32.BackgroundMode.TRANSPARENT);
            var area = new RECT(Left, Top, Right + 1, Bottom + 1);
            graph.Dc.DrawText(Text, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_VCENTER | Gdi32.DrawTextFormat.DT_SINGLELINE);
        }
    }
}
