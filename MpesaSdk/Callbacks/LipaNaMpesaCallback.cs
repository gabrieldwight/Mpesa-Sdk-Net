using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// This is the root key for the entire Callback message.
    /// </summary>
    public class LipaNaMpesaCallback
    {
        [JsonPropertyName("Body")]
        public LipaNaMpesaBody Body { get; set; }
    }
}
