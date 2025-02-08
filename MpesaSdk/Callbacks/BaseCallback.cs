using System;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class BaseCallback
    {
        [JsonPropertyName("Result")]
        public Result Result { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("ResultType")]
        public string ResultType { get; set; }

        [JsonPropertyName("ResultCode")]
        public string ResultCode { get; set; }

        [JsonPropertyName("ResultDesc")]
        public string ResultDesc { get; set; }

        [JsonPropertyName("OriginatorConversationID")]
        public Guid OriginatorConversationId { get; set; }

        [JsonPropertyName("ConversationID")]
        public string ConversationId { get; set; }

        [JsonPropertyName("TransactionID")]
        public string TransactionId { get; set; }

        [JsonPropertyName("ResultParameters")]
        public BaseResultCallbackMetadata ResultCallbackMetadata { get; set; }

        [JsonPropertyName("ReferenceData")]
        public BaseReferenceCallbackMetadata ReferenceCallbackMetadata { get; set; }
    }
}
