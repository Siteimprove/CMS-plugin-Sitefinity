using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Abstractions;
using Siteimprove.Integration.Sitefinity.Resources;

namespace Siteimprove.Integration.Sitefinity.Configuration
{
    /// <summary>
    /// Defines the Siteimprove configuration settings
    /// </summary>
    [DescriptionResource(typeof(SiteimproveResources), "SiteimproveConfig")]
    public class SiteimproveConfig : ConfigSection
    {
        [ConfigurationProperty("logActivityInTheConsole", IsRequired = true, DefaultValue = false)]
        [ObjectInfo(typeof(SiteimproveResources), Title = "LogActivityInTheConsoleTitle", Description = "LogActivityInTheConsoleDescription")]
        public bool LogActivityInTheConsole
        {
            get
            {
                return (bool)this["logActivityInTheConsole"];
            }

            set
            {
                this["logActivityInTheConsole"] = value;
            }
        }


        [ConfigurationProperty("scriptUrl", IsRequired = true, DefaultValue = "https://cdn.siteimprove.net/cms/overlay.js")]
        [ObjectInfo(typeof(SiteimproveResources), Title = "ScriptUrlTitle", Description = "ScriptUrlDescription")]
        public string ScriptUrl
        {
            get
            {
                return (string)this["scriptUrl"];
            }

            set
            {
                this["scriptUrl"] = value;
            }
        }

        [ConfigurationProperty("tokens")]
        [ObjectInfo(typeof(SiteimproveResources), Title = "TokensTitle", Description = "TokensDescription")]
        public ConfigElementDictionary<string, TokenConfigElement> Tokens
        {
            get
            {
                return (ConfigElementDictionary<string, TokenConfigElement>)this["tokens"];
            }
        }
    }
}
