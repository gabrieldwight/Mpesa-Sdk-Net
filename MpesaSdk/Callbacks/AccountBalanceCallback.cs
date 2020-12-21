using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    public class AccountBalanceCallback
    {
        [JsonProperty("Result")]
        public AccountBalanceResult Result { get; set; }
    }
}
