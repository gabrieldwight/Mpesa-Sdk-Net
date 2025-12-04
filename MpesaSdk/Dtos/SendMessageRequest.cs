using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class SendMessageRequest : CustomerQueryBaseRequest
	{
		[JsonPropertyName("message")]
		public string Message { get; set; }
	}
}
