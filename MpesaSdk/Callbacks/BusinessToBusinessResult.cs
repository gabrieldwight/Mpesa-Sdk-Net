using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class BusinessToBusinessResult : MpesaBaseResult
    {
        [JsonProperty("ReferenceData")]
        public BusinessToBusinessReferenceData ReferenceData { get; set; }
    }
}
