using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class MobileNumberValidationResponse
	{
		[JsonPropertyName("responseRefID")]
		public string ResponseRefID { get; set; }

		[JsonPropertyName("responseCode")]
		public string ResponseCode { get; set; }

		[JsonPropertyName("responseMessage")]
		public string ResponseMessage { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }
	}
}
