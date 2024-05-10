using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class Nyx2Model : ModelBase
    {
        public override string Name => "Nyx-2";

        public override string TopazName => "nyx-2";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.Nyx2;

        public override int DefaultFactor => 1;

        public override string EncodeName(int upscaleFactor, 
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"nyx2_{upscaleFactor}x";
        }

        public override bool HasDetailedSettings => true;
    }
}
