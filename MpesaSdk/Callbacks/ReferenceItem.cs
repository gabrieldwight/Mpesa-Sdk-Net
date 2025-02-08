using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class ReferenceItem
    {
        [JsonPropertyName("Key")]
        public string Key { get; set; }
        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }
}
