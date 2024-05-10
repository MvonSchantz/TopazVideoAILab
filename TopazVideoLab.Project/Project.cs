
using System.Collections.Generic;
using System.Linq;
using Pri.LongPath;
using VideoProcessingLib.AviSynth;

namespace TopazVideoLab.Project
{
    public class Project
    {
        public static readonly string SourceId = "source";

        public string ProjectFileName { get; set; }

        public string SourceVideo { get; set; }

        public int Frames { get; set; }

        public AspectRatio AspectRatio { get; set; }
        public ColorSpace ColorSpace { get; set; }
        public Interlace Interlace { get; set; }
        public Size InputSize { get; set; }
        public Size OutputSize { get; set; }

        public float Framerate { get; set; }

        public Project()
        {

        }

        public void Clear()
        {
            ProjectFileName = null;
            SourceVideo = null;
            Frames = 0;
            InputSize = new Size(0, 0);
            OutputSize = new Size(0, 0);
            Framerate = 0.0f;
            AspectRatio = AspectRatio.Ratio4by3;
        }

        public TopazVideoLabProject SaveToXml(IMainForm mainForm)
        {
            var xml = new TopazVideoLabProject();

            var version = mainForm.Version;
            xml.MajorVersion = version.Major;
            xml.MinorVersion = version.Minor;
            xml.Build = version.Build;

            var sourceCombination = mainForm.Combinations.FirstOrDefault(c => c.IsSource);

            xml.SourceVideo.Value = SourceVideo;
            xml.SourceVideo.Id = SourceId;
            xml.SourceVideo.Left = sourceCombination?.Left ?? 0;
            xml.SourceVideo.Top = sourceCombination?.Top ?? 0;
            xml.SourceVideo.ColorSpace = ColorSpace;
            xml.SourceVideo.Interlace = Interlace;

            xml.OutputSize.Width = OutputSize?.Width ?? 0;
            xml.OutputSize.Height = OutputSize?.Height ?? 0;
            xml.OutputSize.Ratio = AspectRatio;

            xml.Combination = new List<CombinationType>();

            for (var i = 0; i < mainForm.Combinations.Length; i++)
            {
                var combination = mainForm.Combinations[i];
                if (combination.IsSource)
                {
                    continue;
                }

                var model = mainForm.ModelManager.GetModel(combination.UpscaleAlgorithm);

                var xmlCombination = new CombinationType();

                xmlCombination.Left = combination.Left;
                xmlCombination.Top = combination.Top;
                xmlCombination.Id = combination.Id;

                if (combination.UpscaleAlgorithm != UpscaleAlgorithm.None)
                {
                    xmlCombination.Resize = new ResizeType
                    {
                        Width = combination.Resize.Width,
                        Height = combination.Resize.Height,
                        Algorithm = combination.ResizeAlgorithm,
                        Preset = combination.ResizePreset
                    };
                    xmlCombination.Noise = combination.Noise;
                    xmlCombination.NoiseSpecified = true;
                    xmlCombination.NoisePreset = combination.NoisePreset;
                    xmlCombination.NoisePresetSpecified = true;
                }
                else
                {
                    xmlCombination.Resize = null;
                    xmlCombination.NoiseSpecified = false;
                }

                xmlCombination.Upscale.Algorithm = combination.UpscaleAlgorithm;
                xmlCombination.Upscale.Factor = combination.UpscaleFactor;

                if (model != null && model.HasOffsetSettings)
                //if (combination.UpscaleAlgorithm == UpscaleAlgorithm.Proteus)
                {
                    if (combination.OffsetX != 0)
                    {
                        xmlCombination.Upscale.OffsetX = combination.OffsetX;
                        xmlCombination.Upscale.OffsetXSpecified = true;
                    }

                    if (combination.OffsetY != 0)
                    {
                        xmlCombination.Upscale.OffsetY = combination.OffsetY;
                        xmlCombination.Upscale.OffsetYSpecified = true;
                    }
                }

                if (model != null && model.HasDetailedSettings)
                //if (combination.UpscaleAlgorithm == UpscaleAlgorithm.Proteus || combination.UpscaleAlgorithm == UpscaleAlgorithm.Iris || combination.UpscaleAlgorithm == UpscaleAlgorithm.IrisMq)
                {
                    if (combination.Auto)
                    {
                        xmlCombination.Upscale.Auto = combination.Auto;
                        xmlCombination.Upscale.AutoSpecified = true;
                    }
                    else
                    {
                        xmlCombination.Upscale.RevertCompression = combination.RevertCompression;
                        xmlCombination.Upscale.RevertCompressionSpecified = true;
                        xmlCombination.Upscale.RecoverDetails = combination.RecoverDetails;
                        xmlCombination.Upscale.RecoverDetailsSpecified = true;
                        xmlCombination.Upscale.Sharpen = combination.Sharpen;
                        xmlCombination.Upscale.SharpenSpecified = true;
                        xmlCombination.Upscale.ReduceNoise = combination.ReduceNoise;
                        xmlCombination.Upscale.ReduceNoiseSpecified = true;
                        xmlCombination.Upscale.Dehalo = combination.Dehalo;
                        xmlCombination.Upscale.DehaloSpecified = true;
                        xmlCombination.Upscale.AntiAliasDeblur = combination.AntiAliasDeblur;
                        xmlCombination.Upscale.AntiAliasDeblurSpecified = true;
                    }
                }

                if (model != null && model.HasRecoverOriginalSettings)
                //if (combination.UpscaleAlgorithm == UpscaleAlgorithm.Proteus || combination.UpscaleAlgorithm == UpscaleAlgorithm.Iris || combination.UpscaleAlgorithm == UpscaleAlgorithm.IrisMq ||
                //    combination.UpscaleAlgorithm == UpscaleAlgorithm.ArtemisHq || combination.UpscaleAlgorithm == UpscaleAlgorithm.ArtemisMq || combination.UpscaleAlgorithm == UpscaleAlgorithm.ArtemisLq)
                {
                    xmlCombination.Upscale.Blend = combination.RecoverOriginalDetails;
                    xmlCombination.Upscale.BlendSpecified = true;
                }

                foreach (var source in combination.Sources)
                {
                    var xmlSource = new SourceType();
                    xmlSource.Weight = source.Weight;
                    xmlSource.Id = source.Source.Id;

                    xmlCombination.Source.Add(xmlSource);
                }

                xml.Combination.Add(xmlCombination);
            }

            xml.Preview.Frame = mainForm.PreviewFrame;
            xml.Preview.Length = mainForm.PreviewLength;

            return xml;
        }

        public void LoadFromXml(IMainForm mainForm, TopazVideoLabProject xml)
        {
            string projectFileName = ProjectFileName;
            Clear();
            ProjectFileName = projectFileName;

            SourceVideo = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(ProjectFileName), xml.SourceVideo.Value));

            OutputSize = new Size(xml.OutputSize.Width, xml.OutputSize.Height);
            AspectRatio = xml.OutputSize.Ratio;
            ColorSpace = xml.SourceVideo.ColorSpace;
            Interlace = xml.SourceVideo.Interlace;

            var source = mainForm.AddCombination(true);
            source.Left = xml.SourceVideo.Left;
            source.Top = xml.SourceVideo.Top;
            source.IsSource = true;

            Dictionary<string, ICombination> IdToCombination = new Dictionary<string, ICombination>();
            IdToCombination.Add(xml.SourceVideo.Id, source);

            var sources = new List<(ISource Source, string SourceId)>();

            foreach (var xmlCombination in xml.Combination)
            {
                var combination = mainForm.AddCombination(false);
                IdToCombination.Add(xmlCombination.Id, combination);

                combination.Left = xmlCombination.Left;
                combination.Top = xmlCombination.Top;

                combination.UpscaleAlgorithm = xmlCombination.Upscale.Algorithm;
                combination.UpscaleFactor = xmlCombination.Upscale.Factor;

                var model = mainForm.ModelManager.GetModel(combination.UpscaleAlgorithm);

                if (combination.UpscaleAlgorithm != UpscaleAlgorithm.None)
                {
                    combination.Resize.Width = xmlCombination.Resize.Width;
                    combination.Resize.Height = xmlCombination.Resize.Height;
                    combination.ResizeAlgorithm = xmlCombination.Resize.Algorithm;
                    combination.ResizePreset = xmlCombination.Resize.Preset;
                    combination.Noise = xmlCombination.Noise;
                    combination.NoisePreset = xmlCombination.NoisePresetSpecified ? xmlCombination.NoisePreset : NoisePreset.QtgmcVerySlow;
                }

                if (model != null && model.HasDetailedSettings)
                //if (combination.UpscaleAlgorithm == UpscaleAlgorithm.Proteus || combination.UpscaleAlgorithm == UpscaleAlgorithm.Iris || combination.UpscaleAlgorithm == UpscaleAlgorithm.IrisMq)
                {
                    combination.RevertCompression = xmlCombination.Upscale.RevertCompression;
                    combination.RecoverDetails = xmlCombination.Upscale.RecoverDetails;
                    combination.Sharpen = xmlCombination.Upscale.Sharpen;
                    combination.ReduceNoise = xmlCombination.Upscale.ReduceNoise;
                    combination.Dehalo = xmlCombination.Upscale.Dehalo;
                    combination.AntiAliasDeblur = xmlCombination.Upscale.AntiAliasDeblur;
                    combination.Auto = xmlCombination.Upscale.Auto;
                }

                if (model != null && model.HasOffsetSettings)
                //if (combination.UpscaleAlgorithm == UpscaleAlgorithm.Proteus)
                {
                    combination.OffsetX = xmlCombination.Upscale.OffsetX;
                    combination.OffsetY = xmlCombination.Upscale.OffsetY;
                }

                if (model != null && model.HasRecoverOriginalSettings)
                //if (combination.UpscaleAlgorithm == UpscaleAlgorithm.Proteus || combination.UpscaleAlgorithm == UpscaleAlgorithm.Iris || combination.UpscaleAlgorithm == UpscaleAlgorithm.IrisMq ||
                //    combination.UpscaleAlgorithm == UpscaleAlgorithm.ArtemisHq || combination.UpscaleAlgorithm == UpscaleAlgorithm.ArtemisMq || combination.UpscaleAlgorithm == UpscaleAlgorithm.ArtemisLq)
                {
                    combination.RecoverOriginalDetails = xmlCombination.Upscale.Blend;
                }

                foreach (var xmlSource in xmlCombination.Source)
                {
                    var inputSource = combination.AddSource();
                    inputSource.Weight = xmlSource.Weight;
                    sources.Add((inputSource, xmlSource.Id));
                }
            }

            foreach (var inputSource in sources)
            {
                var sourceCombination = IdToCombination[inputSource.SourceId];
                inputSource.Source.Source = sourceCombination;
            }

            mainForm.PreviewFrame = xml.Preview.Frame;
            mainForm.PreviewLength = xml.Preview.Length;
        }
    }
}


