using Siteimprove.Integration.Sitefinity.Mvc.Controllers;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Provides business logic specific for overlays used in Sitefinity Backend pages
    /// </summary>
    public class BackendPageOverlayModel : PageOverlayModel
    {
        public override string ViewName => "Backend";

        public override void PopulateControllerHost()
        {
            var controller = new SiteimproveController();
            controller.Model = this;

            this.ControllerHost.HostController<SiteimproveController>(controller);
        }
    }
}
