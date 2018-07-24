using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    public interface IConfigModel
    {
        ExternalScriptViewModel GetSiteimproveScript(IUrlModel urlModel);
        bool GetShouldLogActivityValue();
    }
}
