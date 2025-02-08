using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class BaseResultCallbackMetadata
    {
        [JsonPropertyName("ResultParameter")]
        public List<BaseResultCallbackMetadataItem> ResultParameter { get; set; }
    }

    public class BaseResultCallbackMetadataItem
    {
        [JsonPropertyName("Name")]
        public string Key { get; set; }
        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }
}
