namespace MpesaSdk
{
    /// <summary>
    /// This is a unique command that specifies transaction type.
    /// </summary>
    public static class Transaction_Type
    {
        /// <summary>
        /// Reversal for an erroneous C2B transaction
        /// </summary>
        public const string TransactionReversal = "TransactionReversal";

        /// <summary>
        /// This supports sending money to both registered and unregistered M-Pesa customers. Used to send money from an employer to employees e.g. salaries.
        /// </summary>
        public const string SalaryPayment = "SalaryPayment";

        /// <summary>
        /// This is a normal business to customer payment, send money from an employer to employees and supports only M-Pesa registered customers e.g. refunds.
        /// </summary>
        public const string BusinessPayment = "BusinessPayment";

        /// <summary>
        /// This is a promotional payment to customers. The M-Pesa notification message is a congratulatory message. Supports only M-Pesa registered customers. Used to send money when promotions take place e.g. raffle winners.
        /// </summary>
        public const string PromotionPayment = "PromotionPayment";

        /// <summary>
        /// Used to check the balance in a paybill/buy goods account (includes utility, MMF, Merchant, Charges paid account).
        /// </summary>
        public const string AccountBalance = "AccountBalance";

        /// <summary>
        /// Used to simulate a transaction taking place in the case of C2B Simulate Transaction or to initiate a transaction on behalf of the customer (STK Push).
        /// </summary>
        public const string CustomerPayBillOnline = "CustomerPayBillOnline";

        /// <summary>
        /// Used to simulate a transaction taking place in the case of C2B Simulate Transaction or to initiate a transaction on behalf of the customer (STK Push).
        /// </summary>
        public const string CustomerBuyGoodsOnline = "CustomerBuyGoodsOnline";

        /// <summary>
        /// Used to query the details of a transaction.
        /// </summary>
        public const string TransactionStatusQuery = "TransactionStatusQuery";

        /// <summary>
        /// Similar to STK push, uses M-Pesa PIN as a service.
        /// </summary>
        public const string CheckIdentity = "CheckIdentity";

        /// <summary>
        /// Sending funds from one paybill to another paybill
        /// </summary>
        public const string BusinessPayBill = "BusinessPayBill";

        /// <summary>
        /// Sending funds from buy goods to another buy goods.
        /// </summary>
        public const string BusinessBuyGoods = "BusinessBuyGoods";

        /// <summary>
        /// Transfer of funds from utility to MMF account.
        /// </summary>
        public const string DisburseFundsToBusiness = "DisburseFundsToBusiness";

        /// <summary>
        /// Transferring funds from one paybills MMF to another paybills MMF account.
        /// </summary>
        public const string BusinessToBusinessTransfer = "BusinessToBusinessTransfer";

        /// <summary>
        /// Transferring funds from paybills MMF to another paybills utility account.
        /// </summary>
        public const string BusinessTransferFromMMFToUtility = "BusinessTransferFromMMFToUtility";

        /// <summary>
        /// MerchantToMerchantTransfer Command ID
        /// </summary>
        public const string MerchantToMerchantTransfer = "MerchantToMerchantTransfer";

        /// <summary>
        /// MerchantTransferFromMerchantToWorking Command ID
        /// </summary>
        public const string MerchantTransferFromMerchantToWorking = "MerchantTransferFromMerchantToWorking";

        /// <summary>
        /// MerchantServicesMMFAccountTransfer Command ID
        /// </summary>
        public const string MerchantServicesMMFAccountTransfer = "MerchantServicesMMFAccountTransfer";

        /// <summary>
        /// AgencyFloatAdvance Command ID
        /// </summary>
        public const string AgencyFloatAdvance = "AgencyFloatAdvance";
    }
}
