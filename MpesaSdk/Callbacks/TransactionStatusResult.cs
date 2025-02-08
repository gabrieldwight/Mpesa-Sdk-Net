using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class TransactionStatusResult : MpesaBaseResult
    {
        [JsonPropertyName("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
