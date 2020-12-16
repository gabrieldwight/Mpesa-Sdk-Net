using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class BusinessToCustomerResult : MpesaBaseResult
    {
        [JsonProperty("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
