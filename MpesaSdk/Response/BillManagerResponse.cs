using Newtonsoft.Json;

namespace MpesaSdk.Response
{
	public class BillManagerResponse
	{
		[JsonProperty("app_key")]
		public string AppKey { get; set; }

		[JsonProperty("resmsg")]
		public string Resmsg { get; set; }

		[JsonProperty("rescode")]
		public string Rescode { get; set; }

		[JsonProperty("Status_Message")]
		public string StatusMessage { get; set; }
	}
}
