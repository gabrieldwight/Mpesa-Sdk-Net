using Newtonsoft.Json;
using System.Collections.Generic;

namespace MpesaSdk.Callbacks
{
    public class TaxRemittanceCallbackMetadata
    {
        [JsonProperty("ResultParameter")]
        public List<TaxRemittanceCallbackMetadataItem> ResultParameter { get; set; }
    }

    public class TaxRemittanceCallbackMetadataItem
    {
        [JsonProperty("Name")]
        public string Key { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
