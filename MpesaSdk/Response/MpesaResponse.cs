using Newtonsoft.Json;

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
        [JsonProperty("ResponseCode")]
        public int ResponseCode { get; set; }
    }
}
