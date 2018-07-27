using System;
using System.Web;
using Telerik.Sitefinity.Pages.Model;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Implementations of this interface shall provide business logic for validating and resolving Urls
    /// </summary>
    public interface IUrlModel
    {
        bool IsUrlValid(string url);

        string ResolveDomainFrom(string url);

        string ResolveUrlFrom(Guid rootId, Guid pageId, string culture);

        string ResolveUrlFromSiteMapNode(SiteMapNode node);

        string ResolveDomainFromPageNode(PageNode node);

        string ResolveDomainBySiteId(Guid siteId);
    }
}
