namespace TopazVideoLab.Project
{
    public interface ISource
    {
        float Weight { get; set; }

        ICombination Source { get; set; }
    }
}
