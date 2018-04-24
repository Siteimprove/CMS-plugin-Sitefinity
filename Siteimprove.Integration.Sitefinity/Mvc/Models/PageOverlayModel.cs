using System;
using Telerik.Sitefinity.Services;
using Siteimprove.Integration.Sitefinity.Web;
using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Telerik.Sitefinity.Configuration;
using Siteimprove.Integration.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;
using Siteimprove.Integration.Sitefinity.Resources;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Provides common business logic for Overlay widget displayed in Sitefinity pages
    /// </summary>
    public abstract class PageOverlayModel : IPageOverlayModel
    {
        protected IControllerHost _controllerHost;
        protected ITokenModel _tokenModel;
        protected IUrlModel _urlMode;

        public string CurrentDomain
        {
            get
            {
                return SystemManager.CurrentContext.CurrentSite.GetUri().AbsoluteUri;
            }
        }

        public virtual string ViewName => "Default";

        public IControllerHost ControllerHost
        {
            get
            {
                if (this._controllerHost == null)
                    this._controllerHost = new ControllerHost();

                return this._controllerHost;
            }
        }

        public ITokenModel TokenModel
        {
            get
            {
                if (this._tokenModel == null)
                    this._tokenModel = new TokenModel();

                return this._tokenModel;
            }
        }

        public IUrlModel UrlModel
        {
            get
            {
                if (this._urlMode == null)
                    this._urlMode = new UrlModel();

                return this._urlMode;
            }
        }

        public virtual OverlayViewModel GetViewModel()
        {
            var overlayModel = new OverlayViewModel();
            overlayModel.Domain = this.CurrentDomain;
            overlayModel.Token = this.TokenModel.GetTokenCreateIfNull(this.UrlModel.ResolveDomainFrom(this.CurrentDomain));
            overlayModel.ExternalScript = this.GetSiteimproveScript();
            overlayModel.ShouldLogActivity = Config.Get<SiteimproveConfig>().LogActivityInTheConsole;
            
            return overlayModel;
        }

        private ExternalScriptViewModel GetSiteimproveScript()
        {
            var script = new ExternalScriptViewModel();
            script.Url = Config.Get<SiteimproveConfig>().ScriptUrl;
            script.LoadAsync = false;

            if (string.IsNullOrEmpty(script.Url) || !this.UrlModel.IsUrlValid(script.Url))
                throw new ArgumentException(Res.Get<SiteimproveResources>().ErrorScriptUrlNotValid);

            return script; 
        }

        public abstract void PopulateControllerHost();
    }
}
