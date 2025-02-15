﻿using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    /// <summary>
    /// B2B Data transfer object
    /// </summary>
    public class BusinessToBusiness
    {
        /// <summary>
        /// This is the credential/username used to authenticate the transaction request.
        /// </summary>
        [JsonPropertyName("Initiator")]
		[JsonInclude]
		public string Initiator { get; private set; }

        /// <summary>
        /// Base64 encoded string of the Security Credential, which is encrypted using M-Pesa public key and 
        /// validates the transaction on M-Pesa Core system.
        /// </summary>
        [JsonPropertyName("SecurityCredential")]
		[JsonInclude]
		public string SecurityCredential { get; private set; }

        /// <summary>
        /// Unique command for each transaction type, possible values are: BusinessPayBill, MerchantToMerchantTransfer,
        /// MerchantTransferFromMerchantToWorking, MerchantServicesMMFAccountTransfer, AgencyFloatAdvance
        /// </summary>
        [JsonPropertyName("CommandID")]
		[JsonInclude]
		public string CommandID { get; private set; }

        /// <summary>
        /// Type of organization sending the transaction.
        /// </summary>
        [JsonPropertyName("SenderIdentifierType")]
		[JsonInclude]
		public string SenderIdentifierType { get; private set; }

        /// <summary>
        /// Type of organization receiving the funds being transacted.
        /// </summary>
        [JsonPropertyName("RecieverIdentifierType")]
		[JsonInclude]
		public string RecieverIdentifierType { get; private set; }

        /// <summary>
        /// The amount being transacted.
        /// </summary>
        [JsonPropertyName("Amount")]
		[JsonInclude]
		public string Amount { get; private set; }

        /// <summary>
        /// Organization’s short code initiating the transaction.
        /// </summary>
        [JsonPropertyName("PartyA")]
		[JsonInclude]
		public string PartyA { get; private set; }

        /// <summary>
        /// Organization’s short code receiving the funds being transacted.
        /// </summary>
        [JsonPropertyName("PartyB")]
		[JsonInclude]
		public string PartyB { get; private set; }

        /// <summary>
        /// Account Reference mandatory for “BusinessPaybill” CommandID.
        /// </summary>
        [JsonPropertyName("AccountReference")]
		[JsonInclude]
		public string AccountReference { get; private set; }

        /// <summary>
        /// Comments that are sent along with the transaction.
        /// </summary>
        [JsonPropertyName("Remarks")]
		[JsonInclude]
		public string Remarks { get; private set; }

        /// <summary>
        /// The path that stores information of time out transactions. It should be properly validated to 
        /// make sure that it contains the port, URI and domain name or publicly available IP.
        /// </summary>
        [JsonPropertyName("QueueTimeOutURL")]
		[JsonInclude]
		public string QueueTimeOutURL { get; private set; }

        /// <summary>
        /// The path that receives results from M-Pesa. It should be properly validated to make sure 
        /// that it contains the port, URI and domain name or publicly available IP.
        /// </summary>
        [JsonPropertyName("ResultURL")]
		[JsonInclude]
		public string ResultURL { get; private set; }

        /// <summary>
        /// B2B data transfer object
        /// </summary>
        /// <param name="initiator">This is the credential/username used to authenticate the transaction request.</param>
        /// <param name="securityCredential">
        /// Base64 encoded string of the Security Credential, which is encrypted using M-Pesa public key and 
        /// validates the transaction on M-Pesa Core system.
        /// </param>
        /// <param name="commandId">
        /// Unique command for each transaction type, possible values are: BusinessPayBill, MerchantToMerchantTransfer,
        /// MerchantTransferFromMerchantToWorking, MerchantServicesMMFAccountTransfer, AgencyFloatAdvance
        /// </param>
        /// <param name="amount">The amount being transacted.</param>
        /// <param name="partyA">Organization’s short code initiating the transaction.</param>
        /// <param name="partyB">Organization’s short code receiving the funds being transacted.</param>
        /// <param name="senderIdentifierType">Type of organization sending the transaction.</param>
        /// <param name="receiverIdentifierType">Type of organization receiving the transaction.</param>
        /// <param name="accountReference">Account Reference mandatory for “BusinessPaybill” CommandID.</param>
        /// <param name="remarks">Comments that are sent along with the transaction.</param>
        /// <param name="queueTimeoutUrl">
        /// The path that stores information of time out transactions. It should be properly validated to 
        /// make sure that it contains the port, URI and domain name or publicly available IP.
        /// </param>
        /// <param name="resultUrl">
        /// The path that receives results from M-Pesa. It should be properly validated to make sure 
        /// that it contains the port, URI and domain name or publicly available IP.
        /// </param>
        public BusinessToBusiness(string initiator, string securityCredential, string commandId, string amount, string partyA, string partyB, string senderIdentifierType, string receiverIdentifierType, string accountReference, string remarks, string queueTimeoutUrl, string resultUrl)
        {
            Initiator = initiator;
            SecurityCredential = securityCredential;
            CommandID = commandId;
            Amount = amount;
            PartyA = partyA;
            PartyB = partyB;
            SenderIdentifierType = senderIdentifierType;
            RecieverIdentifierType = receiverIdentifierType;
            AccountReference = accountReference;
            Remarks = remarks;
            QueueTimeOutURL = queueTimeoutUrl;
            ResultURL = resultUrl;
        }

        protected BusinessToBusiness()
        {

        }
    }
}
