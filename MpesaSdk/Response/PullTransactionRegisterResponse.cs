using Newtonsoft.Json;

namespace MpesaSdk.Response
{
    public class PullTransactionRegisterResponse
    {
        [JsonProperty("ResponseRefID")]
        public string ResponseRefID { get; set; }

        [JsonProperty("ResponseStatus")]
        public string ResponseStatus { get; set; }

        [JsonProperty("ShortCode")]
        public string ShortCode { get; set; }

        [JsonProperty("ResponseDescription")]
        public string ResponseDescription { get; set; }
    }
}
