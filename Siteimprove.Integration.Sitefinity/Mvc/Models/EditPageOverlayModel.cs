using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Web.UI.NavigationControls;
using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Siteimprove.Integration.Sitefinity.Mvc.Controllers;

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
            if (currentNode != null)
            {
                var url = NavigationUtilities.ResolveUrl(currentNode, true);
                overlayModel.PageUrl = UrlPath.ResolveUrl(url, true, true);
            }

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
