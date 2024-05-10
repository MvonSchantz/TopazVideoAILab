using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class ArtemisHqModel : ModelBase
    {
        public override string Name => "Artemis HQ";

        public override string TopazName => "ahq-12";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.ArtemisHq;

        public override int DefaultFactor => 2;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"ahq{upscaleFactor}x";
        }

        public override bool HasRecoverOriginalSettings => true;
    }
}
