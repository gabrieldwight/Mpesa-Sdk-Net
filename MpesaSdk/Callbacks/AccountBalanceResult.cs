using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class AccountBalanceResult : MpesaBaseResult
    {
        [JsonProperty("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
