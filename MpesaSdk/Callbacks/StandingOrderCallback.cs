using Newtonsoft.Json;
using System.Collections.Generic;

namespace MpesaSdk.Callbacks
{
	public class StandingOrderCallback
	{
		[JsonProperty("responseHeader")]
		public ResponseHeader ResponseHeader { get; set; }

		[JsonProperty("ResponseBody")]
		public ResponseBody ResponseBody { get; set; }
	}

	public class ResponseHeader
	{
		[JsonProperty("responseRefID")]
		public string ResponseRefId { get; set; }

		[JsonProperty("requestRefID")]
		public string RequestRefId { get; set; }

		[JsonProperty("responseCode")]
		public long ResponseCode { get; set; }

		[JsonProperty("responseDescription")]
		public string ResponseDescription { get; set; }
	}

	public class ResponseBody
	{
		[JsonProperty("responseData")]
		public List<StandingOrderCallbackMetadataItem> ResponseData { get; set; }
	}

	public class StandingOrderCallbackMetadataItem
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("value")]
		public string Value { get; set; }
	}
}