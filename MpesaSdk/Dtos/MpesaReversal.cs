﻿using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    /// <summary>
    /// Mpesa Transaction reversal data transfer object
    /// </summary>
    public class MpesaReversal
    {
        /// <summary>
        /// The name of Initiator to initiating  the request
        /// </summary>
        [JsonPropertyName("Initiator")]
		[JsonInclude]
		public string Initiator { get; private set; }

        /// <summary>
        /// Encrypted password for the initiator to authenticate the transaction request.
        /// Use <c>Credentials.EncryptPassword</c> method available under MpesaLib.Helpers to encrypt the password.
        /// </summary>
        [JsonPropertyName("SecurityCredential")]
		[JsonInclude]
		public string SecurityCredential { get; private set; }

        /// <summary>
        /// Takes only 'TransactionReversal' Command id.
        /// The default value has been set to that so you don't have to set this property.
        /// </summary>
        [JsonPropertyName("CommandID")]
        [JsonInclude]
        public string CommandID { get; private set; } = Transaction_Type.TransactionReversal;

        /// <summary>
        /// Unique identifier to identify a transaction on M-Pesa. e.g LKXXXX1234
        /// </summary>
        [JsonPropertyName("TransactionID")]
        [JsonInclude]
        public string TransactionID { get; private set; }


		/// <summary>
		/// This is the Amount transacted, normally a numeric value. Money that customer pays to the Shorcode. 
		/// Only whole numbers are supported.
		/// </summary>
		[JsonPropertyName("Amount")]
		[JsonInclude]
		public string Amount { get; private set; }

		/// <summary>
		/// Organization receiving the transaction (Shortcode)
		/// </summary>
		[JsonPropertyName("ReceiverParty")]
		[JsonInclude]
		public string ReceiverParty { get; private set; }

        /// <summary>
        /// Type of organization receiving the transaction.
        /// 11 - Organization Identifier on M-Pesa
        /// </summary>
        [JsonPropertyName("RecieverIdentifierType")]
		[JsonInclude]
		public string RecieverIdentifierType { get; private set; }

        /// <summary>
        /// Comments that are sent along with the transaction. (Upto 100 characters)
        /// </summary>
        [JsonPropertyName("Remarks")]
		[JsonInclude]
		public string Remarks { get; private set; }

        /// <summary>
        /// The path that stores information of time out transaction.
        /// </summary>
        [JsonPropertyName("QueueTimeOutURL")]
		[JsonInclude]
		public string QueueTimeOutURL { get; private set; }

        /// <summary>
        /// The path that stores information of transaction 
        /// </summary>
        [JsonPropertyName("ResultURL")]
		[JsonInclude]
		public string ResultURL { get; private set; }

        /// <summary>
        /// Optional Parameter (upto 100 characters)
        /// </summary>
        [JsonPropertyName("Occasion")]
		[JsonInclude]
		public string Occasion { get; private set; }

        /// <summary>
        /// Transaction reversal data transfer object
        /// </summary>
        /// <param name="initiator">The name of Initiator to initiating  the request</param>
        /// <param name="securityCredential">
        /// Encrypted password for the initiator to authenticate the transaction request.
        /// Use <c>Credentials.EncryptPassword</c> method available under MpesaLib.Helpers to encrypt the password.
        /// </param>
        /// <param name="transactionId">Unique identifier to identify a transaction on M-Pesa. e.g LKXXXX1234</param>
        /// <param name="amount">The amount used to pay for the transaction.</param>
        /// <param name="receiverparty">Organization receiving the transaction (Shortcode)</param>
        /// <param name="receiverIdentifierType">
        /// Type of organization receiving the transaction.
        /// 11 - Organization Identifier on M-Pesa
        /// </param>
        /// <param name="remarks">Comments that are sent along with the transaction. (Upto 100 characters)</param>
        /// <param name="queueTimeoutUrl">The path that stores information of time out transaction.</param>
        /// <param name="resultUrl">The path that stores information of transaction </param>
        /// <param name="occasion"> Optional Parameter (upto 100 characters)</param>
        public MpesaReversal(string initiator, string securityCredential, string transactionId, string amount, string receiverparty, string receiverIdentifierType, string remarks, string queueTimeoutUrl, string resultUrl, string occasion)
        {
            Initiator = initiator;
            SecurityCredential = securityCredential;
            CommandID = Transaction_Type.TransactionReversal;
            TransactionID = transactionId;
            Amount = amount;
            ReceiverParty = receiverparty;
            RecieverIdentifierType = receiverIdentifierType;
            Remarks = remarks;
            QueueTimeOutURL = queueTimeoutUrl;
            ResultURL = resultUrl;
            Occasion = occasion;
        }
    }
}
