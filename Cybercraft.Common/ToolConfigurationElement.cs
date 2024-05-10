using System.Configuration;

namespace Cybercraft.Common
{
    public class ToolConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("path", IsRequired = true)]
        public string Path
        {
            get => (string)this["path"];
            set => this["path"] = value;
        }
    }
}