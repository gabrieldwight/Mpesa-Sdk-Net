using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class LipaNaMpesaBody
    {
        [JsonProperty("StkCallback")]
        public StkCallback StkCallback { get; set; }
    }
}
