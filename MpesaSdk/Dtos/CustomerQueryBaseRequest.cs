using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class CustomerQueryBaseRequest
	{
		[JsonPropertyName("msisdn")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Msisdn { get; set; }

		[JsonPropertyName("startDate")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string StartDate { get; set; }

		[JsonPropertyName("stopDate")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string StopDate { get; set; }

		[JsonPropertyName("vpnGroup")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string VpnGroup { get; set; }

		[JsonPropertyName("username")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Username { get; set; }

		[JsonPropertyName("assetName")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string AssetName { get; set; }
		
		[JsonPropertyName("product")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Product { get; set; }

		[JsonPropertyName("operation")]
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Operation { get; set; }
	}
}
