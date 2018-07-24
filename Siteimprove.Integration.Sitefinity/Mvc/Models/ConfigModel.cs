using Siteimprove.Integration.Sitefinity.Configuration;
using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Siteimprove.Integration.Sitefinity.Resources;
using System;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    public class ConfigModel : IConfigModel
    {
        public ExternalScriptViewModel GetSiteimproveScript(IUrlModel urlModel)
        {
            var script = new ExternalScriptViewModel();
            script.Url = Config.Get<SiteimproveConfig>().ScriptUrl;
            script.LoadAsync = false;

            if (string.IsNullOrEmpty(script.Url) || !urlModel.IsUrlValid(script.Url))
                throw new ArgumentException(Res.Get<SiteimproveResources>().ErrorScriptUrlNotValid);

            return script;
        }

        public bool GetShouldLogActivityValue()
        {
            return Config.Get<SiteimproveConfig>().LogActivityInTheConsole;
        }
    }
}
