using Newtonsoft.Json;
using System;
using System.Text;

namespace MpesaSdk.Dtos
{
    /// <summary>
    /// STK Push data transfer object
    /// </summary>
    public class LipaNaMpesaOnline
    {
        #region Properties
        /// <summary>
        /// This is organizations shortcode (Paybill or Buygoods - A 5 to 6 digit account number) 
        /// used to identify an organization and receive the transaction.
        /// </summary>
        [JsonProperty("BusinessShortCode")]
        public string BusinessShortCode { get; private set; }

        /// <summary>
        /// This is the Timestamp of the transaction, 
        /// normaly in the formart of YEAR+MONTH+DATE+HOUR+MINUTE+SECOND (YYYYMMDDHHMMSS) 
        /// Each part should be atleast two digits apart from the year which takes four digits.        
        /// </summary>
        [JsonProperty("Timestamp")]
        public string Timestamp { get; private set; } = DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary>
        /// This is the transaction type that is used to identify the transaction when sending the request to M-Pesa. 
        /// The transaction type for M-Pesa Express is "CustomerPayBillOnline" 
        /// </summary>
#if DEBUG
        [JsonProperty("TransactionType")]
        public string TransactionType { get; private set; } = Transaction_Type.CustomerPayBillOnline;
#else
        [JsonProperty("TransactionType")]
        public string TransactionType { get; private set; }
#endif

        /// <summary>
        /// This is the Amount transacted, normally a numeric value. Money that customer pays to the Shorcode. 
        /// Only whole numbers are supported.
        /// </summary>
        [JsonProperty("Amount")]
        public string Amount { get; private set; }

        /// <summary>
        /// The phone number sending money. The parameter expected is a Valid Safaricom Mobile Number 
        /// that is M-Pesa registered in the format 2547XXXXXXXX
        /// </summary>
        [JsonProperty("PartyA")]
        public string PartyA { get; private set; }

        /// <summary>
        /// The organization receiving the funds. The parameter expected is a 5 to 6 digit.
        /// This can be the same as BusinessShortCode value.
        /// </summary>
        [JsonProperty("PartyB")]
        public string PartyB { get; private set; }

        /// <summary>
        /// The Mobile Number to receive the STK Pin Prompt. 
        /// This number can be the same as PartyA value.
        /// </summary>
        [JsonProperty("PhoneNumber")]
        public string PhoneNumber { get; private set; }

        /// <summary>
        /// A CallBack URL is a valid secure URL that is used to receive notifications from M-Pesa API. 
        /// It is the endpoint to which the results will be sent by M-Pesa API.
        /// </summary>
        [JsonProperty("CallBackURL")]
        public string CallBackURL { get; private set; }

        /// <summary>
        /// Account Reference: This is an Alpha-Numeric parameter that is defined by your system as an Identifier 
        /// of the transaction for CustomerPayBillOnline transaction type. Along with the business name, 
        /// this value is also displayed to the customer in the STK PIN Prompt message. 
        /// Maximum of 12 characters.
        /// </summary>
        [JsonProperty("AccountReference")]
        public string AccountReference { get; private set; }

        /// <summary>
        /// This is any additional information/comment that can be sent along with the request from your system. 
        /// Maximum of 13 Characters.
        /// </summary>
        [JsonProperty("TransactionDesc")]
        public string TransactionDesc { get; private set; }

        /// <summary>
        /// Lipa Na Mpesa Online PassKey
        /// Provide the Passkey only if you want MpesaLib to Encode the Password for you.
        /// </summary>
        public string Passkey { get; private set; }

        /// <summary>
        /// This is the password used for encrypting the request sent: A base64 encoded string. 
        /// The base64 string is a combination of Shortcode+Passkey+Timestamp
        /// The Defualt value is set by a private method that creates the necessary base64 encoded string
        /// Don't set this property if you have set the passKey property.
        /// </summary>
        [JsonProperty("Password")]
        public string Password { get; private set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Mpesa Lipa Na Mpesa STK Push data transfer object
        /// </summary>
        /// <param name="businessShortCode">
        /// This is organizations shortcode (Paybill or Buygoods - A 5 to 6 digit account number) 
        /// used to identify an organization and receive the transaction.
        /// </param>
        /// <param name="timeStamp">
        /// This is the Timestamp of the transaction, 
        /// normaly in the formart of YEAR+MONTH+DATE+HOUR+MINUTE+SECOND (YYYYMMDDHHMMSS) 
        /// Each part should be atleast two digits apart from the year which takes four digits.    
        /// </param>
        /// <param name="transactionType">
        /// This is the transaction type that is used to identify the transaction when sending the request to M-Pesa. 
        /// The transaction type for M-Pesa Express is "CustomerPayBillOnline" 
        /// </param>
        /// <param name="amount">
        ///  This is the Amount transacted, normally a numeric value. Money that customer pays to the Shorcode. 
        /// Only whole numbers are supported.
        /// </param>
        /// <param name="partyA">
        ///  The phone number sending money. The parameter expected is a Valid Safaricom Mobile Number 
        /// that is M-Pesa registered in the format 2547XXXXXXXX
        /// </param>
        /// <param name="partyB">
        /// The organization receiving the funds. The parameter expected is a 5 to 6 digit.
        /// This can be the same as BusinessShortCode value.
        /// </param>
        /// <param name="phoneNumber"> 
        /// The Mobile Number to receive the STK Pin Prompt. 
        /// This number can be the same as PartyA value.
        /// </param>
        /// <param name="callBackUrl">
        /// A CallBack URL is a valid secure URL that is used to receive notifications from M-Pesa API. 
        /// It is the endpoint to which the results will be sent by M-Pesa API.
        /// </param>
        /// <param name="accountReference">
        /// Account Reference: This is an Alpha-Numeric parameter that is defined by your system as an Identifier 
        /// of the transaction for CustomerPayBillOnline transaction type. Along with the business name, 
        /// this value is also displayed to the customer in the STK PIN Prompt message. 
        /// Maximum of 12 characters.
        /// </param>
        /// <param name="transactionDescription">
        /// This is any additional information/comment that can be sent along with the request from your system. 
        /// Maximum of 13 Characters.
        /// </param>
        /// <param name="passkey">
        /// Lipa Na Mpesa Online PassKey
        /// Provide the Passkey only if you want MpesaLib to Encode the Password for you.
        /// </param>
        public LipaNaMpesaOnline(string businessShortCode, DateTime timeStamp, string transactionType, string amount,
            string partyA, string partyB, string phoneNumber, string callBackUrl, string accountReference,
            string transactionDescription, string passkey)
        {
            var formattedTimestamp = timeStamp.ToString("yyyyMMddHHmmss");

            BusinessShortCode = businessShortCode;
            Timestamp = formattedTimestamp;
            TransactionType = transactionType;
            Amount = amount;
            PartyA = partyA;
            PartyB = partyB;
            PhoneNumber = phoneNumber;
            CallBackURL = callBackUrl;
            AccountReference = accountReference;
            TransactionDesc = transactionDescription;
            Passkey = passkey;
            Password = EncodePassword(BusinessShortCode, passkey, formattedTimestamp);
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// This method creates the necessary base64 encoded string that encrypts the request sent 
        /// </summary>
        private string EncodePassword(string shortCode, string passkey, string timestamp)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{shortCode}{passkey}{timestamp}"));
        }
        #endregion
    }
}
