using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    public class B2CAccountTopUpRequest
    {
        [JsonPropertyName("Initiator")]
		[JsonInclude]
		public string Initiator { get; private set; }

        [JsonPropertyName("SecurityCredential")]
		[JsonInclude]
		public string SecurityCredential { get; private set; }

        [JsonPropertyName("CommandID")]
		[JsonInclude]
		public string CommandId { get; private set; } = Transaction_Type.BusinessPayToBulk;

        [JsonPropertyName("SenderIdentifierType")]
		[JsonInclude]
		public string SenderIdentifierType { get; private set; } = "4";

        [JsonPropertyName("RecieverIdentifierType")]
		[JsonInclude]
		public string RecieverIdentifierType { get; private set; } = "4";

        [JsonPropertyName("Amount")]
		[JsonInclude]
		public string Amount { get; private set; }

        [JsonPropertyName("PartyA")]
		[JsonInclude]
		public string PartyA { get; private set; }

        [JsonPropertyName("PartyB")]
		[JsonInclude]
		public string PartyB { get; private set; }

        [JsonPropertyName("AccountReference")]
		[JsonInclude]
		public string AccountReference { get; private set; }

        [JsonPropertyName("Requester")]
		[JsonInclude]
		public string Requester { get; private set; }

        [JsonPropertyName("Remarks")]
		[JsonInclude]
		public string Remarks { get; private set; }

        [JsonPropertyName("QueueTimeOutURL")]
		[JsonInclude]
		public string QueueTimeOutUrl { get; private set; }

        [JsonPropertyName("ResultURL")]
		[JsonInclude]
		public string ResultUrl { get; private set; }

        public B2CAccountTopUpRequest(string initiator, string securityCredential, string amount, string partyA, string partyB, string accountReference, string requester, string remarks, string queueTimeOutUrl, string resultUrl)
        {
            Initiator = initiator;
            SecurityCredential = securityCredential;
            Amount = amount;
            PartyA = partyA;
            PartyB = partyB;
            AccountReference = accountReference;
            Requester = requester;
            Remarks = remarks;
            QueueTimeOutUrl = queueTimeOutUrl;
            ResultUrl = resultUrl;
        }
    }
}
