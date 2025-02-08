using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Callbacks
{
	public class StandingOrderCallback
	{
		[JsonPropertyName("responseHeader")]
		public ResponseHeader ResponseHeader { get; set; }

		[JsonPropertyName("ResponseBody")]
		public ResponseBody ResponseBody { get; set; }
	}

	public class ResponseHeader
	{
		[JsonPropertyName("responseRefID")]
		public string ResponseRefId { get; set; }

		[JsonPropertyName("requestRefID")]
		public string RequestRefId { get; set; }

		[JsonPropertyName("responseCode")]
		public long ResponseCode { get; set; }

		[JsonPropertyName("responseDescription")]
		public string ResponseDescription { get; set; }
	}

	public class ResponseBody
	{
		[JsonPropertyName("responseData")]
		public List<StandingOrderCallbackMetadataItem> ResponseData { get; set; }
	}

	public class StandingOrderCallbackMetadataItem
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
		[JsonPropertyName("value")]
		public string Value { get; set; }
	}
}