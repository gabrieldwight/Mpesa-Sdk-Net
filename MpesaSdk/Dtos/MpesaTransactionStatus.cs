using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    /// <summary>
    /// Mpesa Transaction Status Query data transfer object
    /// </summary>
    public class MpesaTransactionStatus
    {
        /// <summary>
        /// The name of Initiator to initiating  the request.
        /// This is the credential/username used to authenticate the transaction request.
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
        /// Takes only 'TransactionStatusQuery' command id
        /// The default value has been set to that so you don't have to set this property.
        /// </summary>
        [JsonPropertyName("CommandID")]
		[JsonInclude]
		public string CommandID { get; private set; } = Transaction_Type.TransactionStatusQuery;

        /// <summary>
        /// Unique identifier to identify a transaction on M-Pesa. e.g LKXXXX1234
        /// </summary>
        [JsonPropertyName("TransactionID")]
		[JsonInclude]
		public string TransactionID { get; private set; }

        /// <summary>
        /// This is a global unique identifier for the transaction request returned by the API proxy upon successful request submission. If you don’t have the M-PESA transaction ID you can use this to query.
        /// </summary>
        [JsonPropertyName("OriginatorConversationID")]
		[JsonInclude]
		public string OriginatorConversationID { get; private set; }

        /// <summary>
        /// Organization/MSISDN receiving the transaction
        /// </summary>
        [JsonPropertyName("PartyA")]
		[JsonInclude]
		public string PartyA { get; private set; }

        /// <summary>
        /// Type of organization receiving the transaction
        /// 1 – MSISDN
        /// 2 – Till Number
        /// 4 – Organization short code
        /// </summary>
        [JsonPropertyName("IdentifierType")]
		[JsonInclude]
		public string IdentifierType { get; private set; }

        /// <summary>
        /// Comments that are sent along with the transaction
        /// </summary>
        [JsonPropertyName("Remarks")]
		[JsonInclude]
		public string Remarks { get; private set; }

        /// <summary>
        /// The path that stores information of time out transaction. https://ip or domain:port/path
        /// </summary>
        [JsonPropertyName("QueueTimeOutURL")]
		[JsonInclude]
		public string QueueTimeOutURL { get; private set; }

        /// <summary>
        /// The path that stores information of transaction. https://ip or domain:port/path
        /// </summary>
        [JsonPropertyName("ResultURL")]
		[JsonInclude]
		public string ResultURL { get; private set; }

        /// <summary>
        /// Optional Parameter. (upto 100 characters)
        /// </summary>
        [JsonPropertyName("Occasion")]
		[JsonInclude]
		public string Occasion { get; private set; }

        /// <summary>
        /// Mpesa Transaction Status Query data transfer object
        /// </summary>
        /// <param name="initiator">
        /// The name of Initiator to initiating  the request.
        /// This is the credential/username used to authenticate the transaction request.
        /// </param>
        /// <param name="securityCredential">
        /// Encrypted password for the initiator to authenticate the transaction request.
        /// Use <c>Credentials.EncryptPassword</c> method available under MpesaLib.Helpers to encrypt the password.
        /// </param>
        /// <param name="transactionId">Unique identifier to identify a transaction on M-Pesa. e.g LKXXXX1234</param>
        /// <param name="partyA">Organization/MSISDN receiving the transaction</param>
        /// <param name="identifierType">
        /// Type of organization receiving the transaction
        /// 1 – MSISDN
        /// 2 – Till Number
        /// 4 – Organization short code
        /// </param>
        /// <param name="remarks">
        /// Comments that are sent along with the transaction
        /// </param>
        /// <param name="queueTimeoutUrl">The path that stores information of time out transaction. https://ip or domain:port/path</param>
        /// <param name="resultUrl">The path that stores information of transaction. https://ip or domain:port/path</param>
        /// <param name="occasion">Optional Parameter. (upto 100 characters)</param>
        public MpesaTransactionStatus(string initiator, string securityCredential, string transactionId, string partyA, string identifierType, string remarks, string queueTimeoutUrl, string resultUrl, string occasion)
        {
            Initiator = initiator;
            SecurityCredential = securityCredential;
            CommandID = Transaction_Type.TransactionStatusQuery;
            TransactionID = transactionId;
            PartyA = partyA;
            IdentifierType = identifierType;
            Remarks = remarks;
            QueueTimeOutURL = queueTimeoutUrl;
            ResultURL = resultUrl;
            Occasion = occasion;
        }

        /// <summary>
        /// Mpesa Transaction Status Query data transfer object
        /// </summary>
        /// <param name="initiator">
        /// The name of Initiator to initiating  the request.
        /// This is the credential/username used to authenticate the transaction request.
        /// </param>
        /// <param name="securityCredential">
        /// Encrypted password for the initiator to authenticate the transaction request.
        /// Use <c>Credentials.EncryptPassword</c> method available under MpesaLib.Helpers to encrypt the password.
        /// </param>
        /// <param name="transactionId">Unique identifier to identify a transaction on M-Pesa. e.g LKXXXX1234</param>
        /// <param name="originatorConversationID">Unique identifier to identify without transaction ID. e.g AG_XXXXX_XXX</param>
        /// <param name="partyA">Organization/MSISDN receiving the transaction</param>
        /// <param name="identifierType">
        /// Type of organization receiving the transaction
        /// 1 – MSISDN
        /// 2 – Till Number
        /// 4 – Organization short code
        /// </param>
        /// <param name="remarks">
        /// Comments that are sent along with the transaction
        /// </param>
        /// <param name="queueTimeoutUrl">The path that stores information of time out transaction. https://ip or domain:port/path</param>
        /// <param name="resultUrl">The path that stores information of transaction. https://ip or domain:port/path</param>
        /// <param name="occasion">Optional Parameter. (upto 100 characters)</param>
        public MpesaTransactionStatus(string initiator, string securityCredential, string transactionId, string originatorConversationID, string partyA, string identifierType, string remarks, string queueTimeoutUrl, string resultUrl, string occasion)
        {
            Initiator = initiator;
            SecurityCredential = securityCredential;
            CommandID = Transaction_Type.TransactionStatusQuery;
            TransactionID = transactionId;
            OriginatorConversationID = originatorConversationID;
            PartyA = partyA;
            IdentifierType = identifierType;
            Remarks = remarks;
            QueueTimeOutURL = queueTimeoutUrl;
            ResultURL = resultUrl;
            Occasion = occasion;
        }
    }
}
