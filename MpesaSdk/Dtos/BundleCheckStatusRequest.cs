using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BundleCheckStatusRequest
	{
		[JsonPropertyName("params")]
		public BundleParams Params { get; set; }
	}

	public class BundleParams
	{
		[JsonPropertyName("serviceAccountId")]
		public string ServiceAccountId { get; set; }
		
		[JsonPropertyName("id")]
		public string Id { get; set; }
	}
}
