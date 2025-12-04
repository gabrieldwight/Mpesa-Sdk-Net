using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class GetAllMessageRequest : CustomerQueryBaseRequest
	{
		[JsonPropertyName("pageNo")]
		public int PageNumber { get; set; }
		[JsonPropertyName("pageSize")]
		public int PageSize { get; set; }
	}
}
