using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class AccountBalanceResult : MpesaBaseResult
    {
        [JsonPropertyName("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
