using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class GaiaCgModel : ModelBase
    {
        public override string Name => "Gaia CG";

        public override string TopazName => "gcg-5";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.GaiaCg;

        public override int DefaultFactor => 2;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"gcg{upscaleFactor}x";
        }
    }
}
