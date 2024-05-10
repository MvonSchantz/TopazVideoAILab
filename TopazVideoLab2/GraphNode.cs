using System.Globalization;
using System.Security.Cryptography;
using Cybercraft.Common.WinForms;
using Cybercraft.Common.WinForms.Gdi;
using TopazVideoLab.Project;
using TopazVideoLab2.Models;
using VideoProcessingLib.AviSynth;
using static System.Net.Mime.MediaTypeNames;

namespace TopazVideoLab2
{
    public class GraphNode : GraphComponent, ICombination
    {
        private const int BoxWidth = 104;//128;
        private const int BoxHeight = 64;//96;

        int ICombination.Left
        {
            get => MidX;
            set => Left = value - BoxWidth / 2;
        }

        int ICombination.Top
        {
            get => MidY;
            set => Top = value - BoxHeight / 2;
        }

        string ICombination.Id => IsSource ? Project.SourceId : "node-" + (MainForm.GraphNodes.IndexOf(this) + 1).ToInvariant();

        public string PreText =>
            IsSource
                ? MainForm.Project.SourceVideo != null
                    ? Path.GetFileNameWithoutExtension(MainForm.Project.SourceVideo)
                    : ""
                : UpscaleAlgorithm == UpscaleAlgorithm.None 
                    ? MainForm.Project.OutputSize?.ToString() ?? ""
                    : PreprocessingToText(Resize, ResizeAlgorithm, Noise, NoisePreset);

        public string Text => IsSource ? "src" : UpscaleAlgorithmToName(UpscaleAlgorithm, UpscaleFactor);

        public bool IsHighlighted { get; set; }

        public bool IsSelected { get; set; }

        public List<GraphNodeInput> Inputs { get; } = new List<GraphNodeInput>();

        ISource[] ICombination.Sources => Inputs.Cast<ISource>().ToArray();

        public bool IsSource { get; set; }

        public UpscaleAlgorithm UpscaleAlgorithm { get; set; }

        public int UpscaleFactor { get; set; }

        public VideoProcessingLib.AviSynth.Size Resize { get; set; } = new VideoProcessingLib.AviSynth.Size(0, 0);
        
        public ResizeAlgorithm ResizeAlgorithm { get; set; } = ResizeAlgorithm.Spline64;

        public ResizePreset ResizePreset { get; set; }

        public float Noise { get; set; } = 0.0f;

        public NoisePreset NoisePreset { get; set; } = NoisePreset.QtgmcVeryFast;

        public int RevertCompression { get; set; } = 0;
        public int RecoverDetails { get; set; } = 0;
        public int Sharpen { get; set; } = 0;
        public int ReduceNoise { get; set; } = 0;
        public int Dehalo { get; set; } = 0;
        public int AntiAliasDeblur { get; set; } = 0;
        public int OffsetX { get; set; } = 0;
        public int OffsetY { get; set; } = 0;
        public int RecoverOriginalDetails { get; set; } = 20;

        public bool Auto { get; set; } = false;

        public string Name
        {
            get
            {
                string FloatTrim(float f)
                {
                    string str = f.ToString("F2", NumberFormatInfo.InvariantInfo);
                    str = str.TrimEnd('0');
                    str = str.TrimEnd('.');
                    str = str.TrimStart('0');
                    return str;
                }

                if (IsSource)
                {
                    return "s";
                }



                string localName;
                switch (UpscaleAlgorithm)
                {
                    case UpscaleAlgorithm.None:
                        localName = "m";
                        break;
                    default:
                        var model = ModelManager.GetModel(UpscaleAlgorithm);
                        if (model == null)
                        {
                            throw new ArgumentOutOfRangeException();
                        }
                        localName = model.EncodeName(UpscaleFactor, Auto, RevertCompression, RecoverDetails, Sharpen, ReduceNoise, Dehalo, AntiAliasDeblur, RecoverOriginalDetails);
                        break;
                }

                if (Noise > 0.0f)
                {
                    localName = $"noise{FloatTrim(Noise)}" + NoisePreset + localName;
                }

                if (ResizeAlgorithm != ResizeAlgorithm.None)
                {
                    if (ResizeAlgorithm == ResizeAlgorithm.Spline64)
                    {
                        localName = $"{Resize.Width}x{Resize.Height}" + localName;
                    }
                    else
                    {
                        localName = $"{Resize.Width}x{Resize.Height}{ResizeAlgorithm.ToString().ToLowerInvariant()}" + localName;
                    }
                }

                if (Inputs.Count == 0)
                {
                    return localName;
                }

                if (Inputs.Count == 1)
                {
                    return (Inputs[0].Source.Name + "_" + localName).TrimEnd('_');
                }

                if (Inputs.All(i => Math.Abs(i.Weight - Inputs[0].Weight) < 0.001f))
                {
                    return ($"({string.Join("+", Inputs.Select(i => i.Source.Name).OrderBy(n => n))})_{localName}").TrimEnd('_'); ;
                }
                else
                {
                    return ($"({string.Join("+", Inputs.OrderBy(i => i.Source.Name).Select(i => $"{FloatTrim(i.Weight)}x{i.Source.Name}"))})_{localName}").TrimEnd('_'); 
                }
            }
        }

        private static SHA256 sha = SHA256.Create();

        public string HashName {
            get
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(Name);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        public override string ToString() => Name;

        public VideoProcessingLib.AviSynth.Size OutputSize
        {
            get
            {
                if (IsSource)
                {
                    return MainForm.Project.InputSize;
                }

                if (UpscaleAlgorithm == UpscaleAlgorithm.None)
                {
                    return MainForm.Project.OutputSize;
                }

                var size = new VideoProcessingLib.AviSynth.Size(Resize);
                size *= UpscaleFactor;
                return size;
            }
        }

        public GraphNode(MainForm mainForm, int left, int top, bool isSource) : base(mainForm, isSource ? BoxWidth * 2 : BoxWidth, BoxHeight, left, top)
        {
        }

        ISource ICombination.AddSource()
        {
            var source = new GraphNodeInput(null, this);
            Inputs.Add(source);
            return source;
        }

        public static string PreprocessingToText(VideoProcessingLib.AviSynth.Size resize, ResizeAlgorithm algorithm, float noise, NoisePreset noisePreset)
        {
            if (noise > 0.0f)
            {
                return $"{resize}, {algorithm}\nNoise {noise.ToString("F2", NumberFormatInfo.InvariantInfo)}";
            }
            return $"{resize}, {algorithm}";
        }

        public static string UpscaleAlgorithmToName(UpscaleAlgorithm algorithm, int factor)
        {
            switch (algorithm)
            {
                case UpscaleAlgorithm.None:
                    return "Merge";
                default:
                    var model = ModelManager.GetModel(algorithm);
                    if (model == null)
                    {
                        throw new ArgumentOutOfRangeException(nameof(algorithm), algorithm, null);
                    }
                    return $"{model.Name} {factor}x";
            }
        }

        public override void Draw(GraphDc graph)
        {
            graph.Dc.SelectObject(IsSelected ? graph.ComponentSelectedBackgroundBrush : graph.ComponentBackgroundBrush);
            graph.Dc.SelectObject(IsHighlighted ? graph.ComponentHighlightedOutlinePen : graph.ComponentOutlinePen);

            graph.Dc.Rectangle(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);

            graph.Dc.SelectObject(graph.ArrowBoxMainFont);
            graph.Dc.SetBkMode(Gdi32.BackgroundMode.TRANSPARENT);
            if (IsSource && MainForm.Project.SourceVideo != null)
            {
                var area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
                int height = graph.Dc.DrawText(PreText, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_WORDBREAK | Gdi32.DrawTextFormat.DT_CALCRECT);
                area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top + Height / 2 - height / 2, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
                graph.Dc.DrawText(PreText, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_WORDBREAK);
            }
            else
            {
                var area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
                int height = graph.Dc.DrawText(PreText, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_WORDBREAK | Gdi32.DrawTextFormat.DT_CALCRECT);
                area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top + Height / 3 - height / 2, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
                graph.Dc.DrawText(PreText, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_WORDBREAK);

                graph.Dc.SelectObject(graph.ComponentMainFont);
                area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
                height = graph.Dc.DrawText(Text, ref area, Gdi32.DrawTextFormat.DT_CENTER | Gdi32.DrawTextFormat.DT_CALCRECT);
                area = new RECT(graph.OffsetX + Left, graph.OffsetY + Top + 2 * Height / 3 - height / 2, graph.OffsetX + Right + 1, graph.OffsetY + Bottom + 1);
                graph.Dc.DrawText(Text, ref area, Gdi32.DrawTextFormat.DT_CENTER);
            }
        }

        public override void OnMouseDoubleClick()
        {
            if (IsSource)
            {
                MainForm.LoadSource();
            }
        }

        public void Render()
        {
            var renderer = new NodeRenderer(MainForm, this);
            renderer.Render();
        }
    }
}
