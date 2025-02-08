using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class BaseReferenceCallbackMetadata
    {
        [JsonPropertyName("ReferenceItem")]
        public List<BaseReferenceCallbackMetadataItem> ReferenceItem { get; set; }
    }

    public class BaseReferenceCallbackMetadataItem
    {
        [JsonPropertyName("Name")]
        public string Key { get; set; }
        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }
}
