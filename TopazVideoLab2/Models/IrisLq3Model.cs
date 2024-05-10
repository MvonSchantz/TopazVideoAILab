using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class IrisLq3Model : ModelBase
    {
        public override string Name => "Iris LQ-3";

        public override string TopazName => "iris-3";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.IrisLq3;

        public override int DefaultFactor => 4;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            if (auto)
            {
                return $"irisLq3Auto{upscaleFactor}x";
            }
            else
            {
                return $"irisLq3-{revertCompression}-{recoverDetails}-{sharpen}-{reduceNoise}-{dehalo}-{antiAliasDeblur}-{recoverOriginalDetails}-{upscaleFactor}x";
            }
        }

        public override bool HasDetailedSettings => true;

        public override bool HasRecoverOriginalSettings => true;
    }
}
