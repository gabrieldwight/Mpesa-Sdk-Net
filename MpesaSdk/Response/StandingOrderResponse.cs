using Newtonsoft.Json;

namespace MpesaSdk.Response
{
	public class StandingOrderResponse
	{
		[JsonProperty("ResponseHeader")]
		public ResponseHeader ResponseHeader { get; set; }

		[JsonProperty("ResponseBody")]
		public ResponseBody ResponseBody { get; set; }
	}

	public class ResponseBody
	{
		[JsonProperty("responseDescription")]
		public string ResponseDescription { get; set; }

		[JsonProperty("responseCode")]
		public string ResponseCode { get; set; }
	}

	public class ResponseHeader
	{
		[JsonProperty("responseRefID")]
		public string ResponseRefId { get; set; }

		[JsonProperty("responseCode")]
		public string ResponseCode { get; set; }

		[JsonProperty("responseDescription")]
		public string ResponseDescription { get; set; }

		[JsonProperty("ResultDesc")]
		public string ResultDesc { get; set; }
	}
}
