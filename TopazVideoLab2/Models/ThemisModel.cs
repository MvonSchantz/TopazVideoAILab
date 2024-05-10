using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class ThemisModel : ModelBase
    {
        public override string Name => "Themis-2";

        public override string TopazName => "thm-2";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.Themis2;

        public override int DefaultFactor => 1;

        public override string EncodeName(int upscaleFactor, 
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"thm2_{upscaleFactor}x";
        }

    }
}
