using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class B2BExpressCheckoutCallback
    {
        [JsonProperty("resultCode")]
        public string ResultCode { get; set; }

        [JsonProperty("resultDesc")]
        public string ResultDesc { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("resultType")]
        public string ResultType { get; set; }

        [JsonProperty("conversationID")]
        public string ConversationId { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("paymentReference")]
        public string PaymentReference { get; set; }
    }
}
