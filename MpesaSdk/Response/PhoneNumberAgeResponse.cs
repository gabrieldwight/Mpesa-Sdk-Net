using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class PhoneNumberAgeResponse
	{
		[JsonPropertyName("requestRefID")]
		public string RequestRefID { get; set; }

		[JsonPropertyName("responseCode")]
		public string ResponseCode { get; set; }

		[JsonPropertyName("responseDesc")]
		public string ResponseDesc { get; set; }

		[JsonPropertyName("msisdnRegistrationDate")]
		public string MsisdnRegistrationDate { get; set; }

		[JsonPropertyName("customerNumber")]
		public string CustomerNumber { get; set; }
	}
}
