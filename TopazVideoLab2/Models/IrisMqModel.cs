using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class IrisMqModel : ModelBase
    {
        public override string Name => "Iris MQ-2";

        public override string TopazName => "iris-2";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.IrisMq;

        public override int DefaultFactor => 4;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            if (auto)
            {
                return $"irisMq2Auto{upscaleFactor}x";
            }
            else
            {
                return $"irisMq2-{revertCompression}-{recoverDetails}-{sharpen}-{reduceNoise}-{dehalo}-{antiAliasDeblur}-{recoverOriginalDetails}-{upscaleFactor}x";
            }
        }

        public override bool HasDetailedSettings => true;

        public override bool HasRecoverOriginalSettings => true;
    }
}
