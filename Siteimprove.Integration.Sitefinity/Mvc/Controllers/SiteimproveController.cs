using System;
using System.Web.Mvc;
using Siteimprove.Integration.Sitefinity.Mvc.Models;
using Siteimprove.Integration.Sitefinity.Mvc.ViewModels;
using Siteimprove.Integration.Sitefinity.Resources;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Localization;

namespace Siteimprove.Integration.Sitefinity.Mvc.Controllers
{
    /// <summary>
    /// Main MVC Controller that will be used to load the Siteimprove scripts into various parts of the Sitefinity backend
    /// </summary>
    public class SiteimproveController : Controller
    {
        public IPageOverlayModel Model { get; set; }

        public ActionResult Index()
        {
            try
            {
                var viewModel = Model.GetViewModel();
                return View(this.Model.ViewName, viewModel);
            }
            catch (Exception exception)
            {
                Log.Write(Res.Get<SiteimproveResources>().ErrorOverlayWidgetLoading + this.HttpContext.Request.Url.AbsoluteUri + Environment.NewLine + exception, ConfigurationPolicy.ErrorLog);
                return View("Error", new ErrorViewModel(exception));
            }
        }

        [Route("~/siteimprove/propertiesBySite/{siteId?}")]
        [HttpGet]
        public JsonResult OverlayPropertiesBySite(Guid? siteId)
        {
            try
            {
                var model = new EditPropertiesOverlayModel();
                var siteIdGuid = siteId ?? Guid.Empty;

                var viewModel = model.GetViewModelBySite(siteIdGuid);
                var result = new
                {
                    Token = viewModel.Token,
                    ShouldLogActivity = viewModel.ShouldLogActivity,
                    Domain = viewModel.Domain
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Log.Write(Res.Get<SiteimproveResources>().ErrorOverlayWidgetLoading + this.HttpContext.Request.Url.AbsoluteUri + Environment.NewLine + exception, ConfigurationPolicy.ErrorLog);
                throw new ApplicationException();
            }
        }

        [Route("~/siteimprove/propertiesByPage/{pageId}")]
        [HttpGet]
        public JsonResult OverlayPropertiesByPage(string pageId)
        {
            try
            {
                var model = new EditPropertiesOverlayModel();
                var viewModel = model.GetViewModelByPage(pageId);
                var result = new
                {
                    Token = viewModel.Token,
                    ShouldLogActivity = viewModel.ShouldLogActivity,
                    Domain = viewModel.Domain
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception)
            {
                Log.Write(Res.Get<SiteimproveResources>().ErrorOverlayWidgetLoading + this.HttpContext.Request.Url.AbsoluteUri + Environment.NewLine + exception, ConfigurationPolicy.ErrorLog);
                throw new ApplicationException();
            }
        }

        protected override void HandleUnknownAction(string actionName)
        {
            this.ActionInvoker.InvokeAction(this.ControllerContext, "Index");
        }
    }
}
