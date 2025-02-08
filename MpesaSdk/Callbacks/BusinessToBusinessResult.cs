using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class BusinessToBusinessResult : MpesaBaseResult
    {
        [JsonPropertyName("ReferenceData")]
        public BusinessToBusinessReferenceData ReferenceData { get; set; }
    }
}
