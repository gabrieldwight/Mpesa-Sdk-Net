using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class BusinessToCustomerResult : MpesaBaseResult
    {
        [JsonPropertyName("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
