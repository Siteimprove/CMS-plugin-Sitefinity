using System;
using System.Linq;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web.Events;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Microsoft.Practices.Unity;
using Siteimprove.Integration.Sitefinity.Infrastructure;
using Siteimprove.Integration.Sitefinity.Mvc.Models;
using Siteimprove.Integration.Sitefinity.Configuration;
using Siteimprove.Integration.Sitefinity.Resources;
using System.Collections.Generic;
using Telerik.Sitefinity.Abstractions.VirtualPath.Configuration;

namespace Siteimprove.Integration.Sitefinity
{
    /// <summary>
    /// The main module used to encapsulate all Sitefinity installation/initialization/uninstallation logic
    /// </summary>
    public class SiteimproveModule : ModuleBase
    {
        public override Guid LandingPageId => Guid.Empty;

        public override Type[] Managers => new Type[0];

        private SiteimproveInstaller installer;

        protected override ConfigSection GetModuleConfig()
        {
            return Config.Get<SiteimproveConfig>();
        }

        public override void Initialize(ModuleSettings settings)
        {
            base.Initialize(settings);

            App.WorkWith()
                .Module(settings.Name)
                .Initialize()
                .Localization<SiteimproveResources>()
                .Configuration<SiteimproveConfig>();

            SubscribeToEvents();
        }

        protected override IDictionary<string, Action<VirtualPathElement>> GetVirtualPaths()
        {
            var paths = new Dictionary<string, Action<VirtualPathElement>>();
            paths.Add(SiteimproveVirtualPath, null);
            return paths;
        }

        public override void Unload()
        {
            this.UnsubscribeFromEvents();
            base.Unload();
        }

        public override void Install(SiteInitializer initializer)
        {
            installer = new SiteimproveInstaller();
            this.installer.Install(initializer);
            Log.Write("Installing Siteimprove Plugin module: Success", ConfigurationPolicy.Trace);
        }

        public override void Uninstall(SiteInitializer initializer)
        {
            this.installer = new SiteimproveInstaller();
            this.installer.Uninstall(initializer);
            base.Uninstall(initializer);
            Log.Write("Uninstalling Siteimprove Plugin module: Success", ConfigurationPolicy.Trace);
        }

        public override void Upgrade(SiteInitializer initializer, Version upgradeFrom)
        {
            if (upgradeFrom < SiteimproveVersion10_2_6600_1)
                this.installer.UpgradeTo10_2_6600_1(initializer);

            Log.Write("Upgrading Siteimprove Plugin module: Success", ConfigurationPolicy.Trace);
        }

        public static void Register()
        {
            bool isModuleInstalled = Config.Get<SystemConfig>().ApplicationModules.Elements
               .Any(m => m.Name.Equals(SiteimproveModule.ModuleName));

            if (!isModuleInstalled)
            {
                try
                {
                    Res.RegisterResource<SiteimproveResources>();
                    var configManager = ConfigManager.GetManager();
                    var modulesConfig = configManager.GetSection<SystemConfig>().ApplicationModules;
                    if (modulesConfig != null)
                    {
                        modulesConfig.Add(SiteimproveModule.ModuleName, new AppModuleSettings(modulesConfig)
                        {
                            Name = SiteimproveModule.ModuleName,
                            Title = SiteimproveModule.ModuleTitle,
                            Type = typeof(SiteimproveModule).AssemblyQualifiedName,
                            Description = Res.Get<SiteimproveResources>().SiteimproveModuleDescription,
                            StartupType = StartupType.OnApplicationStart
                        });

                        configManager.Provider.SuppressNotifications = true;
                        configManager.SaveSection(modulesConfig.Section);
                        configManager.Provider.SuppressNotifications = false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("Could not register the Siteimprove module. The following exception was encountered:" + Environment.NewLine + ex);
                }
            }
        }

        private void SubscribeToEvents()
        {
            Bootstrapper.Bootstrapped += Bootstrapper_Bootstrapped;
            EventHub.Subscribe<IPagePreRenderCompleteEvent>(this.OnPagePreRenderCompleteEventHandler);
            PageManager.Executing += new EventHandler<ExecutingEventArgs>(PageManager_Executing);
        }

        private void PageManager_Executing(object sender, ExecutingEventArgs eventArgs)
        {
            var publishingEventHandler = new PageEventHandler();

            try
            {
                if (publishingEventHandler.IsPublishingOperation(eventArgs) && publishingEventHandler.TrySetPageData(sender))
                {
                    var isFrontendNode = publishingEventHandler.IsFrontendNode();
                    var isPublishedNode = publishingEventHandler.IsPublishedNode();

                    if (isFrontendNode && isPublishedNode)
                        publishingEventHandler.ScheduleSiteimproveNotification();
                }
            }
            catch (Exception ex)
            {
                Log.Write("Unhandled exception during execution of the " + eventArgs.CommandName + "command of the PageManager. Full exception below:" + Environment.NewLine + ex);
            }
        }

        private void Bootstrapper_Bootstrapped(object sender, EventArgs e)
        {
            ObjectFactory.Container.RegisterType<PageEditorRouteHandler, OverlayPageEditorRouteHandler>();
        }

        private void OnPagePreRenderCompleteEventHandler(IPagePreRenderCompleteEvent args)
        {
            if (args.PageSiteNode.IsBackend)
            {
                try
                {
                    var overlayModel = new BackendPageOverlayModel();
                    overlayModel.PopulateControllerHost();
                    overlayModel.ControllerHost.AddToPageHeader(args.Page);
                }
                catch (Exception ex)
                {
                    Log.Write("Unhandled exception trying to Add the oVerlay widget onto the " + args.Page.Title + " page. Exception details below:" + Environment.NewLine + ex, ConfigurationPolicy.ErrorLog);
                }
            }
        }

        private void UnsubscribeFromEvents()
        {
            EventHub.Unsubscribe<IPagePreRenderCompleteEvent>(this.OnPagePreRenderCompleteEventHandler);
            Bootstrapper.Bootstrapped -= Bootstrapper_Bootstrapped;
            PageManager.Executing -= PageManager_Executing;
        }

        public const string ModuleName = "SiteimproveModule";
        public const string ModuleTitle = "Siteimprove";
        public const string SiteimproveVirtualPath = "~/Siteimprove/*";

        public static readonly Version SiteimproveVersion10_2_6600_1 = new Version(10, 2, 6600, 1);
    }
}