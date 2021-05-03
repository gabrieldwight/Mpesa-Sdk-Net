using System;

namespace MpesaSdk
{
    /// <summary>
    /// Strongly typed properties from the MPESA API request endpoints
    /// </summary>
    public static class MpesaRequestEndpoint
    {
        /// <summary>
        /// Sandbox MPESA API BaseAdress, use in a development environment
        /// </summary>
        public static Uri SandboxBaseAddress { get; set; } = new Uri("https://sandbox.safaricom.co.ke/");

        /// <summary>
        /// Live MPESA API BaseAdress, use in staging, production  environments
        /// </summary>
        public static Uri LiveBaseAddress { get; set; } = new Uri("https://api.safaricom.co.ke/");

        /// <summary>
        /// Lipa na M-Pesa Online Payment API is used to initiate a M-Pesa transaction on behalf of a customer using STK Push. This is the same technique mySafaricom App uses whenever the app is used to make payments.
        /// </summary>
        public static string LipaNaMpesaOnline { get; set; } = "mpesa/stkpush/v1/processrequest";

        /// <summary>
        /// QueryLipaNaMpesaOnlieTransaction API Request Endpoint
        /// </summary>
        public static string QueryLipaNaMpesaOnlineTransaction { get; set; } = "mpesa/stkpushquery/v1/query";

        /// <summary>
        /// CustomerToBusinessSimulate API Request Endpoint
        /// </summary>
        public static string CustomerToBusinessSimulate { get; set; } = "mpesa/c2b/v1/simulate";

        /// <summary>
        /// This API enables Business to Business (B2B) transactions between a business and another business. Use of this API requires a valid and verified B2B M-Pesa short code for the business initiating the transaction and the both businesses involved in the transaction.
        /// </summary>
        public static string BusinessToBusiness { get; set; } = "mpesa/b2b/v1/paymentrequest";

        /// <summary>
        /// This API enables Business to Customer (B2C) transactions between a company and customers who are the end-users of its products or services. Use of this API requires a valid and verified B2C M-Pesa Short code.
        /// </summary>
        public static string BusinessToCustomer { get; set; } = "mpesa/b2c/v1/paymentrequest";

        /// <summary>
        /// AuthToken Request API Endpoint
        /// </summary>
        public static string AuthToken { get; set; } = "oauth/v1/generate?grant_type=client_credentials";

        /// <summary>
        /// The Account Balance API requests for the account balance of a shortcode.
        /// </summary>
        public static string QueryAccountBalance { get; set; } = "mpesa/accountbalance/v1/query";

        /// <summary>
        /// RegisterC2BUrl API Request Endpoint
        /// </summary>
        public static string RegisterC2BUrl { get; set; } = "mpesa/c2b/v1/registerurl";

        /// <summary>
        /// Reverses a B2B, B2C or C2B M-Pesa transaction.
        /// </summary>
        public static string ReverseMpesaTransaction { get; set; } = "mpesa/reversal/v1/request";

        /// <summary>
        /// Transaction Status API checks the status of a B2B, B2C and C2B APIs transactions.
        /// </summary>
        public static string QueryMpesaTransactionStatus { get; set; } = "mpesa/transactionstatus/v1/query";

        /// <summary>
        /// Register Url to recieve pull transaction callback
        /// </summary>
        public static string PullMpesaTransactionRegisterUrl { get; set; } = "pulltransactions/v1/register";

        /// <summary>
        /// Pull Transaction API checks for the missed transactions that are not send in the C2B callback.
        /// </summary>
        public static string PullMpesaTransaction { get; set; } = "pulltransactions/v1/query";
    }
}
