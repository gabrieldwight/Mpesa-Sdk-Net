using System;
using System.Text.Json.Serialization;

namespace MpesaSdk.Response
{
	public class LipaNaBongaResponse
	{
		[JsonPropertyName("header")]
		public LipaNaBongaHeader Header { get; set; }
		
		[JsonPropertyName("body")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public LipaNaBongaResponseBody Body { get; set; }
	}

	public class LipaNaBongaHeader : MpesaBaseHeaderResponse
	{
		
	}

	public class LipaNaBongaResponseBody
	{
		[JsonPropertyName("amount")]
		public int Amount { get; set; }

		[JsonPropertyName("points")]
		public int Points { get; set; }

		[JsonPropertyName("rate")]
		public decimal Rate { get; set; }
	}
}
