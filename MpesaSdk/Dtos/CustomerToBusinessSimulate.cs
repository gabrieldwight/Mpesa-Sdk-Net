using System.Text.Json.Serialization;

namespace MpesaSdk.Dtos
{
    /// <summary>
    /// C2B Simulate data transfer object
    /// </summary>
    public class CustomerToBusinessSimulate
    {
        /// <summary>
        /// This is the Short Code receiving the amount being transacted.
        /// </summary>
        [JsonPropertyName("ShortCode")]
		[JsonInclude]
		public string ShortCode { get; private set; }

        /// <summary>
        /// This is a unique identifier of the transaction type: There are two types of these Identifiers:
        /// CustomerPayBillOnline: This is used for Pay Bills shortcodes.
        /// CustomerBuyGoodsOnline: This is used for Buy Goods shortcodes.
        /// Buy Default this property is set to CustomerPayBillOnline
        /// </summary>
        [JsonPropertyName("CommandID")]
		[JsonInclude]
		public string CommandID { get; private set; }

        /// <summary>
        /// This is the amount being transacted. The parameter expected is a numeric value.
        /// </summary>
        [JsonPropertyName("Amount")]
		[JsonInclude]
		public string Amount { get; private set; }

        /// <summary>
        /// This is the phone number initiating the C2B transaction.(format: 2547XXXXXXXX)
        /// </summary>
        [JsonPropertyName("Msisdn")]
		[JsonInclude]
		public string Msisdn { get; private set; }

        /// <summary>
        /// This is used on CustomerPayBillOnline option only. 
        /// This is where a customer is expected to enter a unique bill identifier, e.g an Account Number. 
        /// </summary>
        [JsonPropertyName("BillRefNumber")]
		[JsonInclude]
		public string BillRefNumber { get; private set; }


        /// <summary>
        /// C2B Simulate data transfer object
        /// </summary>
        /// <param name="shortCode">This is the Short Code receiving the amount being transacted.</param>
        /// <param name="commandId">
        /// This is a unique identifier of the transaction type: There are two types of these Identifiers:
        /// CustomerPayBillOnline: This is used for Pay Bills shortcodes.
        /// CustomerBuyGoodsOnline: This is used for Buy Goods shortcodes.
        /// Buy Default this property is set to CustomerPayBillOnline
        /// </param>
        /// <param name="amount">
        /// This is the amount being transacted. The parameter expected is a numeric value.
        /// </param>
        /// <param name="msisdn">This is the phone number initiating the C2B transaction.(format: 2547XXXXXXXX)</param>
        /// <param name="billReferenceNumber">
        /// This is used on CustomerPayBillOnline option only. 
        /// This is where a customer is expected to enter a unique bill identifier, e.g an Account Number. 
        /// </param>
        public CustomerToBusinessSimulate(string shortCode, string commandId, string amount, string msisdn, string billReferenceNumber)
        {
            ShortCode = shortCode;
            CommandID = commandId;
            Amount = amount;
            Msisdn = msisdn;
            BillRefNumber = billReferenceNumber;
        }
    }
}
