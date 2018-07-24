using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Siteimprove.Integration.Sitefinity.Web;
using Telerik.Sitefinity.Services;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Provides common business logic for Overlay widget displayed in Sitefinity pages
    /// </summary>
    public abstract class PageOverlayModel : IPageOverlayModel
    {
        protected IControllerHost _controllerHost;
        protected ITokenModel _tokenModel;
        protected IUrlModel _urlModel;
        protected IConfigModel _configModel;

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
                if (this._urlModel == null)
                    this._urlModel = new UrlModel();

                return this._urlModel;
            }
        }

        public IConfigModel ConfigModel
        {
            get
            {
                if (this._configModel == null)
                    this._configModel = new ConfigModel();

                return this._configModel;
            }
        }

        public virtual OverlayViewModel GetViewModel()
        {
            var overlayModel = new OverlayViewModel
            {
                Domain = this.CurrentDomain,
                Token = this.TokenModel.GetTokenCreateIfNull(this.UrlModel.ResolveDomainFrom(this.CurrentDomain)),
                ExternalScript = this.ConfigModel.GetSiteimproveScript(this.UrlModel),
                ShouldLogActivity = this.ConfigModel.GetShouldLogActivityValue()
            };

            return overlayModel;
        }

        public abstract void PopulateControllerHost();
    }
}
