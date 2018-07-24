using System;
using System.Web;
using System.Linq;
using System.Threading;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Data;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.BackgroundTasks;
using Telerik.Sitefinity.Security.Claims;

namespace Siteimprove.Integration.Sitefinity.Infrastructure
{
    /// <summary>
    /// Intercepts Page Publish events using the PageManger Executing events
    /// </summary>
    public class PageEventHandler
    {
        private PageData _pageData;

        public PageData PageData
        {
            get
            {
                if (this._pageData == null)
                    throw new ArgumentNullException("Page data cannot be null. Try call ExtractPageDataIfValidPublishingOperation method first");

                return this._pageData;
            }
        }

        public bool IsPublishingOperation(ExecutingEventArgs eventArgs)
        {
            var isPublishingOperation = false;
            var isTransaction = this.IsCommittingTransaction(eventArgs);
            if (isTransaction)
                isPublishingOperation = this.IsCurrentHttpRequestForPublishing()
                    || this.IsPageSaveRequest()
                    || this.IsUserSimulatedFromBackgroundTask();

            return isPublishingOperation;
        }

        public bool TrySetPageData(object eventSender)
        {
            var isParseSuccessful = false;

            if (eventSender is PageDataProvider dataProvider)
            {
                var pageData = dataProvider.GetDirtyItems()
                                     .OfType<PageData>()
                                     .FirstOrDefault();

                if (pageData != null)
                {
                    this._pageData = pageData;
                    if (this.IsTransactionTypeUpdating(dataProvider))
                        isParseSuccessful = true;
                }
            }

            return isParseSuccessful;
        }

        public bool IsFrontendNode()
        {
            return !this.PageData.NavigationNode.IsBackend;
        }

        public bool IsPublishedNode()
        {
            return this.PageData.NavigationNode.ApprovalWorkflowState == "Published";
        }

        public void ScheduleSiteimproveNotification()
        {
            var backgroundTasksService = ObjectFactory.Resolve<IBackgroundTasksService>();
            if (backgroundTasksService != null)
            {

                var url = this.PageData.NavigationNode.GetFullUrl(Thread.CurrentThread.CurrentUICulture, false);
                url = UrlPath.ResolveUrl(url, true, true);
                backgroundTasksService.EnqueueTask(() =>
                {
                    try
                    {
                        var siteimproveClient = new SiteimproveHttpClient();
                        siteimproveClient.RecheckUrl(url);
                    }
                    catch (Exception ex)
                    {
                        Log.Write("Unhandled exception during scheduling a Siteimprove page recheck operation " + url + Environment.NewLine + ex, ConfigurationPolicy.ErrorLog);
                    }
                });
            }
        }

        private bool IsCommittingTransaction(ExecutingEventArgs eventArgs)
        {
            return eventArgs.CommandName == "CommitTransaction" || eventArgs.CommandName == "FlushTransaction";
        }

        private bool IsCurrentHttpRequestForPublishing()
        {
            var url = HttpContext.Current.Request.Url.ToString();

            return (url.Contains("workflowOperation=Publish") || url.Contains("/batchPublishDraft/"));
        }

        private bool IsPageSaveRequest()
        {
            var url = HttpContext.Current.Request.Url.ToString();
            var httpMethod = HttpContext.Current.Request.HttpMethod.ToUpperInvariant();

            return httpMethod == "PUT" && url.Contains("PagesService.svc");
        }

        private bool IsUserSimulatedFromBackgroundTask()
        {
            var result = false;
            var currentUser = ClaimsManager.GetCurrentIdentity();
            if (currentUser != null)
                result = currentUser.AuthenticationType == "UserRegion";

            return result;
        }

        private bool IsTransactionTypeUpdating(PageDataProvider provider)
        {
            var result = false;
            if (this.PageData != null)
                result = provider.GetDirtyItemStatus(this.PageData) == SecurityConstants.TransactionActionType.Updated;

            return result;
        }
    }
}
