using Newtonsoft.Json;
using System.Collections.Generic;

namespace MpesaSdk.Callbacks
{
    public class BaseReferenceCallbackMetadata
    {
        [JsonProperty("ReferenceItem")]
        public List<BaseReferenceCallbackMetadataItem> ReferenceItem { get; set; }
    }

    public class BaseReferenceCallbackMetadataItem
    {
        [JsonProperty("Name")]
        public string Key { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
