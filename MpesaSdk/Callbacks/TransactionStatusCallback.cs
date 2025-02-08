using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class TransactionStatusCallback
    {
        [JsonPropertyName("Result")]
        public TransactionStatusResult Result { get; set; }
    }
}
