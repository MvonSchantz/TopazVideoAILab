namespace TopazVideoLab.Project
{
    public interface IModel
    {
        string Name { get; }

        string TopazName { get; }

        UpscaleAlgorithm UpscaleAlgorithm { get; }

        bool HasDetailedSettings { get; }

        bool HasOffsetSettings { get; }
    
        bool HasRecoverOriginalSettings { get; }
    }
}
