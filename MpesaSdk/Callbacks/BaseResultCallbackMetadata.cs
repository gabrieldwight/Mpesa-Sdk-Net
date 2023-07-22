using Newtonsoft.Json;
using System.Collections.Generic;

namespace MpesaSdk.Callbacks
{
    public class BaseResultCallbackMetadata
    {
        [JsonProperty("ResultParameter")]
        public List<BaseResultCallbackMetadataItem> ResultParameter { get; set; }
    }

    public class BaseResultCallbackMetadataItem
    {
        [JsonProperty("Name")]
        public string Key { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
