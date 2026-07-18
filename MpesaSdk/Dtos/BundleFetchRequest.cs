using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BundleFetchRequest
	{
		[JsonPropertyName("params")]
		public Params Params { get; set; }
	}

	public class Params
	{
		[JsonPropertyName("msisdn")]
		public string Msisdn { get; set; }
	}
}
