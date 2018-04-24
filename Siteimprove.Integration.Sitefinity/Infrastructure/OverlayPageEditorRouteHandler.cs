using System;
using System.Web;
using System.Web.UI;
using System.Web.Routing;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure;
using Telerik.Sitefinity.Frontend.Mvc.Infrastructure.Routing;
using Siteimprove.Integration.Sitefinity.Mvc.Models;
using Telerik.Sitefinity.Abstractions;

namespace Siteimprove.Integration.Sitefinity.Infrastructure
{
    /// <summary>
    /// Inherits the PageEditor Route handler used by Feather. Used to load the Siteimprove required controls
    /// </summary>
    public class OverlayPageEditorRouteHandler : MvcPageEditorRouteHandler
    {
        protected override IHttpHandler BuildHandler(RequestContext requestContext, IPageData pageData)
        {
            var handler = base.BuildHandler(requestContext, pageData);

            var page = handler.GetPageHandler();
            if (page != null)
            {
                page.Init += this.OverlayPageEditorRouteHandlerInit;
            }

            return handler;
        }

        private void OverlayPageEditorRouteHandlerInit(object sender, EventArgs e)
        {
            if (sender is Page page)
            {
                try
                {
                    var overlayModel = new EditPageOverlayModel();
                    overlayModel.PopulateControllerHost();
                    overlayModel.ControllerHost.AddToPageHeader(page);
                }
                catch (Exception ex)
                {
                    Log.Write("Unhandled exception during rendering the Siteimprove Overlay widget onto the " + page.Title + " page. Exception details below:" + Environment.NewLine + ex, ConfigurationPolicy.ErrorLog);
                }
                
            }
        }
    }
}
