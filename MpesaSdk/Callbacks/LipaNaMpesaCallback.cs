using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// This is the root key for the entire Callback message.
    /// </summary>
    public class LipaNaMpesaCallback
    {
        [JsonProperty("Body")]
        public LipaNaMpesaBody Body { get; set; }
    }
}
