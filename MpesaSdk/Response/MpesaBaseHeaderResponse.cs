using System;
using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class MpesaBaseHeaderResponse
	{
		[JsonPropertyName("requestRefId")]
		public string RequestRefId { get; set; }

		[JsonPropertyName("responseCode")]
		public int ResponseCode { get; set; }

		[JsonPropertyName("responseMessage")]
		public string ResponseMessage { get; set; }

		[JsonPropertyName("customerMessage")]
		public string CustomerMessage { get; set; }

		[JsonPropertyName("timestamp")]
		public DateTime Timestamp { get; set; }
	}
}
