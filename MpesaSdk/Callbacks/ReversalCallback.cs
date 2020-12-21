using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class ReversalCallback
    {
        [JsonProperty("Result")]
        public ReversalResult Result { get; set; }
    }
}
