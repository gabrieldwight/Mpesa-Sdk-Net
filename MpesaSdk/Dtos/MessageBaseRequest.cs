using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class MessageBaseRequest
	{
		[JsonPropertyName("searchValue")]
		public string SearchValue { get; set; }
	}
}
