using Newtonsoft.Json;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// Callback to handle the results for B2B response
    /// </summary>
    public class BusinessToBusinessCallback
    {
        [JsonProperty("Result")]
        public BusinessToBusinessResult Result { get; set; }
    }
}
