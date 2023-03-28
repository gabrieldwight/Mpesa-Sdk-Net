using Newtonsoft.Json;

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
        [JsonProperty("Initiator")]
        public string Initiator { get; private set; }

        /// <summary>
        /// Encrypted password for the initiator to authenticate the transaction request.
        /// Use <c>Credentials.EncryptPassword</c> method available under MpesaLib.Helpers to encrypt the password.
        /// </summary>
        [JsonProperty("SecurityCredential")]
        public string SecurityCredential { get; private set; }

        /// <summary>
        /// Takes only 'TransactionReversal' Command id.
        /// The default value has been set to that so you don't have to set this property.
        /// </summary>
        [JsonProperty("CommandID")]
        public string CommandID { get; private set; } = Transaction_Type.TransactionReversal;

        /// <summary>
        /// Unique identifier to identify a transaction on M-Pesa. e.g LKXXXX1234
        /// </summary>
        [JsonProperty("TransactionID")]
        public string TransactionID { get; private set; }


		/// <summary>
		/// This is the Amount transacted, normally a numeric value. Money that customer pays to the Shorcode. 
		/// Only whole numbers are supported.
		/// </summary>
		[JsonProperty("Amount")]
		public string Amount { get; private set; }

		/// <summary>
		/// Organization receiving the transaction (Shortcode)
		/// </summary>
		[JsonProperty("ReceiverParty")]
        public string ReceiverParty { get; private set; }

        /// <summary>
        /// Type of organization receiving the transaction.
        /// 11 - Organization Identifier on M-Pesa
        /// </summary>
        [JsonProperty("RecieverIdentifierType")]
        public string RecieverIdentifierType { get; private set; }

        /// <summary>
        /// Comments that are sent along with the transaction. (Upto 100 characters)
        /// </summary>
        [JsonProperty("Remarks")]
        public string Remarks { get; private set; }

        /// <summary>
        /// The path that stores information of time out transaction.
        /// </summary>
        [JsonProperty("QueueTimeOutURL")]
        public string QueueTimeOutURL { get; private set; }

        /// <summary>
        /// The path that stores information of transaction 
        /// </summary>
        [JsonProperty("ResultURL")]
        public string ResultURL { get; private set; }

        /// <summary>
        /// Optional Parameter (upto 100 characters)
        /// </summary>
        [JsonProperty("Occasion")]
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
