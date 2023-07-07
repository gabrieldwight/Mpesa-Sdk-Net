using Newtonsoft.Json;
using System;

namespace MpesaSdk.Callbacks
{
    public class TaxRemittanceCallback
    {
        [JsonProperty("Result")]
        public Result Result { get; set; }
    }

    public class Result
    {
        [JsonProperty("ResultType")]
        public string ResultType { get; set; }

        [JsonProperty("ResultCode")]
        public string ResultCode { get; set; }

        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }

        [JsonProperty("OriginatorConversationID")]
        public Guid OriginatorConversationId { get; set; }

        [JsonProperty("ConversationID")]
        public string ConversationId { get; set; }

        [JsonProperty("TransactionID")]
        public string TransactionId { get; set; }

        [JsonProperty("ResultParameters")]
        public TaxRemittanceCallbackMetadata TaxRemittanceCallbackMetadata { get; set; }

        [JsonProperty("ReferenceData")]
        public TaxReferenceCallbackMetadata TaxReferenceCallbackMetadata { get; set; }
    }
}
