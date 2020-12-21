using Newtonsoft.Json;
using System.Collections.Generic;

namespace MpesaSdk.Callbacks
{
    public class MpesaBaseResult
    {
        [JsonProperty("ResultType")]
        public long ResultType { get; set; }
        [JsonProperty("ResultCode")]
        public long ResultCode { get; set; }
        [JsonProperty("ResultDesc")]
        public string ResultDesc { get; set; }
        [JsonProperty("OriginatorConversationID")]
        public string OriginatorConversationID { get; set; }
        [JsonProperty("ConversationID")]
        public string ConversationID { get; set; }
        [JsonProperty("TransactionID")]
        public string TransactionID { get; set; }
        [JsonProperty("ResultParameters")]
        public ResultParameters ResultParameters { get; set; }
    }

    public class ResultParameters
    {
        [JsonProperty("ResultParameter")]
        public List<ResultParameter> ResultParameter { get; set; }
    }

    public class ResultParameter
    {
        [JsonProperty("Key")]
        public string Key { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
