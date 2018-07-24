using System;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Restriction;
using Telerik.Sitefinity.Web.UI.Fields.Config;
using Telerik.Sitefinity.Web.UI.ContentUI.Config;
using Siteimprove.Integration.Sitefinity.Resources;
using Siteimprove.Integration.Sitefinity.FieldControls;
using Telerik.Sitefinity.Web.UI.ContentUI.Views.Backend.Master.Config;

namespace Siteimprove.Integration.Sitefinity
{
    public class SiteimproveInstaller
    {
        public void Install(SiteInitializer initializer)
        {
            this.RegisterCustomScripts(initializer);
        }

        public void UpgradeTo10_2_6600_1(SiteInitializer initializer)
        {
            this.RegisterCustomScripts(initializer);
        }

        public void Uninstall(SiteInitializer initializer)
        {
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
    }
}