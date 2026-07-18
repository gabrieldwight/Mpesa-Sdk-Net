using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class BundleCheckStatusResponse
	{
		[JsonPropertyName("responseId")]
		public string ResponseId { get; set; }

		[JsonPropertyName("responseDesc")]
		public string ResponseDesc { get; set; }

		[JsonPropertyName("responseStatus")]
		public string ResponseStatus { get; set; }

		[JsonPropertyName("responseCreated")]
		public string ResponseCreated { get; set; }
	}
}
