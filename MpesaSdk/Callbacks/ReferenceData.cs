using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class ReferenceData
    {
        [JsonPropertyName("ReferenceItem")]
        public ReferenceItem ReferenceItem { get; set; }
    }
}
