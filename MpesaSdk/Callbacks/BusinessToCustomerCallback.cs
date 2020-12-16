using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// Callback to handle B2C result response
    /// </summary>
    public class BusinessToCustomerCallback
    {
        [JsonProperty("Result")]
        public BusinessToCustomerResult Result { get; set; }
    }
}
