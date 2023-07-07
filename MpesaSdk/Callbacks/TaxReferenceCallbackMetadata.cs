using Newtonsoft.Json;
using System.Collections.Generic;

namespace MpesaSdk.Callbacks
{
    public class TaxReferenceCallbackMetadata
    {
        [JsonProperty("ReferenceItem")]
        public List<TaxReferenceCallbackMetadataItem> ReferenceItem { get; set; }
    }

    public class TaxReferenceCallbackMetadataItem
    {
        [JsonProperty("Name")]
        public string Key { get; set; }
        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
