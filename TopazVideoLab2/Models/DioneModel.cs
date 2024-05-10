using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class DioneModel : ModelBase
    {
        public override string Name => "Dione";

        public override string TopazName => "dtd-4";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.Dione;

        public override int DefaultFactor => 4;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"dtd{upscaleFactor}x";
        }
    }
}
