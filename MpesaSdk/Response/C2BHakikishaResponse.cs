using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class C2BHakikishaResponse
	{
		[JsonPropertyName("requestId")]
		public string RequestId { get; set; }

		[JsonPropertyName("timestamp")]
		public long Timestamp { get; set; }

		[JsonPropertyName("accountName")]
		public string AccountName { get; set; }

		[JsonPropertyName("accountNumber")]
		public string AccountNumber { get; set; }

		[JsonPropertyName("shortcode")]
		public string Shortcode { get; set; }
	}
}
