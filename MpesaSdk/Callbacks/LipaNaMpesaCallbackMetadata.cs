using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    /// <summary>
    /// This is the JSON object that holds more details for the transaction. It is only returned for Successful transaction.
    /// </summary>
    public class LipaNaMpesaCallbackMetadata
    {
        /// <summary>
        /// This is a JSON Array, within the CallbackMetadata, that holds additional transaction details in JSON objects. Since this array is returned under the CallbackMetadata, it is only returned for Successful transaction.
        /// </summary>
        [JsonPropertyName("Item")]
        public List<CallbackMetadataItem> ResultParameter { get; set; }
    }

    public class CallbackMetadataItem
    {
        [JsonPropertyName("Name")]
        public string Key { get; set; }
        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }
}
