using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class ReversalCallback
    {
        [JsonPropertyName("Result")]
        public ReversalResult Result { get; set; }
    }
}
