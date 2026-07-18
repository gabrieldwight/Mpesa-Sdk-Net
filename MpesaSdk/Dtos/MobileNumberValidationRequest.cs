using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class MobileNumberValidationRequest
	{
		[JsonPropertyName("requestRefID")]
		public string RequestRefID { get; set; }

		[JsonPropertyName("shortCode")]
		public string ShortCode { get; set; }

		[JsonPropertyName("msisdn")]
		public string Msisdn { get; set; }

		[JsonPropertyName("idType")]
		public string IdType { get; set; }

		[JsonPropertyName("idNumber")]
		public string IdNumber { get; set; }
	}
}
