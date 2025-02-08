using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
    /// <summary>
    /// MpesaResponse inherited from the base response to display the results obtained from the Mpesa Api.
    /// </summary>
    public class MpesaResponse : MpesaBaseResponse
    {
        /// <summary>
        /// Gets or sets the response code.
        /// </summary>
        /// <value>The response code.</value>
        [JsonPropertyName("ResponseCode")]
        public string ResponseCode { get; set; }
    }
}
