using System.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace TopazVideoLab2
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class ToolsSection : ConfigurationSection
    {
        [ConfigurationProperty("TopazFfmpeg")]
        public ToolElement TopazFfmpeg
        {
            get => (ToolElement)this["TopazFfmpeg"];
            set => this["TopazFfmpeg"] = value;
        }

        [ConfigurationProperty("Avs2Yuv")]
        public ToolElement Avs2Yuv
        {
            get => (ToolElement)this["Avs2Yuv"];
            set => this["Avs2Yuv"] = value;
        }

        [ConfigurationProperty("VirtualDub64")]
        public ToolElement VirtualDub64
        {
            get => (ToolElement)this["VirtualDub64"];
            set => this["VirtualDub64"] = value;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class PathsSection : ConfigurationSection
    {
        [ConfigurationProperty("ModelDataDir")]
        public ToolElement ModelDataDir
        {
            get => (ToolElement)this["ModelDataDir"];
            set => this["ModelDataDir"] = value;
        }

        [ConfigurationProperty("ModelDir")]
        public ToolElement ModelDir
        {
            get => (ToolElement)this["ModelDir"];
            set => this["ModelDir"] = value;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class ToolElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get => (string)this["path"];
            set => this["path"] = value;
        }
    }

}
