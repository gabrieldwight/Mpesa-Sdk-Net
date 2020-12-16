using Newtonsoft.Json;
using System.Collections.Generic;

namespace MpesaSdk.Callbacks
{
    public class BusinessToBusinessReferenceData
    {
        [JsonProperty("ReferenceItem")]
        public List<ReferenceItem> ReferenceItem { get; set; }
    }
}
