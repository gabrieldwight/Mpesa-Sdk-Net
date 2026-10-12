using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class C2BHakikishaRequest
	{
		[JsonPropertyName("requestId")]
		public string RequestId { get; set; }

		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }

		[JsonPropertyName("accountNumber")]
		public string AccountNumber { get; set; }

		[JsonPropertyName("shortcode")]
		public string Shortcode { get; set; }
	}
}
