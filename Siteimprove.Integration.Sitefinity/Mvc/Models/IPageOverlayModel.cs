using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Siteimprove.Integration.Sitefinity.Web;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Implementations of this interface should provide business logic for Overlays displayed throughout Sitefinity pages
    /// </summary>
    public interface IPageOverlayModel
    {
        IControllerHost ControllerHost { get; }

        void PopulateControllerHost();

        string CurrentDomain { get; }

        string ViewName { get; }

        OverlayViewModel GetViewModel();
    }
}
