using System;
using Newtonsoft.Json;
using System.Net.Http;
using Siteimprove.Integration.Sitefinity.DTO;
using Siteimprove.Integration.Sitefinity.Mvc.Models;
using Telerik.Sitefinity.Localization;
using Siteimprove.Integration.Sitefinity.Resources;

namespace Siteimprove.Integration.Sitefinity.Infrastructure
{
    /// <summary>
    /// Simple Http client tuned to communicate with Siteimprove's API
    /// </summary>
    public class SiteimproveHttpClient
    {
        private static HttpClient _httpClient = new HttpClient();
        private ITokenModel _tokenModel;
        private IUrlModel _urlModel;

        public ITokenModel TokenModel
        {
            get
            {
                if (this._tokenModel == null)
                    this._tokenModel = new TokenModel();

                return this._tokenModel;
            }
        }

        public IUrlModel UrlModel
        {
            get
            {
                if (this._urlModel == null)
                    this._urlModel = new UrlModel();

                return this._urlModel;
            }
        }

        public void RecheckUrl(string url)
        {
            try
            {
                var payload = new RecheckPageRequest(url);
                payload.Token = this.TokenModel.GetTokenCreateIfNull(this.UrlModel.ResolveDomainFrom(url));
                var responseMessage = _httpClient.PostAsJsonAsync(_recheckEndpoint, payload).Result;
                var responseString = responseMessage.Content.ReadAsStringAsync().Result;

                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception(string.Format(Res.Get<SiteimproveResources>().ErrorRecheckPageResponse, url, _recheckEndpoint, responseMessage.StatusCode, responseString));
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format(Res.Get<SiteimproveResources>().ErrorRecheckPageException, url, _recheckEndpoint);
                throw new HttpRequestException(errorMessage, ex);
            }
        }

        public string FetchToken()
        {
            TokenResponse token = new TokenResponse();
            try
            {
                var responseMessage = _httpClient.GetAsync(_tokenEndpoint).Result;
                var content = responseMessage.Content.ReadAsStringAsync().Result;

                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception(string.Format(Res.Get<SiteimproveResources>().ErrorFetchTokenResponse, _tokenEndpoint, responseMessage.StatusCode, content));

                token = JsonConvert.DeserializeObject<TokenResponse>(content);
                return token.Token;
            }
            catch (Exception ex)
            {
                var errorMessage = Res.Get<SiteimproveResources>().ErrorFetchTokenException;
                throw new HttpRequestException(errorMessage, ex);
            }
        }

        private static readonly string _recheckEndpoint = "https://api-gateway.siteimprove.com/cms-recheck";
        private static readonly string _tokenEndpoint = "https://my2.siteimprove.com/auth/token?cms=ProgressSitefinityCMS";
    }
}
