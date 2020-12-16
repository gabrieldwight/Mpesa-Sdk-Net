using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// This is the first child of the Body.
    /// </summary>
    public class StkCallback
    {
        /// <summary>
        /// This is a global unique Identifier for any submited payment request. This is the same value returned in the acknowledgement message of the initial request.
        /// </summary>
        [JsonProperty("MerchantRequestID")]
        public string MerchantRequestID { get; set; }

        /// <summary>
        /// This is a global unique identifier of the processed checkout transaction request. This is the same value returned in the acknowledgement message of the initial request.
        /// </summary>
        [JsonProperty("CheckoutRequestID")]
        public string CheckoutRequestID { get; set; }

        /// <summary>
        /// This is a numeric status code that indicates the status of the transaction processing. 0 means successful processing and any other code means an error occured or the transaction failed.
        /// </summary>
        [JsonProperty("ResultCode")]
        public string ResultCode { get; set; }

        /// <summary>
        /// Result description is a message from the API that gives the status of the request processing, usualy maps to a specific ResultCode value. It can be a Success processing message or an error description message.
        /// </summary>
        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }

        /// <summary>
        /// This is the JSON object that holds more details for the transaction. It is only returned for Successful transaction.
        /// </summary>
        [JsonProperty("CallbackMetadata")]
        public LipaNaMpesaCallbackMetadata LipaNaMPesaCallbackMetadata { get; set; }
    }
}
