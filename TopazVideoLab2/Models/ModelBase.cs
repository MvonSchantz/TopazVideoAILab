using TopazVideoLab.Project;

namespace TopazVideoLab2.Models
{
    public abstract class ModelBase : IModel
    {
        public abstract string Name { get; }

        public abstract string TopazName { get; }

        public abstract UpscaleAlgorithm UpscaleAlgorithm { get; }

        public abstract int DefaultFactor { get; }

        public abstract string EncodeName(int upscaleFactor, 
            bool auto = false, int revertCompression = -1, int recoverDetails = -1, int sharpen = -1, int reduceNoise = -1, int dehalo = -1, int antiAliasDeblur = -1, int recoverOriginalDetails = -1);

        public virtual bool HasDetailedSettings => false;

        public virtual bool HasOffsetSettings => false;

        public virtual bool HasRecoverOriginalSettings => false;
    }
}
