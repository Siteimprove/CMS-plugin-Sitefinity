using Newtonsoft.Json;

namespace Siteimprove.Integration.Sitefinity.DTO
{
    /// <summary>
    /// Use this DTO to serialize request for a Page recheck
    /// </summary>
    public class RecheckPageRequest
    {
        public RecheckPageRequest(string url)
        {
            this.Url = url;
            this.Type = "recheck";
        }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
