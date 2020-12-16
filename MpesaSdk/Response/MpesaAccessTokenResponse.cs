using Newtonsoft.Json;

namespace MpesaSdk.Response
{
    /// <summary>
    /// Mpesa Accesstoken data transfer object
    /// </summary>
    public class MpesaAccessTokenResponse
    {
        /// <summary>
        /// Access token to access other APIs
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; private set; }

        /// <summary>
        /// time token expires
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; private set; }
    }
}
