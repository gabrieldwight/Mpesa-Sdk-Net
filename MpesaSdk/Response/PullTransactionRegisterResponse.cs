using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
    public class PullTransactionRegisterResponse
    {
        [JsonPropertyName("ResponseRefID")]
        public string ResponseRefID { get; set; }

        [JsonPropertyName("ResponseStatus")]
        public string ResponseStatus { get; set; }

        [JsonPropertyName("ShortCode")]
        public string ShortCode { get; set; }

        [JsonPropertyName("ResponseDescription")]
        public string ResponseDescription { get; set; }
    }
}
