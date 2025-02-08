using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class B2BExpressCheckoutCallback
    {
        [JsonPropertyName("resultCode")]
        public string ResultCode { get; set; }

        [JsonPropertyName("resultDesc")]
        public string ResultDesc { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        [JsonPropertyName("resultType")]
        public string ResultType { get; set; }

        [JsonPropertyName("conversationID")]
        public string ConversationId { get; set; }

        [JsonPropertyName("transactionId")]
        public string TransactionId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("paymentReference")]
        public string PaymentReference { get; set; }
    }
}
