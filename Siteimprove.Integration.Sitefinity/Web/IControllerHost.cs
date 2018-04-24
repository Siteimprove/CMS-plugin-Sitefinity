using System;
using System.Web.Mvc;
using System.Web.UI;
using Telerik.Sitefinity.Mvc.Proxy;

namespace Siteimprove.Integration.Sitefinity.Web
{
    /// <summary>
    /// Defines behavior a host of and MVC controller in Sitefinity
    /// </summary>
    public interface IControllerHost
    {
        MvcProxyBase Instance { get; }

        void HostController<T>(T controllerInstance) where T : Controller;

        void AddToPageHeader(Page page);
    }
}
