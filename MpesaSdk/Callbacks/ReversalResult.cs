using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class ReversalResult : MpesaBaseResult
    {
        [JsonProperty("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
