using System;

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
    }
}
