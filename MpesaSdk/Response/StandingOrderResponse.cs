using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class StandingOrderResponse
	{
		[JsonPropertyName("ResponseHeader")]
		public ResponseHeader ResponseHeader { get; set; }

		[JsonPropertyName("ResponseBody")]
		public ResponseBody ResponseBody { get; set; }
	}

	public class ResponseBody
	{
		[JsonPropertyName("responseDescription")]
		public string ResponseDescription { get; set; }

		[JsonPropertyName("responseCode")]
		public string ResponseCode { get; set; }
	}

	public class ResponseHeader
	{
		[JsonPropertyName("responseRefID")]
		public string ResponseRefId { get; set; }

		[JsonPropertyName("responseCode")]
		public string ResponseCode { get; set; }

		[JsonPropertyName("responseDescription")]
		public string ResponseDescription { get; set; }

		[JsonPropertyName("ResultDesc")]
		public string ResultDesc { get; set; }
	}
}
