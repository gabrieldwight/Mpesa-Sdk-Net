using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class TransactionStatusCallback
    {
        [JsonProperty("Result")]
        public TransactionStatusResult Result { get; set; }
    }
}
