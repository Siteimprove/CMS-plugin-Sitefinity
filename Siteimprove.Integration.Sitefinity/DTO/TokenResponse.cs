using Newtonsoft.Json;

namespace Siteimprove.Integration.Sitefinity.DTO
{
    /// <summary>
    /// Use this DTO to handle successfull Token response from Siteimprove
    /// </summary>
    public class TokenResponse
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
