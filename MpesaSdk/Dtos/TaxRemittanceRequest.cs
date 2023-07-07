using Newtonsoft.Json;

namespace MpesaSdk.Dtos
{
    public class TaxRemittanceRequest
    {
        [JsonProperty("Initiator")]
        public string Initiator { get; private set; }

        [JsonProperty("SecurityCredential")]
        public string SecurityCredential { get; private set; }

        [JsonProperty("Command ID")]
        public string CommandId { get; private set; } = Transaction_Type.PayTaxToKRA;

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

        [JsonProperty("Remarks")]
        public string Remarks { get; private set; }

        [JsonProperty("QueueTimeOutURL")]
        public string QueueTimeOutUrl { get; private set; }

        [JsonProperty("ResultURL")]
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
