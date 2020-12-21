using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class TransactionStatusResult : MpesaBaseResult
    {
        [JsonProperty("ReferenceData")]
        public ReferenceData ReferenceData { get; set; }
    }
}
