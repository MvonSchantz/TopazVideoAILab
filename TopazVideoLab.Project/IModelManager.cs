namespace TopazVideoLab.Project
{
    public interface IModelManager
    {
        IModel GetModel(UpscaleAlgorithm algorithm);
    }
}
