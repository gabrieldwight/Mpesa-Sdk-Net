using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class ReversalResult : MpesaBaseResult
    {
        [JsonPropertyName("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
