using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class BillManagerResponse
	{
		[JsonPropertyName("app_key")]
		public string AppKey { get; set; }

		[JsonPropertyName("resmsg")]
		public string Resmsg { get; set; }

		[JsonPropertyName("rescode")]
		public string Rescode { get; set; }

		[JsonPropertyName("Status_Message")]
		public string StatusMessage { get; set; }
	}
}
