using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class PhoneNumberAgeRequest
	{
		[JsonPropertyName("customerNumber")]
		public string CustomerNumber { get; set; }
	}
}
