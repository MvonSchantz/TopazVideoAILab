using System.Configuration;

namespace Cybercraft.Common
{
    /*
    
    <configSections>
      <section name="section name in App.config" type="Namespace.SectionClassName, Namespace"/>
    </configSections>    
     

    // ReSharper disable once ClassNeverInstantiated.Global
    public class SettingsSection : ConfigurationSection
    {
        [ConfigurationProperty("section name")]
        public SettingsConfigurationElement Section
        {
            get => (SettingsConfigurationElement)this["section name"];
            set => this["section name"] = value;
        }
    }

    public class SettingsConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("element name", IsRequired = true)]
        public string Element
        {
            get => (string)this["element name"];
            set => this["element name"] = value;
        }
    }


    */

    public class Configuration
    {
        public static T GetConfigurationSection<T>(string name) where T : ConfigurationSection
        {
            // ReSharper disable once UsePatternMatching
            var configurationSection = ConfigurationManager.GetSection(name) as T;
            if (configurationSection == null)
                throw new ConfigurationErrorsException($"Missing {name} section in App.config.");
            return configurationSection;
        }
    }
}
