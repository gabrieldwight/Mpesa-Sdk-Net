using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class AccountBalanceCallback
    {
        [JsonPropertyName("Result")]
        public AccountBalanceResult Result { get; set; }
    }
}
