namespace MpesaSdk.Response
{
    /// <summary>
    /// This is a response obtained when making a stk push transaction.
    /// </summary>
    public class LipaNaMpesaOnlinePushStkResponse : MpesaResponse
    {
        /// <summary>
        /// This is a global unique identifier of the processed checkout transaction request.
        /// </summary>
        /// <value>The checkout request identifier.</value>
        public string CheckoutRequestID { get; set; }

        /// <summary>
        /// This is a message that your system can display to the Customer as an acknowledgement of the payment request submission.
        /// </summary>
        /// <value>The customer message.</value>
        public string CustomerMessage { get; set; }

        /// <summary>
        /// This is a global unique Identifier for any submited payment request.
        /// </summary>
        /// <value>The merchant request identifier.</value>
        public string MerchantRequestID { get; set; }

        public string PhoneNumber { get; set; }
    }
}
