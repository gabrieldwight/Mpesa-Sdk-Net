using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
    public class PullTransactionRegisterUrl
    {
        /// <summary>
        /// Organization ShortCode that was used during Go-Live process.
        /// </summary>
        [JsonProperty("ShortCode")]
        public string ShortCode { get; set; }

        /// <summary>
        /// Defines the type of operation, default value is Pull.
        /// </summary>
        [JsonProperty("RequestType")]
        public string RequestType { get; set; } = "Pull";

        /// <summary>
        /// This is Safaricom MSISDN associated with the organization account using Pull API(07XXXXXXXX or 2547XXXXXXX).
        /// </summary>
        [JsonProperty("NominatedNumber")]
        public string NominatedNumber { get; set; }

        /// <summary>
        /// A CallBack URL is a valid secure URL that is used to receive notifications.
        /// </summary>
        [JsonProperty("CallBackURL")]
        public string CallBackURL { get; set; }
    }
}
