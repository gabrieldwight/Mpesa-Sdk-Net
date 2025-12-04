using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class SwapResponse
	{
		[JsonPropertyName("requestRefID")]
		public string RequestRefId { get; set; }

		[JsonPropertyName("responseCode")]
		public string ResponseCode { get; set; }

		[JsonPropertyName("responseDesc")]
		public string ResponseDesc { get; set; }

		[JsonPropertyName("lastSwapDate")]
		public string LastSwapDate { get; set; }
	}
}
