using Newtonsoft.Json;

namespace MpesaSdk.Response
{
    /// <summary>
    /// This is a response obtained when making a query operation on stk push transaction.
    /// </summary>
    public class LipaNaMpesaQueryStkResponse
    {
        /// <summary>
        /// This is a Numeric status code that indicates the status of the transaction submission. 0 means successful submission and any other code means an error occured. 
        /// </summary>
        [JsonProperty("ResponseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// Response description is an acknowledment message from the API that gives the status of the request submission usualy maps to a specific ResponseCode value. It can be a Success submission message or an error description
        /// </summary>
        [JsonProperty("ResponseDescription")]
        public string ResponseDescription { get; set; }

        /// <summary>
        /// This is a global unique Identifier for any submited payment request.
        /// </summary>
        [JsonProperty("MerchantRequestID")]
        public string MerchantRequestID { get; set; }

        /// <summary>
        /// This is a global unique identifier of the processed checkout transaction request.
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
    }
}
