using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class DeleteMessageRequest
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
	}
}
