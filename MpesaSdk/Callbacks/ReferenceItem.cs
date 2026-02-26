using MpesaSdk.Converters;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
    public class ReferenceItem
    {
        [JsonPropertyName("Key")]
        public string Key { get; set; }
        [JsonPropertyName("Value")]
		[JsonConverter(typeof(FlexibleStringConverter))]
		public string Value { get; set; }
    }
}
