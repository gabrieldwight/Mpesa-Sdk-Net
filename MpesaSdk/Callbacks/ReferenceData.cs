using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class ReferenceData
    {
        [JsonProperty("ReferenceItem")]
        public ReferenceItem ReferenceItem { get; set; }
    }
}
