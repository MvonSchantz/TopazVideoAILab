using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class ArtemisMqModel : ModelBase
    {
        public override string Name => "Artemis MQ";

        public override string TopazName => "amq-13";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.ArtemisMq;

        public override int DefaultFactor => 2;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            return $"amq{upscaleFactor}x";
        }

        public override bool HasRecoverOriginalSettings => true;
    }
}
