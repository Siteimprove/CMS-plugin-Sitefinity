namespace Siteimprove.Integration.Sitefinity.Mvc.Models
{
    /// <summary>
    /// Implementations of this interface shall provide business logic for validating and resolvign Urls
    /// </summary>
    public interface IUrlModel
    {
        bool IsUrlValid(string url);

        string ResolveDomainFrom(string url);
    }
}
