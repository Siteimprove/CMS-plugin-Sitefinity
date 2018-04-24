using System;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Localization;
using Siteimprove.Integration.Sitefinity.Resources;

namespace Siteimprove.Integration.Sitefinity.Web
{
    /// <summary>
    /// A wrapper around the Sitefinity MvcProxyControl to host the OverlayController
    /// </summary>
    public class ControllerHost : IControllerHost
    {
        private MvcControllerProxy _instance;

        public MvcProxyBase Instance
        {
            get
            {
                if (this._instance == null)
                    this._instance = new MvcControllerProxy();

                return this._instance;
            }
        }

        public void HostController<T>(T controllerInstance) where T : Controller
        {
            this.AddControllerType(controllerInstance.GetType());
            this.Instance.Settings = new ControllerSettings(controllerInstance);
        }

        public void AddToPageHeader(Page page)
        {
            page.Header.Controls.Add((MvcControllerProxy)this.Instance);
        }

        private void AddControllerType(Type type)
        {
            if (!type.IsSubclassOf(typeof(Controller)))
                throw new ArgumentOutOfRangeException(Res.Get<SiteimproveResources>().ErrorControllerType);

            this.Instance.ControllerName = type.FullName;
        }
    }
}
