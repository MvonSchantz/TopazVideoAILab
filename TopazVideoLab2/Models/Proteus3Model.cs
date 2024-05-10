using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public class Proteus3Model : ModelBase
    {
        public override string Name => "Proteus-3";

        public override string TopazName => "prob-3";

        public override UpscaleAlgorithm UpscaleAlgorithm => UpscaleAlgorithm.Proteus;

        public override int DefaultFactor => 2;

        public override string EncodeName(int upscaleFactor,
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1)
        {
            if (auto)
            {
                return $"probAuto{upscaleFactor}x";
            }
            else
            {
                return $"prob-{revertCompression}-{recoverDetails}-{sharpen}-{reduceNoise}-{dehalo}-{antiAliasDeblur}-{recoverOriginalDetails}-{upscaleFactor}x";
            }
        }

        public override bool HasDetailedSettings => true;

        public override bool HasOffsetSettings => true;

        public override bool HasRecoverOriginalSettings => true;
    }
}
