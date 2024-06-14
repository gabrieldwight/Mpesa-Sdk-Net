using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
    public class B2CAccountTopUpRequest
    {
        [JsonProperty("Initiator")]
        public string Initiator { get; private set; }

        [JsonProperty("SecurityCredential")]
        public string SecurityCredential { get; private set; }

        [JsonProperty("CommandID")]
        public string CommandId { get; private set; } = Transaction_Type.BusinessPayToBulk;

        [JsonProperty("SenderIdentifierType")]
        public string SenderIdentifierType { get; private set; } = "4";

        [JsonProperty("RecieverIdentifierType")]
        public string RecieverIdentifierType { get; private set; } = "4";

        [JsonProperty("Amount")]
        public string Amount { get; private set; }

        [JsonProperty("PartyA")]
        public string PartyA { get; private set; }

        [JsonProperty("PartyB")]
        public string PartyB { get; private set; }

        [JsonProperty("AccountReference")]
        public string AccountReference { get; private set; }

        [JsonProperty("Requester")]
        public string Requester { get; private set; }

        [JsonProperty("Remarks")]
        public string Remarks { get; private set; }

        [JsonProperty("QueueTimeOutURL")]
        public string QueueTimeOutUrl { get; private set; }

        [JsonProperty("ResultURL")]
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
