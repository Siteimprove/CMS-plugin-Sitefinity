using Siteimprove.Integration.Sitefinity.Mvc.Controllers;
using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Telerik.Sitefinity.Web;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Provides business logic specific for overlays used in the Page Editor
    /// </summary>
    public class EditPageOverlayModel : PageOverlayModel
    {
        public override OverlayViewModel GetViewModel()
        {
            var overlayModel = base.GetViewModel();

            var currentNode = SiteMapBase.GetCurrentNode();
            overlayModel.PageUrl = this.UrlModel.ResolveUrlFromSiteMapNode(currentNode);

            return overlayModel;
        }

        public override void PopulateControllerHost()
        {
            var controller = new SiteimproveController();
            controller.Model = this;

            this.ControllerHost.HostController<SiteimproveController>(controller);
        }

        public override string ViewName => "PageEdit";
    }
}
