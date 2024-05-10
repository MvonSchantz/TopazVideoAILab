using System.Diagnostics;
using System.Globalization;
using Cybercraft.Common.WinForms;
using Cybercraft.Common.WinForms.Gdi;
using TopazVideoLab.Project;
using VideoProcessingLib.AviSynth;

namespace TopazVideoLab2
{
    public class GraphNodeInput : GraphComponent, ISource
    {
        private const int BoxWidth = 72;
        private const int BoxHeight = 34;

        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int EndX { get; private set; }
        public int EndY { get; private set; }

        public string Text => string.Join("\n", new string[] { SizeToString(), "Weight: " + Weight.ToInvariant("F2") }.Where(s => !string.IsNullOrWhiteSpace(s)));

        public bool IsSelected { get; set; }


        private GraphNode source;
        public GraphNode Source
        {
            get => source;
            private set
            {
                if (value != null && value == Target)
                {
                    Debugger.Break();
                }
                source = value;
            }
        }

        ICombination ISource.Source
        {
            get => Source;
            set => Source = (GraphNode)value;
        }

        public GraphNode Target { get; }

        public float Weight { get; set; } = 1.0f;

        /*public Cybercraft.Common.WinForms.Size Resize { get; set; } = new Cybercraft.Common.WinForms.Size(0, 0);

        public ResizeAlgorithm ResizeAlgorithm { get; set; } = ResizeAlgorithm.Spline64;

        public float Noise { get; set; } = 0.0f;*/


        public GraphNodeInput(GraphNode source, GraphNode target) : base(target.MainForm, BoxWidth, BoxHeight, 0, 0, false)
        {
            if (source == target)
            {
                Debugger.Break();
            }

            Source = source;
            Target = target;
            if (source != null)
            {
                StartX = (source.Left + source.Right) / 2;
                StartY = source.Bottom;
            }
            EndX = (target.Left + target.Right) / 2;
            EndY = target.Top;
            CalculateBox();
        }

        public static string NodeInputToText(Cybercraft.Common.WinForms.Size resize, ResizeAlgorithm algorithm, float noise)
        {
            return $"{resize}, {algorithm}\nNoise {noise.ToString("F2", NumberFormatInfo.InvariantInfo)}";
        }

        public string SizeToString()
        {
            if (Source.OutputSize != null)
            {
                return Source.OutputSize.ToString();
            }

            return "";
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
            
            graph.Dc.SelectObject(IsSelected ? graph.ArrowBoxSelectedBackgroundBrush : graph.ArrowBoxBackgroundBrush);
            graph.Dc.SelectObject(graph.ArrowBoxOutlinePen);

            graph.Dc.Rectangle(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);

            graph.Dc.SelectObject(graph.ArrowBoxMainFont);
            graph.Dc.SetBkMode(Gdi32.BackgroundMode.TRANSPARENT);
            var area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
            int height = graph.Dc.DrawText(Text, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_WORDBREAK | Gdi32.DrawTextFormat.DT_CALCRECT);
            area = new RECT(graph.OffsetX + Left, (graph.OffsetY + Top + graph.OffsetY + Bottom + 1 - height) / 2, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
            graph.Dc.DrawText(Text, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_WORDBREAK);
        }
    }
}
