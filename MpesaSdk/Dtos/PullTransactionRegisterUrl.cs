using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    public class PullTransactionRegisterUrl
    {
        /// <summary>
        /// Organization ShortCode that was used during Go-Live process.
        /// </summary>
        [JsonPropertyName("ShortCode")]
        public string ShortCode { get; set; }

        /// <summary>
        /// Defines the type of operation, default value is Pull.
        /// </summary>
        [JsonPropertyName("RequestType")]
        public string RequestType { get; set; } = "Pull";

        /// <summary>
        /// This is Safaricom MSISDN associated with the organization account using Pull API(07XXXXXXXX or 2547XXXXXXX).
        /// </summary>
        [JsonPropertyName("NominatedNumber")]
        public string NominatedNumber { get; set; }

        /// <summary>
        /// A CallBack URL is a valid secure URL that is used to receive notifications.
        /// </summary>
        [JsonPropertyName("CallBackURL")]
        public string CallBackURL { get; set; }

        /// <summary>
        /// Pull Transaction Register Url data transfer object
        /// </summary>
        /// <param name="shortCode">
        /// Organization ShortCode that was used during Go-Live process.
        /// </param>
        /// <param name="nominatedNumber">
        /// This is Safaricom MSISDN associated with the organization account using Pull API(07XXXXXXXX or 2547XXXXXXX).
        /// </param>
        /// <param name="callBackURL">A CallBack URL is a valid secure URL that is used to receive notifications.</param>
        public PullTransactionRegisterUrl(string shortCode, string nominatedNumber, string callBackURL)
        {
            ShortCode = shortCode;
            RequestType = "Pull";
            NominatedNumber = nominatedNumber;
            CallBackURL = callBackURL;
        }
    }
}
