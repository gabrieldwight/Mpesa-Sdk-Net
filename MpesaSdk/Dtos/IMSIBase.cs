using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class IMSIBase
	{
		[JsonPropertyName("customerNumber")]
		public string CustomerNumber { get; set; }
	}
}
