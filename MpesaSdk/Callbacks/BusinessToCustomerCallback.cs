using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// Callback to handle B2C result response
    /// </summary>
    public class BusinessToCustomerCallback
    {
        [JsonPropertyName("Result")]
        public BusinessToCustomerResult Result { get; set; }
    }
}
