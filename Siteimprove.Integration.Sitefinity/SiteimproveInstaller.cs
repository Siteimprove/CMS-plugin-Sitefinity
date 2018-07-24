using System;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Restriction;
using Telerik.Sitefinity.Web.UI.Fields.Config;
using Telerik.Sitefinity.Web.UI.ContentUI.Config;
using Siteimprove.Integration.Sitefinity.Resources;
using Telerik.Sitefinity.WebSecurity.Configuration;
using Siteimprove.Integration.Sitefinity.FieldControls;
using Telerik.Sitefinity.Web.UI.ContentUI.Views.Backend.Master.Config;

namespace Siteimprove.Integration.Sitefinity
{
    public class SiteimproveInstaller
    {
        public void Install(SiteInitializer initializer)
        {
            this.RegisterCustomScripts(initializer);
            this.AddSiteimproveToWebSecurityConfig();
        }

        public void UpgradeTo10_2_6600_1(SiteInitializer initializer)
        {
            this.RegisterCustomScripts(initializer);
            this.AddSiteimproveToWebSecurityConfig();
        }

        public void Uninstall(SiteInitializer initializer)
        {
            this.RemoveSiteimproveFromWebSecurityConfig();
            this.RemoveCustomScripts();
        }

        #region Configuration

        private void RegisterCustomScripts(SiteInitializer initializer)
        {
            this.RegisterOverlayFieldControl(initializer, PagesDefinitions.FrontendPagesCreateViewName, SiteimproveInstaller.OverlayScriptCreateFieldName);
            this.RegisterOverlayFieldControl(initializer, PagesDefinitions.FrontendPagesEditViewName, SiteimproveInstaller.OverlayScriptEditFieldName);
            this.RegisterOverlayFieldControl(initializer, PagesDefinitions.FrontendPagesDuplicateViewName, SiteimproveInstaller.OverlayScriptDuplicateFieldName);

            this.RegisterExtensionScript(initializer, PagesDefinitions.FrontendPagesCreateViewName);
            this.RegisterExtensionScript(initializer, PagesDefinitions.FrontendPagesEditViewName);
            this.RegisterExtensionScript(initializer, PagesDefinitions.FrontendPagesDuplicateViewName);
        }

        private void RemoveCustomScripts()
        {
            this.RemoveExtensionScriptConfig();
            this.RemoveOverlayFieldControlConfig();
        }

        private void RegisterOverlayFieldControl(SiteInitializer initializer, string viewName, string fieldName)
        {
            try
            {
                var configManager = ConfigManager.GetManager();

                using (new FileSystemModeRegion())
                {
                    var contentViewConfig = initializer.Context.GetConfig<ContentViewConfig>();
                    var frontendPages = contentViewConfig.ContentViewControls[PagesDefinitions.FrontendPagesDefinitionName];

                    if (frontendPages.ViewsConfig.ContainsKey(viewName))
                    {
                        var detailsView = (DetailFormViewElement)frontendPages.ViewsConfig[viewName];

                        if (!detailsView.Sections["MainSection"].Fields.ContainsKey(fieldName))
                        {
                            detailsView.Sections["MainSection"].Fields.Add(new GenericFieldDefinitionElement(detailsView.Sections["MainSection"].Fields)
                            {
                                FieldType = typeof(OverlayScriptField),
                                FieldName = fieldName
                            });

                            configManager.Provider.SuppressSecurityChecks = true;
                            configManager.SaveSection(contentViewConfig);
                            configManager.Provider.SuppressSecurityChecks = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorAddingFieldControl;
                throw new Exception(errorMessage, ex);
            }
        }

        private void RegisterExtensionScript(SiteInitializer initializer, string viewName)
        {
            try
            {
                var configManager = ConfigManager.GetManager();

                using (new FileSystemModeRegion())
                {
                    var contentViewConfig = initializer.Context.GetConfig<ContentViewConfig>();
                    var frontendPages = contentViewConfig.ContentViewControls[PagesDefinitions.FrontendPagesDefinitionName];

                    if (frontendPages.ViewsConfig.ContainsKey(viewName))
                    {
                        var editDetailsView = frontendPages.ViewsConfig[viewName];

                        if (!editDetailsView.Scripts.ContainsKey(SiteimproveInstaller.ScriptExtensionLocationAndKey))
                        {
                            editDetailsView.Scripts.Add(new ClientScriptElement(editDetailsView.Scripts)
                            {
                                ScriptLocation = SiteimproveInstaller.ScriptExtensionLocationAndKey,
                                LoadMethodName = "OnPageDetailViewLoaded"
                            });

                            configManager.Provider.SuppressSecurityChecks = true;
                            configManager.SaveSection(contentViewConfig);
                            configManager.Provider.SuppressSecurityChecks = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorRegisteringSiteimprovePageDetailsExtension;
                throw new Exception(errorMessage, ex);
            }
        }

        private void RemoveExtensionScriptConfig()
        {
            try
            {
                bool saveConfig = false;
                var configManager = ConfigManager.GetManager();
                var contentViewConfig = configManager.GetSection<ContentViewConfig>();
                var frontendPages = contentViewConfig.ContentViewControls[PagesDefinitions.FrontendPagesDefinitionName];

                if (frontendPages.ViewsConfig.ContainsKey(PagesDefinitions.FrontendPagesEditViewName))
                {
                    var editDetailsView = frontendPages.ViewsConfig[PagesDefinitions.FrontendPagesEditViewName];
                    var toRemove = editDetailsView.Scripts[SiteimproveInstaller.ScriptExtensionLocationAndKey];

                    if (toRemove != null)
                    {
                        using (new UnrestrictedModeRegion())
                        {
                            editDetailsView.Scripts.Remove(toRemove);
                            saveConfig = true;
                        }
                    }
                }

                if (frontendPages.ViewsConfig.ContainsKey(PagesDefinitions.FrontendPagesCreateViewName))
                {
                    var editDetailsView = frontendPages.ViewsConfig[PagesDefinitions.FrontendPagesCreateViewName];
                    var toRemove = editDetailsView.Scripts[SiteimproveInstaller.ScriptExtensionLocationAndKey];

                    if (toRemove != null)
                    {
                        using (new UnrestrictedModeRegion())
                        {
                            editDetailsView.Scripts.Remove(toRemove);
                            saveConfig = true;
                        }
                    }
                }

                if (frontendPages.ViewsConfig.ContainsKey(PagesDefinitions.FrontendPagesDuplicateViewName))
                {
                    var duplicateDetailsView = frontendPages.ViewsConfig[PagesDefinitions.FrontendPagesDuplicateViewName];
                    var toRemove = duplicateDetailsView.Scripts[SiteimproveInstaller.ScriptExtensionLocationAndKey];

                    if (toRemove != null)
                    {
                        using (new UnrestrictedModeRegion())
                        {
                            duplicateDetailsView.Scripts.Remove(toRemove);
                            saveConfig = true;
                        }
                    }
                }

                if (saveConfig)
                {
                    using (new UnrestrictedModeRegion())
                    {
                        configManager.Provider.SuppressSecurityChecks = true;
                        configManager.SaveSection(contentViewConfig);
                        configManager.Provider.SuppressSecurityChecks = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorRemovingScriptConfig;
                throw new Exception(errorMessage, ex);
            }
        }

        private void RemoveOverlayFieldControlConfig()
        {
            try
            {
                bool saveConfig = false;
                var configManager = ConfigManager.GetManager();
                var contentViewConfig = configManager.GetSection<ContentViewConfig>();
                var frontendPages = contentViewConfig.ContentViewControls[PagesDefinitions.FrontendPagesDefinitionName];

                if (frontendPages.ViewsConfig.ContainsKey(PagesDefinitions.FrontendPagesEditViewName))
                {
                    var detailsView = (DetailFormViewElement)frontendPages.ViewsConfig[PagesDefinitions.FrontendPagesEditViewName];
                    var toRemove = detailsView.Sections["MainSection"].Fields[SiteimproveInstaller.OverlayScriptEditFieldName];

                    if (toRemove != null)
                    {
                        detailsView.Sections["MainSection"].Fields.Remove(toRemove);
                        saveConfig = true;
                    }
                }

                if (frontendPages.ViewsConfig.ContainsKey(PagesDefinitions.FrontendPagesCreateViewName))
                {
                    var detailsView = (DetailFormViewElement)frontendPages.ViewsConfig[PagesDefinitions.FrontendPagesCreateViewName];
                    var toRemove = detailsView.Sections["MainSection"].Fields[SiteimproveInstaller.OverlayScriptCreateFieldName];

                    if (toRemove != null)
                    {
                        detailsView.Sections["MainSection"].Fields.Remove(toRemove);
                        saveConfig = true;
                    }
                }

                if (frontendPages.ViewsConfig.ContainsKey(PagesDefinitions.FrontendPagesDuplicateViewName))
                {
                    var detailsView = (DetailFormViewElement)frontendPages.ViewsConfig[PagesDefinitions.FrontendPagesDuplicateViewName];
                    var toRemove = detailsView.Sections["MainSection"].Fields[SiteimproveInstaller.OverlayScriptDuplicateFieldName];

                    if (toRemove != null)
                    {
                        detailsView.Sections["MainSection"].Fields.Remove(toRemove);
                        saveConfig = true;
                    }
                }

                if (saveConfig)
                {
                    using (new UnrestrictedModeRegion())
                    {
                        configManager.Provider.SuppressSecurityChecks = true;
                        configManager.SaveSection(contentViewConfig);
                        configManager.Provider.SuppressSecurityChecks = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorRemovingFieldControl;
                throw new Exception(errorMessage, ex);
            }
        }

        private const string SiteimproveVirtualPath = "~/Siteimprove/*";
        private const string ScriptExtensionLocationAndKey = "Siteimprove.Integration.Sitefinity.ScriptExtensions.siteimprove.js, Siteimprove.Integration.Sitefinity";
        private const string OverlayScriptCreateFieldName = "SiteimproveOverlayScriptCreate";
        private const string OverlayScriptDuplicateFieldName = "SiteimproveOverlayScriptDuplicate";
        private const string OverlayScriptEditFieldName = "SiteimproveOverlayScriptEdit";

        #endregion

        #region Content-Security-Policy

        private void AddSiteimproveToWebSecurityConfig()
        {
            try
            {
                var configManager = ConfigManager.GetManager();
                var webSecurityConfig = configManager.GetSection<WebSecurityConfig>();
                var headers = webSecurityConfig.HttpSecurityHeaders.ResponseHeaders;

                bool hasChanges = false;

                var cspHeaders = headers[SiteimproveInstaller.CspSectionName].Value;
                string newSettingValue = cspHeaders;

                using (new UnrestrictedModeRegion())
                {
                    if (!this.HeaderContainValue(newSettingValue, SiteimproveInstaller.ScriptSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl1))
                    {
                        newSettingValue = this.InsertHeaderValue(newSettingValue, SiteimproveInstaller.ScriptSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl1);
                        hasChanges = true;
                    }

                    if (!this.HeaderContainValue(newSettingValue, SiteimproveInstaller.ConnectSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl2))
                    {
                        newSettingValue = this.InsertHeaderValue(newSettingValue, SiteimproveInstaller.ConnectSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl2);
                        hasChanges = true;
                    }

                    if (!this.HeaderContainValue(newSettingValue, SiteimproveInstaller.ChildSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl2))
                    {
                        newSettingValue = this.InsertHeaderValue(newSettingValue, SiteimproveInstaller.ChildSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl2);
                        hasChanges = true;
                    }

                    if (hasChanges)
                    {
                        configManager.Provider.SuppressSecurityChecks = true;
                        headers[SiteimproveInstaller.CspSectionName].Value = newSettingValue;
                        configManager.SaveSection(webSecurityConfig);
                        configManager.Provider.SuppressSecurityChecks = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorAddingCspValuesInConfig;
                throw new Exception(errorMessage, ex);
            }
        }

        private void RemoveSiteimproveFromWebSecurityConfig()
        {
            try
            {
                var configManager = ConfigManager.GetManager();
                var webSecurityConfig = configManager.GetSection<WebSecurityConfig>();
                if (webSecurityConfig != null)
                {
                    var headers = webSecurityConfig?.HttpSecurityHeaders?.ResponseHeaders;
                    var cspHeaders = headers[SiteimproveInstaller.CspSectionName]?.Value;

                    if (cspHeaders == null)
                        return;

                    using (new UnrestrictedModeRegion())
                    {
                        cspHeaders = this.RemoveHeaderValue(cspHeaders, SiteimproveInstaller.ScriptSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl1);
                        cspHeaders = this.RemoveHeaderValue(cspHeaders, SiteimproveInstaller.ConnectSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl2);
                        cspHeaders = this.RemoveHeaderValue(cspHeaders, SiteimproveInstaller.ChildSrcHeaderName, SiteimproveInstaller.SiteimrpoveUrl2);

                        configManager.Provider.SuppressSecurityChecks = true;
                        headers[SiteimproveInstaller.CspSectionName].Value = cspHeaders;
                        configManager.SaveSection(webSecurityConfig);
                        configManager.Provider.SuppressSecurityChecks = false;
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorRemovingCspValuesInConfig;
                throw new Exception(errorMessage, ex);
            }
        }

        private bool HeaderContainValue(string sfSettingValue, string headerName, string headerValue)
        {
            var headerMatch = System.Text.RegularExpressions.Regex.Match(sfSettingValue, headerName + @".*?;");
            bool isMatch = headerMatch.Value.Contains(headerValue);
            return isMatch;
        }

        private string InsertHeaderValue(string sfSettingValue, string headerName, string headerValue)
        {
            var resultValue = System.Text.RegularExpressions.Regex.Replace(sfSettingValue, headerName + @"\s([^;]*)", headerName + " $1 " + headerValue);
            return resultValue;
        }

        private string RemoveHeaderValue(string sfSettingValue, string headerName, string headerValue)
        {
            var result = sfSettingValue.Replace(" " + headerValue, string.Empty);
            return result;
        }

        private const string CspSectionName = "Content-Security-Policy";
        private const string SiteimrpoveUrl1 = "https://cdn.siteimprove.net";
        private const string SiteimrpoveUrl2 = "https://*.siteimprove.com";

        private const string ScriptSrcHeaderName = "script-src";
        private const string ConnectSrcHeaderName = "connect-src";
        private const string ChildSrcHeaderName = "child-src";

        #endregion
    }
}
