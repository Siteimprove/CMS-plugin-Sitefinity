using Siteimprove.Integration.Sitefinity.Configuration;
using Siteimprove.Integration.Sitefinity.Infrastructure;
using Siteimprove.Integration.Sitefinity.Resources;
using System;
using System.Diagnostics;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Restriction;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Provides business logic for the Siteimprove Token related logic in Sitefinity
    /// </summary>
    public class TokenModel : ITokenModel
    {
        private SiteimproveHttpClient _httpClient;
        private ConfigManager _configManager;

        public TokenModel()
        {
            this._httpClient = new SiteimproveHttpClient();
        }

        private ConfigManager ConfigManager
        {
            get
            {
                if (this._configManager == null)
                    this._configManager = ConfigManager.GetManager();

                return this._configManager;
            }
        }

        public string GetTokenCreateIfNull(string domain)
        {
            TokenConfigElement tokenElement;

            if (this.TryGetToken(domain, out tokenElement))
            {
                var token = this._httpClient.FetchToken();
                tokenElement = this.SetToken(token, domain);
            }

            return tokenElement.Token;
        }

        public bool TryGetToken(string domain, out TokenConfigElement tokenElement)
        {
            return !Config.Get<SiteimproveConfig>().Tokens.TryGetValue(domain, out tokenElement);
        }

        public TokenConfigElement SetToken(string token, string domain)
        {
            var siteimproveConfig = this.ConfigManager.GetSection<SiteimproveConfig>();
            var tokenElement = new TokenConfigElement(siteimproveConfig.Tokens)
            {
                Token = token,
                Domain = domain
            };

            try
            {
                using (new UnrestrictedModeRegion())
                {

                    this.ConfigManager.Provider.SuppressSecurityChecks = true;
                    siteimproveConfig.Tokens.Add(tokenElement);
                    this.ConfigManager.SaveSection(siteimproveConfig);
                    this.ConfigManager.Provider.SuppressSecurityChecks = false;
                }

                return tokenElement;
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorSavingTokenInConfigs;
                throw new Exception(errorMessage, ex);
            }
        }
    }
}
