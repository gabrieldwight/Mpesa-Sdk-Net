using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class FilterMessageRequest
	{
		[JsonPropertyName("startDate")]
		public string StartDate { get; set; }

		[JsonPropertyName("endDate")]
		public string EndDate { get; set; }

		[JsonPropertyName("status")]
		public string Status { get; set; }
	}
}
