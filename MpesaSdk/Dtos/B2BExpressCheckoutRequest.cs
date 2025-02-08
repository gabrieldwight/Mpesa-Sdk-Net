using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    public class B2BExpressCheckoutRequest
    {
        [JsonPropertyName("primaryShortCode")]
        public string PrimaryShortCode { get; set; }

        [JsonPropertyName("receiverShortCode")]
        public string ReceiverShortCode { get; set; }

        [JsonPropertyName("amount")]
        public string Amount { get; set; }

        [JsonPropertyName("paymentRef")]
        public string PaymentRef { get; set; }

        [JsonPropertyName("callbackUrl")]
        public string CallbackUrl { get; set; }

        [JsonPropertyName("partnerName")]
        public string PartnerName { get; set; }

        [JsonPropertyName("RequestRefID")]
        public string RequestRefId { get; set; }

        public B2BExpressCheckoutRequest(string primaryShortCode, string receiverShortCode, string amount, string paymentRef, string callbackUrl, string partnerName, string requestRefId)
        {
            PrimaryShortCode = primaryShortCode;
            ReceiverShortCode = receiverShortCode;
            Amount = amount;
            PaymentRef = paymentRef;
            CallbackUrl = callbackUrl;
            PartnerName = partnerName;
            RequestRefId = requestRefId;
        }
    }
}
