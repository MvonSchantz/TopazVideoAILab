using System.Configuration;
using System.Drawing.Drawing2D;
using Cybercraft.Common;
using TopazVideoLab2.Models;
using VideoProcessingLib.AviSynth;
using Configuration = Cybercraft.Common.Configuration;

namespace TopazVideoLab2
{
    public static class TopazProcess
    {
        // compression = Revert compression
        // details = Recover details
        // blur = Sharpen
        // noise = Reduce noise
        // halo = Dehalo
        // preblur = Anti-Alias/Deblur

        // estimate=20 = Auto
        // blend=0.2 = Recover original detail

        /*public static readonly string Proteus = "prob-3";
        public static readonly string GaiaHq = "ghq-5";
        public static readonly string GaiaCg = "gcg-5";
        public static readonly string Dione = "dtd-4";
        public static readonly string Iris = "iris-1";
        public static readonly string ArtemisHq = "ahq-12";
        public static readonly string ArtemisMq = "amq-13";
        public static readonly string ArtemisLq = "alq-13";*/

        public static void Run(string sourceFile, string targetFolder, string model, int width = 0, int height = 0, 
            bool auto = false, int revertCompression = 0, int recoverDetails = 0, int sharpen = 0, int reduceNoise = 0, int dehalo = 0, int antiAliasDeblur = 0, int recoverOriginalDetails = 20)
        {
            var toolsSection = ConfigurationManager.GetSection("tools") as ToolsSection;
            if (toolsSection == null)
            {
                throw new ConfigurationErrorsException("Missing tools section in App.config.");
            }
            var ffmpegPath = toolsSection.TopazFfmpeg.Path;
            var avs2yuvPath = toolsSection.Avs2Yuv.Path;

            var pathsSection = ConfigurationManager.GetSection("paths") as PathsSection;
            if (pathsSection == null)
            {
                throw new ConfigurationErrorsException("Missing paths section in App.config.");
            }
            var modelDataDir = pathsSection.ModelDataDir.Path;
            var modelDir = pathsSection.ModelDir.Path;

            string scale = width == 0 || height == 0 ? "" : $":scale=0:w={width}:h={height}";

            string autoStr = auto == false ? "" : ":estimate=20";

            string ToFloat(int value)
            {
                switch (value)
                {
                    case -100:
                        return "-1";
                    case 0:
                        return "0";
                    case 100:
                        return "1";
                }

                float f = value / 100.0f;
                string str = f.ToInvariant("F2");
                str = str.TrimEnd('0');
                str = str.TrimEnd('.');
                return str;
            }

            var modelObject = ModelManager.GetModel(model);

            string proteusSettings = !modelObject.HasDetailedSettings ? "" : $":compression={ToFloat(revertCompression)}:details={ToFloat(recoverDetails)}:blur={ToFloat(sharpen)}:noise={ToFloat(reduceNoise)}:halo={ToFloat(dehalo)}:preblur={ToFloat(antiAliasDeblur)}";
            if (auto && modelObject.HasDetailedSettings)
            {
                proteusSettings = ":compression=0:details=0:blur=0:noise=0:halo=0:preblur=0";
            }

            string blendStr = !modelObject.HasRecoverOriginalSettings ? "" : $":blend={ToFloat(recoverOriginalDetails)}";

            /*var process = Process.Run(
                "cmd.exe",
                @$"/C """"{avs2yuvPath}"" -csp I444 ""{sourceFile}"" - | ""{ffmpegPath}"" -hide_banner -f yuv4mpegpipe -i - -flush_packets 1 -sws_flags spline+accurate_rnd+full_chroma_int -color_trc 2 -colorspace 2 -color_primaries 2 -filter_complex tvai_up=model={model}{scale}:device=0:vram=1:instances=1 -c:v png -pix_fmt rgb24 -start_number 0 ""{targetFolder}\%06d.png"" """,
                new Dictionary<string, string>() { { "TVAI_MODEL_DATA_DIR", @"C:\ProgramData\Topaz Labs LLC\Topaz Video AI\models\"}, { "TVAI_MODEL_DIR", @"C:\ProgramData\Topaz Labs LLC\Topaz Video AI\models"}});*/

            var process = Process.Run(
                "cmd.exe",
                @$"/C """"{avs2yuvPath}"" -csp I444 ""{sourceFile}"" - | ""{ffmpegPath}"" -hide_banner -f yuv4mpegpipe -color_trc bt709 -colorspace bt709 -color_primaries bt709 -i - -flush_packets 1 -sws_flags spline+accurate_rnd+full_chroma_int -filter_complex tvai_up=model={model}{autoStr}{proteusSettings}{blendStr}{scale}:device=0:vram=1:instances=1 -c:v ffv1 -coder 1 -context 1 -g 1 -level 3 -threads 16 -slices 4 -slicecrc 1 -pix_fmt bgra -start_number 0 ""{targetFolder}"" """,
                new Dictionary<string, string>() { { "TVAI_MODEL_DATA_DIR", modelDataDir }, { "TVAI_MODEL_DIR", modelDir } }); // -coder 1 -context 1 -g 1 -level 3 -threads 16 -slices 4 -slicecrc 1 -pix_fmt bgr0

            process.WaitForExit();
        }
    }
}
