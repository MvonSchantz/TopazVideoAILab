using System.Configuration;
using TopazVideoLab.Project;
using TopazVideoLab2.Models;
using VideoProcessingLib.AviSynth;
using VideoProcessingLib.VirtualDub;
using ColorSpace = TopazVideoLab.Project.ColorSpace;

namespace TopazVideoLab2
{
    public class NodeRenderer
    {
        public MainForm MainForm { get; }
        public GraphNode Node { get; }

        private string ProjectFolder { get; }
        private string TempFolder { get; }

        private string OutputFolder { get; }

        private string OutputFile { get; }

        public NodeRenderer(MainForm mainForm, GraphNode node)
        {
            MainForm = mainForm;
            Node = node;
            ProjectFolder = Path.GetDirectoryName(MainForm.Project.ProjectFileName);
            TempFolder = Path.Combine(ProjectFolder, "Temp");
            OutputFolder = Path.Combine(ProjectFolder, "Output");

            if (!Directory.Exists(TempFolder))
            {
                Directory.CreateDirectory(TempFolder);
            }
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            OutputFile = Path.Combine(OutputFolder, $"{mainForm.PreviewFrame}_{mainForm.PreviewLength}_{node.HashName}.avi");
        }

        public void Render()
        {
            if (File.Exists(OutputFile))
            {
                return;
            }

            foreach (var inputNode in Node.Inputs.Where(i => !i.Source.IsSource).Select(i => i.Source))
            {
                var nodeRenderer = new NodeRenderer(MainForm, inputNode);
                nodeRenderer.Render();
            }

            if (Node.UpscaleAlgorithm != UpscaleAlgorithm.None)
            {
                Upscale();
                //Collect();
            }
            else
            {
                Merge();
            }
        }

        private void Merge()
        {
            string scriptFile = Path.Combine(TempFolder, Node.HashName + ".avs");
            using (var script = new Script(scriptFile, 8 * 1024, -1))
            {
                /*script.WriteLine();
                script.WriteLine();
                script.WriteLine("function ShiftR(clip c, int r) {");
                script.WriteLine("\treturn r < 0 ? ShiftL(c, -r) : (r == 0 ? c : StackHorizontal(Blackness(c, width=r), c.Crop(0, 0, -r, 0)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function ShiftL(clip c, int l) {");
                script.WriteLine("\treturn l < 0 ? ShiftR(c, -l) : (l == 0 ? c : StackHorizontal(c.Crop(l, 0, 0, 0), Blackness(c, width=l)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function ShiftD(clip c, int d) {");
                script.WriteLine("\treturn d < 0 ? ShiftU(c, -d) : (d == 0 ? c : StackVertical(Blackness(c, height=d), c.Crop(0, 0, 0, -d)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function ShiftU(clip c, int u) {");
                script.WriteLine("\treturn u < 0 ? ShiftD(c, -u) : (u == 0 ? c : StackVertical(c.Crop(0, u, 0, 0), Blackness(c, height=u)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function Shift(clip c, int x, int y) {");
                script.WriteLine("\treturn c.ShiftR(x).ShiftD(y)");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function Offset(clip c, int x, int y) {");
                script.WriteLine("\treturn c.BilinearResize(c.width * 2, c.height * 2).Shift(x, y).Spline64Resize(c.width, c.height)");
                script.WriteLine("}");
                script.WriteLine();*/

                for (var i = 0; i < Node.Inputs.Count; i++)
                {
                    var input = Node.Inputs[i];
                    if (Node.Inputs.Count > 1)
                    {
                        script.Writer.Write($"src{i + 1} = ");
                    }

                    if (input.Source.IsSource)
                    {
                        script.Output.Source(MainForm.Project.SourceVideo, true, false);
                        switch (MainForm.Project.Interlace)
                        {
                            case Interlace.IVTC:
                                script.Write(@".AssumeTFF().TFM().TDecimate()");
                                break;
                        }
                        script.Write($".Trim({MainForm.PreviewFrame},-{MainForm.PreviewLength})");
                        
                        if (MainForm.Project.ColorSpace == ColorSpace.Rec601)
                        {
                            script.Write(@".ConvertToYUV444(matrix=""Rec601"").ConvertToRGB48(matrix=""Rec601"")");
                        }
                        else
                        {
                            script.Write(@".ConvertToYUV444(matrix=""Rec709"").ConvertToRGB48(matrix=""Rec709"")");
                        }
                    }
                    else
                    {
                        script.Write($@"LWLibavVideoSource(""{Path.Combine(OutputFolder, $"{MainForm.PreviewFrame}_{MainForm.PreviewLength}_{input.Source.HashName}.avi")}"").KillAudio()");
                        script.Write(@".ConvertToRGB48(matrix=""Rec709"")");
                        if (input.Source.UpscaleAlgorithm == UpscaleAlgorithm.Proteus && (input.Source.OffsetX != 0 || input.Source.OffsetY != 0))
                        {
                            script.Write($".Offset({input.Source.OffsetX}, {input.Source.OffsetY})");
                        }
                    }

                    script.Write($".Spline64Resize({MainForm.Project.OutputSize.AvsString})");

                    script.WriteLine();
                }
                script.WriteLine();

                if (Node.Inputs.Count > 1)
                {
                    string layer = null;
                    for (var i = 1; i < Node.Inputs.Count; i++)
                    {
                        var input = Node.Inputs[i];
                        float sumWeights = Node.Inputs.Take(i + 1).Sum(ip => ip.Weight);
                        float opacity = input.Weight / sumWeights;
                        if (layer == null)
                        {
                            layer = $"Layer(src1,src2,opacity={opacity.ToInvariant("F4")})";
                        }
                        else
                        {
                            layer = $"Layer({layer},src{i + 1},opacity={opacity.ToInvariant("F4")})";
                        }
                    }

                    script.WriteLine(layer);
                }
                script.WriteLine("ConvertToRGB24()");
                script.WriteLine();
            }

            var toolsSection = ConfigurationManager.GetSection("tools") as ToolsSection;
            if (toolsSection == null)
                throw new ConfigurationErrorsException("Missing tools section in App.config.");
            VirtualDubProcess.SetPath64(toolsSection.VirtualDub64.Path);

            VirtualDubProcess.RunAvisynth(scriptFile, OutputFile, VideoProcessingLib.AviSynth.ColorSpace.RGB);
        }

        private void Upscale()
        {
            string scriptFile = Path.Combine(TempFolder, Node.HashName + ".avs");
            using (var script = new Script(scriptFile, 8 * 1024, 0))
            {
                script.WriteLine();
                script.WriteLine("function Noise(clip source, float noise, string preset) {");
                script.WriteLine($"\treturn source.QTGMC(Preset=preset, EdiThreads={Cybercraft.Common.Information.System.HardwareCores}, NoiseProcess=2, InputType=1, GrainRestore=noise, NoiseRestore=noise)");
                script.WriteLine("}");
                script.WriteLine();
                /*script.WriteLine("function ShiftR(clip c, int r) {");
                script.WriteLine("\treturn r < 0 ? ShiftL(c, -r) : (r == 0 ? c : StackHorizontal(Blackness(c, width=r), c.Crop(0, 0, -r, 0)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function ShiftL(clip c, int l) {");
                script.WriteLine("\treturn l < 0 ? ShiftR(c, -l) : (l == 0 ? c : StackHorizontal(c.Crop(l, 0, 0, 0), Blackness(c, width=l)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function ShiftD(clip c, int d) {");
                script.WriteLine("\treturn d < 0 ? ShiftU(c, -d) : (d == 0 ? c : StackVertical(Blackness(c, height=d), c.Crop(0, 0, 0, -d)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function ShiftU(clip c, int u) {");
                script.WriteLine("\treturn u < 0 ? ShiftD(c, -u) : (u == 0 ? c : StackVertical(c.Crop(0, u, 0, 0), Blackness(c, height=u)))");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function Shift(clip c, int x, int y) {");
                script.WriteLine("\treturn c.ShiftR(x).ShiftD(y)");
                script.WriteLine("}");
                script.WriteLine();
                script.WriteLine("function Offset(clip c, int x, int y) {");
                script.WriteLine("\treturn c.BilinearResize(c.width * 2, c.height * 2).Shift(x, y).Spline64Resize(c.width, c.height)");
                script.WriteLine("}");
                script.WriteLine();*/

                for (var i = 0; i < Node.Inputs.Count; i++)
                {
                    var input = Node.Inputs[i];
                    if (Node.Inputs.Count > 1)
                    {
                        script.Writer.Write($"src{i + 1} = ");
                    }

                    if (input.Source.IsSource)
                    {
                        script.Output.Source(MainForm.Project.SourceVideo, true, false);
                        switch (MainForm.Project.Interlace)
                        {
                            case Interlace.IVTC:
                                script.Write(@".AssumeTFF().TFM().TDecimate()");
                                break;
                        }
                        script.Write($".Trim({MainForm.PreviewFrame},-{MainForm.PreviewLength})");
                        
                        if (MainForm.Project.ColorSpace == ColorSpace.Rec601)
                        {
                            script.Write(@".ConvertToYUV444(matrix=""Rec601"").ConvertBits(16).ConvertToRGB(matrix=""Rec601"").ConvertToYUV444(matrix=""Rec709"")");
                        }
                        else
                        {
                            script.Write(@".ConvertToYUV444(matrix=""Rec709"").ConvertBits(16)");
                        }
                    }
                    else
                    {
                        script.Write($@"LWLibavVideoSource(""{Path.Combine(OutputFolder, $"{MainForm.PreviewFrame}_{MainForm.PreviewLength}_{input.Source.HashName}.avi")}"").KillAudio()");
                        script.Write(@".ConvertBits(16).ConvertToYUV444(matrix=""Rec709"")");
                        if (input.Source.UpscaleAlgorithm == UpscaleAlgorithm.Proteus && (input.Source.OffsetX != 0 || input.Source.OffsetY != 0))
                        {
                            script.Write($".Offset({input.Source.OffsetX}, {input.Source.OffsetY})");
                        }
                    }

                    switch (Node.ResizeAlgorithm)
                    {
                        case ResizeAlgorithm.None:
                            break;
                        case ResizeAlgorithm.Spline64:
                            script.Write($".Spline64Resize({Node.Resize.AvsString})");
                            break;
                        case ResizeAlgorithm.Lanczos:
                            script.Write($".LanczosResize({Node.Resize.AvsString})");
                            break;
                        case ResizeAlgorithm.Bicubic:
                            script.Write($".BicubicResize({Node.Resize.AvsString})");
                            break;
                        case ResizeAlgorithm.Bilinear:
                            script.Write($".BilinearResize({Node.Resize.AvsString})");
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    if (Node.Noise > 0.0f)
                    {
                        switch (Node.NoisePreset)
                        {
                            case NoisePreset.QtgmcPlacebo:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Placebo\")");
                                break;
                            case NoisePreset.QtgmcVerySlow:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Very Slow\")");
                                break;
                            case NoisePreset.QtgmcSlower:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Slower\")");
                                break;
                            case NoisePreset.QtgmcSlow:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Slow\")");
                                break;
                            case NoisePreset.QtgmcMedium:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Medium\")");
                                break;
                            case NoisePreset.QtgmcFast:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Fast\")");
                                break;
                            case NoisePreset.QtgmcFaster:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Faster\")");
                                break;
                            case NoisePreset.QtgmcVeryFast:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Very Fast\")");
                                break;
                            case NoisePreset.QtgmcSuperFast:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Super Fast\")");
                                break;
                            case NoisePreset.QtgmcUltraFast:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Ultra Fast\")");
                                break;
                            case NoisePreset.QtgmcDraft:
                                script.Write($".Noise({Node.Noise.ToInvariant("F2")}, \"Draft\")");
                                break;
                            case NoisePreset.FilmGrain005:
                                script.Write($".FilmGrainFast({Node.Noise.ToInvariant("F2")}, grainsize=0.05)");
                                break;
                            case NoisePreset.FilmGrain010:
                                script.Write($".FilmGrainFast({Node.Noise.ToInvariant("F2")}, grainsize=0.1)");
                                break;
                            case NoisePreset.FilmGrain015:
                                script.Write($".FilmGrainFast({Node.Noise.ToInvariant("F2")}, grainsize=0.15)");
                                break;
                            case NoisePreset.FilmGrain020:
                                script.Write($".FilmGrainFast({Node.Noise.ToInvariant("F2")}, grainsize=0.2)");
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    
                    script.WriteLine();
                }
                script.WriteLine();

                if (Node.Inputs.Count > 1)
                {
                    string layer = null;
                    for (var i = 1; i < Node.Inputs.Count; i++)
                    {
                        var input = Node.Inputs[i];
                        float sumWeights = Node.Inputs.Take(i + 1).Sum(ip => ip.Weight);
                        float opacity = input.Weight / sumWeights;
                        if (layer == null)
                        {
                            layer = $"Layer(src1,src2,opacity={opacity.ToInvariant("F4")})";
                        }
                        else
                        {
                            layer = $"Layer({layer},src{i + 1},opacity={opacity.ToInvariant("F4")})";
                        }
                    }

                    script.WriteLine(layer);
                }
            }

            int width = Node.UpscaleFactor == 1 ? 0 : Node.OutputSize.Width;
            int height = Node.UpscaleFactor == 1 ? 0 : Node.OutputSize.Height;

            var model = ModelManager.GetModel(Node.UpscaleAlgorithm);
            if (model == null)
            {
                throw new ArgumentOutOfRangeException();
            }
            TopazProcess.Run(scriptFile, OutputFile, model.TopazName, width, height, Node.Auto, Node.RevertCompression, Node.RecoverDetails, Node.Sharpen, Node.ReduceNoise, Node.Dehalo, Node.AntiAliasDeblur, Node.RecoverOriginalDetails);

            /*switch (Node.UpscaleAlgorithm)
            {
                case UpscaleAlgorithm.GaiaHq:
                    TopazProcess.Run(scriptFile, OutputFile, TopazProcess.GaiaHq, width, height);
                    break;
                case UpscaleAlgorithm.GaiaCg:
                    TopazProcess.Run(scriptFile, OutputFile, TopazProcess.GaiaCg, width, height);
                    break;
                case UpscaleAlgorithm.Dione:
                    TopazProcess.Run(scriptFile, OutputFile, TopazProcess.Dione, width, height);
                    break;
                case UpscaleAlgorithm.Proteus:
                    if (Node.Auto)
                    {
                        TopazProcess.Run(scriptFile, OutputFile, TopazProcess.Proteus, width, height, true, recoverOriginalDetails:Node.RecoverOriginalDetails);
                    }
                    else
                    {
                        TopazProcess.Run(scriptFile, OutputFile, TopazProcess.Proteus, width, height, false, Node.RevertCompression, Node.RecoverDetails, Node.Sharpen, Node.ReduceNoise, Node.Dehalo, Node.AntiAliasDeblur, Node.RecoverOriginalDetails);
                    }
                    break;
                case UpscaleAlgorithm.Iris:
                    if (Node.Auto)
                    {
                        TopazProcess.Run(scriptFile, OutputFile, TopazProcess.Iris, width, height, true, recoverOriginalDetails: Node.RecoverOriginalDetails);
                    }
                    else
                    {
                        TopazProcess.Run(scriptFile, OutputFile, TopazProcess.Iris, width, height, false, Node.RevertCompression, Node.RecoverDetails, Node.Sharpen, Node.ReduceNoise, Node.Dehalo, Node.AntiAliasDeblur, Node.RecoverOriginalDetails);
                    }
                    break;
                case UpscaleAlgorithm.ArtemisHq:
                    TopazProcess.Run(scriptFile, OutputFile, TopazProcess.ArtemisHq, width, height, recoverOriginalDetails: Node.RecoverOriginalDetails);
                    break;
                case UpscaleAlgorithm.ArtemisMq:
                    TopazProcess.Run(scriptFile, OutputFile, TopazProcess.ArtemisMq, width, height, recoverOriginalDetails: Node.RecoverOriginalDetails);
                    break;
                case UpscaleAlgorithm.ArtemisLq:
                    TopazProcess.Run(scriptFile, OutputFile, TopazProcess.ArtemisLq, width, height, recoverOriginalDetails: Node.RecoverOriginalDetails);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }*/

            
        }

        public void Collect()
        {
            var files = Directory.GetFiles(TempFolder, "*.png");
            int maxFile = files.Select(f => int.Parse(Path.GetFileNameWithoutExtension(f))).Max();

            string scriptFile = Path.Combine(TempFolder, Node.HashName + ".avs");
            using (var script = new Script(scriptFile, 8 * 1024, -1))
            {
                script.WriteLine($"ImageSource(\"{TempFolder}\\%06d.png\", 0, {maxFile}, {MainForm.Project.Framerate.ToInvariant("F3")}, true, false, \"RGB24\")");
            }

            var toolsSection = ConfigurationManager.GetSection("tools") as ToolsSection;
            if (toolsSection == null)
                throw new ConfigurationErrorsException("Missing tools section in App.config.");
            VirtualDubProcess.SetPath64(toolsSection.VirtualDub64.Path);

            VirtualDubProcess.RunAvisynth(scriptFile, OutputFile, VideoProcessingLib.AviSynth.ColorSpace.RGB);

            foreach (var file in files)
            {
                File.Delete(file);
            }
        }
    }
}

