using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class LipaNaMpesaBody
    {
        [JsonPropertyName("StkCallback")]
        public StkCallback StkCallback { get; set; }
    }
}
