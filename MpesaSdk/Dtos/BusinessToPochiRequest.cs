using System;
using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
	public class BusinessToPochiRequest
	{
		[JsonPropertyName("OriginatorConversationID")]
		public string OriginatorConversationId { get; set; }

		[JsonPropertyName("InitiatorName")]
		public string InitiatorName { get; set; }

		[JsonPropertyName("SecurityCredential")]
		public string SecurityCredential { get; set; }

		[JsonPropertyName("CommandID")]
		public string CommandId { get; set; }

		[JsonPropertyName("Amount")]
		public string Amount { get; set; }

		[JsonPropertyName("PartyA")]
		public string PartyA { get; set; }

		[JsonPropertyName("PartyB")]
		public string PartyB { get; set; }

		[JsonPropertyName("Remarks")]
		public string Remarks { get; set; }

		[JsonPropertyName("QueueTimeOutURL")]
		public string QueueTimeOutUrl { get; set; }

		[JsonPropertyName("ResultURL")]
		public string ResultUrl { get; set; }

		[JsonPropertyName("Occassion")]
		public string Occassion { get; set; }
	}
}
