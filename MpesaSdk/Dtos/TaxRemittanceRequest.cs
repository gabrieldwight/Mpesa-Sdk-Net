using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    public class TaxRemittanceRequest
    {
        [JsonPropertyName("Initiator")]
		[JsonInclude]
		public string Initiator { get; private set; }

        [JsonPropertyName("SecurityCredential")]
		[JsonInclude]
		public string SecurityCredential { get; private set; }

        [JsonPropertyName("Command ID")]
		[JsonInclude]
		public string CommandId { get; private set; } = Transaction_Type.PayTaxToKRA;

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

        [JsonPropertyName("Remarks")]
		[JsonInclude]
		public string Remarks { get; private set; }

        [JsonPropertyName("QueueTimeOutURL")]
		[JsonInclude]
		public string QueueTimeOutUrl { get; private set; }

        [JsonPropertyName("ResultURL")]
		[JsonInclude]
		public string ResultUrl { get; private set; }

        /// <summary>
        /// This API enables businesses to remit tax to Kenya Revenue Authority (KRA). To use this API, prior integration is required with KRA for tax declaration, payment registration number (PRN) generation, and exchange of other tax-related information.
        /// </summary>
        /// <param name="initiator"></param>
        /// <param name="securityCredential"></param>
        /// <param name="amount"></param>
        /// <param name="partyA"></param>
        /// <param name="partyB"></param>
        /// <param name="accountReference"></param>
        /// <param name="remarks"></param>
        /// <param name="queueTimeOutUrl"></param>
        /// <param name="resultUrl"></param>
        public TaxRemittanceRequest(string initiator, string securityCredential, string amount, string partyA, string partyB, string accountReference, string remarks, string queueTimeOutUrl, string resultUrl)
        {
            Initiator = initiator;
            SecurityCredential = securityCredential;
            Amount = amount;
            PartyA = partyA;
            PartyB = partyB; 
            AccountReference = accountReference;
            Remarks = remarks; 
            QueueTimeOutUrl = queueTimeOutUrl;
            ResultUrl = resultUrl;
        }
    }
}
