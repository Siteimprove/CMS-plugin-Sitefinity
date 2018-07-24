using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Services;
using System;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Provides business logic specific for overlays used in the Page Titles&Properties screen
    /// </summary>
    public class EditPropertiesOverlayModel
    {
        protected ITokenModel tokenModel;
        protected IConfigModel configModel;

        public ITokenModel TokenModel
        {
            get
            {
                if (this.tokenModel == null)
                    this.tokenModel = new TokenModel();

                return this.tokenModel;
            }
        }

        protected IUrlModel urlModel;
        public IUrlModel UrlModel
        {
            get
            {
                if (this.urlModel == null)
                    this.urlModel = new UrlModel();

                return this.urlModel;
            }
        }

        public IConfigModel ConfigModel
        {
            get
            {
                if (this.configModel == null)
                    this.configModel = new ConfigModel();

                return this.configModel;
            }
        }

        public OverlayViewModel GetViewModelByPage(string pageId)
        {
            var overlayModel = new OverlayViewModel
            {
                ExternalScript = this.ConfigModel.GetSiteimproveScript(this.UrlModel),
                ShouldLogActivity = this.ConfigModel.GetShouldLogActivityValue()
            };

            var pageManager = PageManager.GetManager();
            var pageNode = pageManager.GetPageNode(new System.Guid(pageId));

            if (pageNode != null)
            {
                overlayModel.Domain = this.UrlModel.ResolveDomainFromPageNode(pageNode);
                overlayModel.Token = this.TokenModel.GetTokenCreateIfNull(this.UrlModel.ResolveDomainFrom(overlayModel.Domain));
            }

            return overlayModel;
        }

        public OverlayViewModel GetViewModelBySite(Guid siteId)
        {
            var overlayModel = new OverlayViewModel
            {
                ExternalScript = this.ConfigModel.GetSiteimproveScript(this.UrlModel),
                ShouldLogActivity = this.ConfigModel.GetShouldLogActivityValue()
            };

            overlayModel.Domain = this.UrlModel.ResolveDomainBySiteId(siteId);
            overlayModel.Token = this.TokenModel.GetTokenCreateIfNull(this.UrlModel.ResolveDomainFrom(overlayModel.Domain));

            return overlayModel;
        }
    }
}