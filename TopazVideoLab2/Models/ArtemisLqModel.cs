using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class ArtemisLqModel : ModelBase
    {
        public override string Name => "Artemis LQ";

        public override string TopazName => "alq-13";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.ArtemisLq;

        public override int DefaultFactor => 2;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"alq{upscaleFactor}x";
        }

        public override bool HasRecoverOriginalSettings => true;
    }
}
