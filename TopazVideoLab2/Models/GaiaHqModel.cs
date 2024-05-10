using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class GaiaHqModel : ModelBase
    {
        public override string Name => "Gaia HQ";

        public override string TopazName => "ghq-5";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.GaiaHq;

        public override int DefaultFactor => 2;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"ghq{upscaleFactor}x";
        }
    }
}
