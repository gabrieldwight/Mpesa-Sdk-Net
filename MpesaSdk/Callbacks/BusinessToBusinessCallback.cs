using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// Callback to handle the results for B2B response
    /// </summary>
    public class BusinessToBusinessCallback
    {
        [JsonPropertyName("Result")]
        public BusinessToBusinessResult Result { get; set; }
    }
}
