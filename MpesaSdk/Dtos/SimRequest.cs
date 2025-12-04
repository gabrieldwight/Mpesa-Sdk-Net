using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class SimRequest
	{
		[JsonPropertyName("vpnGroup")]
		public List<string> VpnGroup { get; set; }

		[JsonPropertyName("startAtIndex")]
		public string StartAtIndex { get; set; }

		[JsonPropertyName("pageSize")]
		public string PageSize { get; set; }

		[JsonPropertyName("username")]
		public string Username { get; set; }
	}
}
