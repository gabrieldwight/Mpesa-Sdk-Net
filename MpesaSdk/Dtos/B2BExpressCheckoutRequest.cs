using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
    public class B2BExpressCheckoutRequest
    {
        [JsonProperty("primaryShortCode")]
        public string PrimaryShortCode { get; set; }

        [JsonProperty("receiverShortCode")]
        public string ReceiverShortCode { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("paymentRef")]
        public string PaymentRef { get; set; }

        [JsonProperty("callbackUrl")]
        public string CallbackUrl { get; set; }

        [JsonProperty("partnerName")]
        public string PartnerName { get; set; }

        [JsonProperty("RequestRefID")]
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
