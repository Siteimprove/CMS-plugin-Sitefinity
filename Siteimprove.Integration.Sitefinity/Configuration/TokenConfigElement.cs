using System.Configuration;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Siteimprove.Integration.Sitefinity.Resources;

namespace Siteimprove.Integration.Sitefinity.Configuration
{
    /// <summary>
    /// Config Element to enable managing Siteimprove generated tokens used by the Siteimprove Plugin
    /// </summary>
    public class TokenConfigElement : ConfigElement
    {
        public TokenConfigElement(ConfigElement parent) : base(parent) { }

        [ConfigurationProperty("domain", IsRequired = true, IsKey = true, DefaultValue = "")]
        [ObjectInfo(typeof(SiteimproveResources), Title = "DomainTitle", Description = "DomainDescription")]
        public string Domain
        {
            get
            {
                return (string)this["domain"];
            }

            set
            {
                this["domain"] = value;
            }
        }

        [ConfigurationProperty("token", IsRequired = true)]
        [ObjectInfo(typeof(SiteimproveResources), Title = "TokenTitle", Description = "TokenDescription")]
        public string Token
        {
            get
            {
                return (string)this["token"];
            }
            set
            {
                this["token"] = value;
            }
        }
    }
}
