using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class MpesaBaseResult
    {
        [JsonPropertyName("ResultType")]
        public long ResultType { get; set; }
        [JsonPropertyName("ResultCode")]
        public long ResultCode { get; set; }
        [JsonPropertyName("ResultDesc")]
        public string ResultDesc { get; set; }
        [JsonPropertyName("OriginatorConversationID")]
        public string OriginatorConversationID { get; set; }
        [JsonPropertyName("ConversationID")]
        public string ConversationID { get; set; }
        [JsonPropertyName("TransactionID")]
        public string TransactionID { get; set; }
        [JsonPropertyName("ResultParameters")]
        public ResultParameters ResultParameters { get; set; }
    }

    public class ResultParameters
    {
        [JsonPropertyName("ResultParameter")]
        public List<ResultParameter> ResultParameter { get; set; }
    }

    public class ResultParameter
    {
        [JsonPropertyName("Key")]
        public string Key { get; set; }
        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }
}
