using System.Text.Json.Serialization;

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
        [JsonPropertyName("access_token")]
		[JsonInclude]
		public string AccessToken { get; private set; }

        /// <summary>
        /// time token expires
        /// </summary>
        [JsonPropertyName("expires_in")]
		[JsonInclude]
		public string ExpiresIn { get; private set; }
    }
}
