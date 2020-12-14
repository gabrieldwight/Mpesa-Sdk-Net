using MpesaSdk.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace MpesaSdk.Interfaces
{
    /// <summary>
    /// IMpesaClient Interface
    /// </summary>
    /// <remarks>
    /// Provides all the Methods implemented by MpesaClient Class. 
    /// A developer can create their own implementation for example for tests/mocks by inheriting from this interface.
    /// </remarks>
    public interface IMpesaClient
    {
        /// <summary>
        /// GetAuthTokenAsync is an asynchronous method that requests for and returns an accesstoken from MPESA API Server.
        /// </summary>
        /// <param name="consumerKey">ConsumerKey provided by Safaricom in Daraja Portal.</param>
        /// <param name="consumerSecret">ConsumerSecret provided by Safaricom in Daraja Portal.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.AuthToken</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A string of characters representing the accesstoken.</returns>
        Task<string> GetAuthTokenAsync(string consumerKey, string consumerSecret, string requestEndPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// GetAuthTokenAsync is an asynchronous method that requests for and returns an accesstoken from MPESA API Server.
        /// </summary>
        /// <param name="consumerKey">ConsumerKey provided by Safaricom in Daraja Portal.</param>
        /// <param name="consumerSecret">ConsumerSecret provided by Safaricom in Daraja Portal.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.AuthToken</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A string of characters representing the accesstoken.</returns>
        string GetAuthToken(string consumerKey, string consumerSecret, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Makes STK Push payment request to MPESA API Server.
        /// </summary>
        /// <param name="lipaNaMpesaOnline">
        /// Data transfer object containing properties for the Lipa Na Mpesa Online API endpoint.
        /// </param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.LipaNaMpesaOnline</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing LNMO response data from MPESA API Server</returns>
        Task<string> MakeLipaNaMpesaOnlinePaymentAsync(LipaNaMpesaOnline lipaNaMpesaOnline, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Makes an STK Push payment request to MPESA API Server.
        /// </summary>
        /// <param name="lipaNaMpesaOnline">
        /// Data transfer object containing properties for the Lipa Na Mpesa Online API endpoint.
        /// </param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.LipaNaMpesaOnline</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API response</returns>
        string MakeLipaNaMpesaOnlinePayment(LipaNaMpesaOnline lipaNaMpesaOnline, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries Mpesa Online Transaction Status
        /// </summary>
        /// <param name="lipaNaMpesaQuery">Transaction Query Data transfer object</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.QueryLipaNaMpesaOnlieTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>
        /// A JSON string containing data from MPESA API reposnse
        /// </returns>
        /// <remarks>
        /// Use only for transactions initiated with <c>MakeLipaNaMpesaOnlinePayment</c> method.
        /// For Other transaction based methods (C2B,B2C,B2B) use <c>QueryMpesaTransactionStatusAsync</c> method.
        /// </remarks>
        Task<string> QueryLipaNaMpesaTransactionAsync(LipaNaMpesaQuery lipaNaMpesaQuery, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries Mpesa Online Transaction Status.
        /// </summary>
        /// <param name="lipaNaMpesaQuery">Transaction Query Data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.QueryLipaNaMpesaOnlieTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>
        /// A JSON string containing data from MPESA API reposnse.
        /// </returns>
        /// <remarks>
        /// Use only for transactions initiated with <c>MakeLipaNaMpesaOnlinePayment</c> method.
        /// For Other transaction based methods (C2B, B2C, B2B, Accountbalance, Reversal) 
        /// use <c>QueryMpesaTransactionStatusAsync</c> method.
        /// </remarks>
        string QueryLipaNaMpesaTransaction(LipaNaMpesaQuery lipaNaMpesaQuery, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries MPESA Paybill Account balance.
        /// </summary>
        /// <param name="accountBalance">Account balance query data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.QueryAccountBalance</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        Task<string> QueryAccountBalanceAsync(AccountBalance accountBalance, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries MPESA Paybill Account balance.
        /// </summary>
        /// <param name="accountBalance">Account balance query data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.QueryAccountBalance</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        string QueryAccountBalance(AccountBalance accountBalance, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Makes a Business to Business payment request between Paybill numbers.
        /// </summary>
        /// <param name="businessToBusiness">B2B data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.BusinessToBusiness</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        Task<string> MakeB2BPaymentAsync(BusinessToBusiness businessToBusiness, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Makes a Business to Business payment request between Paybill numbers.
        /// </summary>
        /// <param name="businessToBusiness">B2B data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.BusinessToBusiness</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        string MakeB2BPayment(BusinessToBusiness businessToBusiness, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);



        /// <summary>
        /// Makes a Business to Customer payment request. Paybill to Phone Number.
        /// </summary>
        /// <param name="businessToCustomer">B2C data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.BusinessToCustomer</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Suitable for refunds, rewards or just about any transaction that involves a business paying a customer.
        /// </remarks>
        Task<string> MakeB2CPaymentAsync(BusinessToCustomer businessToCustomer, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Makes a Business to Customer payment request. Paybill to Phone Number.
        /// </summary>
        /// <param name="businessToCustomer">B2C data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.BusinessToCustomer</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Suitable for refunds, rewards or just about any transaction that involves a business paying a customer.
        /// </remarks>
        string MakeB2CPayment(BusinessToCustomer businessToCustomer, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Simulates a Customer to Business payment request.
        /// </summary>
        /// <param name="customerToBusinessSimulate">C2B data transfer object</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.CustomerToBusinessSimulate</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Use only for Simulation/testing. In production use <c>RegisterC2BUrlAsync</c> method to register 
        /// endpoints in your application that receive customer initiated transactions from the MPESA API
        /// for confirmation and/or validation
        /// </remarks>
        Task<string> MakeC2BPaymentAsync(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);

        /// <summary>
        /// Simulates a Customer to Business payment request.
        /// </summary>
        /// <param name="customerToBusinessSimulate">C2B data transfer object</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.CustomerToBusinessSimulate</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Use only for Simulation/testing. In production use <c>RegisterC2BUrlAsync</c> method to register 
        /// endpoints in your application that receive customer initiated transactions from the MPESA API
        /// for confirmation and/or validation
        /// </remarks>
        string MakeC2BPayment(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Registers Customer to Business Confirmation and Validation URLs.
        /// </summary>
        /// <param name="customerToBusinessRegisterUrl">C2B Register URLs data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.RegisterC2BUrl</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        Task<string> RegisterC2BUrlAsync(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Registers Customer to Business Confirmation and Validation URLs.
        /// </summary>
        /// <param name="customerToBusinessRegisterUrl">C2B Register URLs data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.RegisterC2BUrl</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        string RegisterC2BUrl(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Reverses a Mpesa Transaction.
        /// </summary>
        /// <param name="mpesaReversal">Reversal data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.ReverseMpesaTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        Task<string> ReverseMpesaTransactionAsync(MpesaReversal mpesaReversal, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Reverses a Mpesa Transaction.
        /// </summary>
        /// <param name="mpesaReversal">Reversal data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.ReverseMpesaTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        string ReverseMpesaTransaction(MpesaReversal mpesaReversal, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries the status of an Mpesa Transaction.
        /// </summary>
        /// <param name="mpesaTransactionStatus">Transaction Status data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.QueryMpesaTransactionStatus</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        Task<string> QueryMpesaTransactionStatusAsync(MpesaTransactionStatus mpesaTransactionStatus, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);


        /// <summary>
        /// Queries the status of an Mpesa Transaction.
        /// </summary>
        /// <param name="mpesaTransactionStatus">Transaction Status data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="requestEndPoint">Set to <c>RequestEndPoint.QueryMpesaTransactionStatus</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        string QueryMpesaTransactionStatus(MpesaTransactionStatus mpesaTransactionStatus, string accesstoken, string requestEndPoint, CancellationToken cancellationToken = default);
    }
}
