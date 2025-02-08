using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
    public class B2BExpressCheckoutResponse
    {
        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
