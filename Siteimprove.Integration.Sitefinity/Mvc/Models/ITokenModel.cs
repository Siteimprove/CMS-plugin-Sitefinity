using Siteimprove.Integration.Sitefinity.Configuration;

namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Implementations of this interface shall handle the Siteimprove token related logic in Sitefinity
    /// </summary>
    public interface ITokenModel
    {
        bool TryGetToken(string domain, out TokenConfigElement tokenElement);

        TokenConfigElement SetToken(string token, string domain);

        string GetTokenCreateIfNull(string domain);
    }
}
