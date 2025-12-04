using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class IMSIResponse
	{
		[JsonPropertyName("requestRefID")]
		public string RequestRefId { get; set; }

		[JsonPropertyName("responseCode")]
		public string ResponseCode { get; set; }

		[JsonPropertyName("responseDesc")]
		public string ResponseDesc { get; set; }

		[JsonPropertyName("imsi")]
		public string Imsi { get; set; }

		[JsonPropertyName("lastSwapDate")]
		public string LastSwapDate { get; set; }

		[JsonPropertyName("msisdnRegistrationDate")]
		public string MsisdnRegistrationDate { get; set; }

		[JsonPropertyName("customerNumber")]
		public string CustomerNumber { get; set; }
	}
}
