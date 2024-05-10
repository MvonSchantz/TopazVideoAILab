namespace TopazVideoLab.Project
{
    public interface ICombination
    {
        int Left { get; set; }
        int Top { get; set; }

        string Id { get; }

        bool IsSource { get; set; }

        ISource[] Sources { get; }

        ISource AddSource();

        VideoProcessingLib.AviSynth.Size Resize { get; }

        ResizeAlgorithm ResizeAlgorithm { get; set; }
        ResizePreset ResizePreset { get; set; }

        float Noise { get; set; }

        NoisePreset NoisePreset { get; set; }

        UpscaleAlgorithm UpscaleAlgorithm { get; set; }
        int UpscaleFactor { get; set; }

        int RevertCompression { get; set; }
        int RecoverDetails { get; set; }
        int Sharpen { get; set; }
        int ReduceNoise { get; set; }
        int Dehalo { get; set; }
        int AntiAliasDeblur { get; set; } 
        bool Auto { get; set; }

        int RecoverOriginalDetails { get; set; }

        int OffsetX { get; set; }
        int OffsetY { get; set; }

    }
}
