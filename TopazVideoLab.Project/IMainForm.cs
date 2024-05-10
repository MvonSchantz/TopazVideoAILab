using System;

namespace TopazVideoLab.Project
{
    public interface IMainForm
    {
        Version Version { get; }

        ICombination[] Combinations { get; }

        ICombination AddCombination(bool isSource);

        int PreviewFrame { get; set; }
        int PreviewLength { get; set; }

        IModelManager ModelManager { get; }
    }
}
