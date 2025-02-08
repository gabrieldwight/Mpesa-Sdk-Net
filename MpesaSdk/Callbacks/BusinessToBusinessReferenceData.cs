using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class BusinessToBusinessReferenceData
    {
        [JsonPropertyName("ReferenceItem")]
        public List<ReferenceItem> ReferenceItem { get; set; }
    }
}
