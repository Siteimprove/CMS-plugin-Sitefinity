using System;
using System.Linq;
using System.Web;
using Telerik.Sitefinity.Multisite;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Services;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Web.UI.NavigationControls;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Business logic for Validating & Resolving Url
    /// </summary>
    public class UrlModel : IUrlModel
    {
        public bool IsUrlValid(string url)
        {
            Uri uriResult;
            bool isValidUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return isValidUrl;
        }

        public string ResolveDomainFrom(string url)
        {
            var result = string.Empty;
            Uri uri;
            if (this.IsUrlValid(url) && Uri.TryCreate(url, UriKind.Absolute, out uri))
            {
                result = uri.Scheme + Uri.SchemeDelimiter + uri.Host;
            }

            return result;
        }

        public string ResolveUrlFromSiteMapNode(SiteMapNode node)
        {
            if (node != null)
            {
                var url = NavigationUtilities.ResolveUrl(node, true);
                return UrlPath.ResolveUrl(url, true, true);
            }

            return null;
        }

        public string ResolveDomainFromPageNode(PageNode node)
        {
            if (node != null)
            {
                ISite site;
                if (SystemManager.CurrentContext.IsMultisiteMode)
                {
                    site = SystemManager.CurrentContext.MultisiteContext.GetSites().FirstOrDefault(s => s.SiteMapRootNodeId == node.RootNodeId);
                }
                else
                {
                    site = SystemManager.CurrentContext.CurrentSite;
                }

                var siteUrl = site.GetUri().AbsoluteUri;
                return siteUrl.TrimEnd('/');
            }

            return null;
        }

        public string ResolveDomainBySiteId(Guid siteId)
        {
            var domain = string.Empty;
            if (SystemManager.CurrentContext.IsMultisiteMode)
            {
                if (siteId != Guid.Empty)
                {
                    var currentSite = SystemManager.CurrentContext.MultisiteContext.GetSites().FirstOrDefault(s => s.Id == siteId);
                    domain = currentSite.GetUri().AbsoluteUri;
                }
                else
                {
                    domain = SystemManager.CurrentContext.MultisiteContext.CurrentSite.GetUri().AbsoluteUri;

                }
            }
            else
            {
                domain = SystemManager.CurrentContext.CurrentSite.GetUri().AbsoluteUri;
            }

            return domain;
        }
    }
}
