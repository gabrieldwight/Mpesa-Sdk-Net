using Newtonsoft.Json;

namespace MpesaSdk.Response
{
    public class B2BExpressCheckoutResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
