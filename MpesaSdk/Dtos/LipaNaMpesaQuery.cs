using System;
using System.Text;
using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    /// <summary>
    /// LipaNaMpesa Query transaction status data transfer object
    /// </summary>
    public class LipaNaMpesaQuery
    {
        /// <summary>
        /// This is organizations shortcode (Paybill or Buygoods - A 5 to 6 digit account number) 
        /// used to identify an organization and receive the transaction.
        /// </summary>
        [JsonPropertyName("BusinessShortCode")]
		[JsonInclude]
		public string BusinessShortCode { get; private set; }

        /// <summary>
        /// Lipa Na Mpesa Online PassKey
        /// </summary>
        public string Passkey { get; private set; }

        /// <summary>
        /// This is the password used for encrypting the request sent: A base64 encoded string. 
        /// The base64 string is a combination of Shortcode+Passkey+Timestamp
        /// </summary>
        [JsonPropertyName("Password")]
		[JsonInclude]
		public string Password { get; private set; }


        /// <summary>
        /// This is the Timestamp of the transaction, 
        /// normaly in the formart of YEAR+MONTH+DATE+HOUR+MINUTE+SECOND (YYYYMMDDHHMMSS) 
        /// Each part should be atleast two digits apart from the year which takes four digits.
        /// </summary>
        [JsonPropertyName("Timestamp")]
		[JsonInclude]
		public string Timestamp { get; private set; } = DateTime.Now.ToString("yyyyMMddHHmmss");

        /// <summary>
        /// This is a global unique identifier of the processed checkout transaction request.
        /// e.g ws_CO_DMZ_123212312_2342347678234
        /// </summary>
        [JsonPropertyName("CheckoutRequestID")]
		[JsonInclude]
		public string CheckoutRequestID { get; private set; }



        /// <summary>
        /// LipaNaMpesa Query transaction status data transfer object
        /// </summary>
        /// <param name="businessShortCode">
        /// This is organizations shortcode (Paybill or Buygoods - A 5 to 6 digit account number) 
        /// used to identify an organization and receive the transaction.
        /// </param>
        /// <param name="passkey">Lipa Na Mpesa Online PassKey</param>
        /// <param name="timeStamp">
        /// This is the Timestamp of the transaction, 
        /// normaly in the formart of YEAR+MONTH+DATE+HOUR+MINUTE+SECOND (YYYYMMDDHHMMSS) 
        /// Each part should be atleast two digits apart from the year which takes four digits.
        /// </param>
        /// <param name="checkoutRequestId">
        /// This is a global unique identifier of the processed checkout transaction request.
        /// e.g ws_CO_DMZ_123212312_2342347678234
        /// </param>
        public LipaNaMpesaQuery(string businessShortCode, string passkey, DateTime timeStamp, string checkoutRequestId)
        {
            var formattedTimestamp = timeStamp.ToString("yyyyMMddHHmmss");

            BusinessShortCode = businessShortCode;
            Passkey = passkey;
            Timestamp = formattedTimestamp;
            Password = EncodePassword(businessShortCode, passkey, formattedTimestamp);
            CheckoutRequestID = checkoutRequestId;
        }

        /// <summary>
        /// This method creates the necessary base64 encoded string that encrypts the request sent 
        /// </summary>
        private string EncodePassword(string shortCode, string passkey, string timestamp)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{shortCode}{passkey}{timestamp}"));
        }
    }
}
