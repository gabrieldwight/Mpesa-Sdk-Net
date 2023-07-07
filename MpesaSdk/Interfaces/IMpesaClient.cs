using MpesaSdk.Dtos;
using MpesaSdk.Response;
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
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A string of characters representing the accesstoken.</returns>
		Task<string> GetAuthTokenAsync(string consumerKey, string consumerSecret, CancellationToken cancellationToken = default);

		/// <summary>
		/// GetAuthTokenAsync is an asynchronous method that requests for and returns an accesstoken from MPESA API Server.
		/// </summary>
		/// <param name="consumerKey">ConsumerKey provided by Safaricom in Daraja Portal.</param>
		/// <param name="consumerSecret">ConsumerSecret provided by Safaricom in Daraja Portal.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A string of characters representing the accesstoken.</returns>
		string GetAuthToken(string consumerKey, string consumerSecret, CancellationToken cancellationToken = default);


		/// <summary>
		/// Makes STK Push payment request to MPESA API Server.
		/// </summary>
		/// <param name="lipaNaMpesaOnline">
		/// Data transfer object containing properties for the Lipa Na Mpesa Online API endpoint.
		/// </param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing LNMO response data from MPESA API Server</returns>
		Task<LipaNaMpesaOnlinePushStkResponse> MakeLipaNaMpesaOnlinePaymentAsync(LipaNaMpesaOnline lipaNaMpesaOnline, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Makes an STK Push payment request to MPESA API Server.
		/// </summary>
		/// <param name="lipaNaMpesaOnline">
		/// Data transfer object containing properties for the Lipa Na Mpesa Online API endpoint.
		/// </param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API response</returns>
		LipaNaMpesaOnlinePushStkResponse MakeLipaNaMpesaOnlinePayment(LipaNaMpesaOnline lipaNaMpesaOnline, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Queries Mpesa Online Transaction Status
		/// </summary>
		/// <param name="lipaNaMpesaQuery">Transaction Query Data transfer object</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>
		/// A JSON string containing data from MPESA API reposnse
		/// </returns>
		/// <remarks>
		/// Use only for transactions initiated with <c>MakeLipaNaMpesaOnlinePayment</c> method.
		/// For Other transaction based methods (C2B,B2C,B2B) use <c>QueryMpesaTransactionStatusAsync</c> method.
		/// </remarks>
		Task<LipaNaMpesaQueryStkResponse> QueryLipaNaMpesaTransactionAsync(LipaNaMpesaQuery lipaNaMpesaQuery, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Queries Mpesa Online Transaction Status.
		/// </summary>
		/// <param name="lipaNaMpesaQuery">Transaction Query Data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>
		/// A JSON string containing data from MPESA API reposnse.
		/// </returns>
		/// <remarks>
		/// Use only for transactions initiated with <c>MakeLipaNaMpesaOnlinePayment</c> method.
		/// For Other transaction based methods (C2B, B2C, B2B, Accountbalance, Reversal) 
		/// use <c>QueryMpesaTransactionStatusAsync</c> method.
		/// </remarks>
		LipaNaMpesaQueryStkResponse QueryLipaNaMpesaTransaction(LipaNaMpesaQuery lipaNaMpesaQuery, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Queries MPESA Paybill Account balance.
		/// </summary>
		/// <param name="accountBalance">Account balance query data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<MpesaResponse> QueryAccountBalanceAsync(AccountBalance accountBalance, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Queries MPESA Paybill Account balance.
		/// </summary>
		/// <param name="accountBalance">Account balance query data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		MpesaResponse QueryAccountBalance(AccountBalance accountBalance, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Makes a Business to Business payment request between Paybill numbers.
		/// </summary>
		/// <param name="businessToBusiness">B2B data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<MpesaResponse> MakeB2BPaymentAsync(BusinessToBusiness businessToBusiness, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Makes a Business to Business payment request between Paybill numbers.
		/// </summary>
		/// <param name="businessToBusiness">B2B data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		MpesaResponse MakeB2BPayment(BusinessToBusiness businessToBusiness, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// Makes a Business to Customer payment request. Paybill to Phone Number.
		/// </summary>
		/// <param name="businessToCustomer">B2C data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		/// <remarks>
		/// Suitable for refunds, rewards or just about any transaction that involves a business paying a customer.
		/// </remarks>
		Task<MpesaResponse> MakeB2CPaymentAsync(BusinessToCustomer businessToCustomer, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// Makes a Business to Customer payment request. Paybill to Phone Number.
		/// </summary>
		/// <param name="businessToCustomer">B2C data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		/// <remarks>
		/// Suitable for refunds, rewards or just about any transaction that involves a business paying a customer.
		/// </remarks>
		MpesaResponse MakeB2CPayment(BusinessToCustomer businessToCustomer, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Simulates a Customer to Business payment request.
		/// </summary>
		/// <param name="customerToBusinessSimulate">C2B data transfer object</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		/// <remarks>
		/// Use only for Simulation/testing. In production use <c>RegisterC2BUrlAsync</c> method to register 
		/// endpoints in your application that receive customer initiated transactions from the MPESA API
		/// for confirmation and/or validation
		/// </remarks>
		Task<MpesaResponse> MakeC2BPaymentAsync(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// Simulates a Customer to Business payment request.
		/// </summary>
		/// <param name="customerToBusinessSimulate">C2B data transfer object</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		/// <remarks>
		/// Use only for Simulation/testing. In production use <c>RegisterC2BUrlAsync</c> method to register 
		/// endpoints in your application that receive customer initiated transactions from the MPESA API
		/// for confirmation and/or validation
		/// </remarks>
		MpesaResponse MakeC2BPayment(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Registers Customer to Business Confirmation and Validation URLs.
		/// </summary>
		/// <param name="customerToBusinessRegisterUrl">C2B Register URLs data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<MpesaResponse> RegisterC2BUrlAsync(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Registers Customer to Business Confirmation and Validation URLs.
		/// </summary>
		/// <param name="customerToBusinessRegisterUrl">C2B Register URLs data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		MpesaResponse RegisterC2BUrl(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Reverses a Mpesa Transaction.
		/// </summary>
		/// <param name="mpesaReversal">Reversal data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<MpesaResponse> ReverseMpesaTransactionAsync(MpesaReversal mpesaReversal, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Reverses a Mpesa Transaction.
		/// </summary>
		/// <param name="mpesaReversal">Reversal data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		MpesaResponse ReverseMpesaTransaction(MpesaReversal mpesaReversal, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Queries the status of an Mpesa Transaction.
		/// </summary>
		/// <param name="mpesaTransactionStatus">Transaction Status data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<MpesaResponse> QueryMpesaTransactionStatusAsync(MpesaTransactionStatus mpesaTransactionStatus, string accesstoken, CancellationToken cancellationToken = default);


		/// <summary>
		/// Queries the status of an Mpesa Transaction.
		/// </summary>
		/// <param name="mpesaTransactionStatus">Transaction Status data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		MpesaResponse QueryMpesaTransactionStatus(MpesaTransactionStatus mpesaTransactionStatus, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// To make use of pull, a user will need to register their urls and shortcode. The shortcode used must be for a user who has gone live and is on production
		/// </summary>
		/// <param name="pullTransactionRegisterUrl">Pull Transaction Register Url data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<PullTransactionRegisterResponse> RegisterPullTransactionAsync(PullTransactionRegisterUrl pullTransactionRegisterUrl, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// To make use of pull, a user will need to register their urls and shortcode. The shortcode used must be for a user who has gone live and is on production
		/// </summary>
		/// <param name="pullTransactionRegisterUrl">Pull Transaction Register Url data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		PullTransactionRegisterResponse RegisterPullTransaction(PullTransactionRegisterUrl pullTransactionRegisterUrl, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// To make a pull of the missed transactions. Populate the request body with the following parameters.
		/// NB: This API pulls transactions for a period not exceeding 48hrs.
		/// </summary>
		/// <param name="pullTransactionQuery">Pull Transaction data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<PullTransactionResponse> QueryPullTransactionAsync(PullTransactionQuery pullTransactionQuery, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// To make a pull of the missed transactions. Populate the request body with the following parameters.
		/// NB: This API pulls transactions for a period not exceeding 48hrs.
		/// </summary>
		/// <param name="pullTransactionQuery">Pull Transaction data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		PullTransactionResponse QueryPullTransaction(PullTransactionQuery pullTransactionQuery, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// Use this API to generate a Dynamic QR which enables Safaricom M-PESA customers who have smartphones and use LIPA NA M-PESA as their preferred mode of payment, to scan a QR (Quick Response) code, to capture till number and amount then authorize to pay for goods and services at select LIPA NA M-PESA (LNM) merchant outlets.
		/// </summary>
		/// <param name="dynamicMpesaQR">Dynamic Mpesa QR data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		Task<DynamicMpesaQRResponse> GenerateDynamicMpesaQRAsync(DynamicMpesaQR dynamicMpesaQR, string accesstoken, CancellationToken cancellationToken = default);

		/// <summary>
		/// Use this API to generate a Dynamic QR which enables Safaricom M-PESA customers who have smartphones and use LIPA NA M-PESA as their preferred mode of payment, to scan a QR (Quick Response) code, to capture till number and amount then authorize to pay for goods and services at select LIPA NA M-PESA (LNM) merchant outlets.
		/// </summary>
		/// <param name="dynamicMpesaQR">Dynamic Mpesa QR data transfer object.</param>
		/// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <returns>A JSON string containing data from MPESA API reposnse.</returns>
		DynamicMpesaQRResponse GenerateDynamicMpesaQR(DynamicMpesaQR dynamicMpesaQR, string accesstoken, CancellationToken cancellationToken = default);

        /// <summary>
        /// This API enables businesses to remit tax to Kenya Revenue Authority (KRA). To use this API, prior integration is required with KRA for tax declaration, payment registration number (PRN) generation, and exchange of other tax-related information.
        /// </summary>
        /// <param name="taxRemittanceRequest"></param>
        /// <param name="accesstoken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<MpesaResponse> RemitTaxAsync(TaxRemittanceRequest taxRemittanceRequest, string accesstoken, CancellationToken cancellationToken = default);

        /// <summary>
        /// This API enables businesses to remit tax to Kenya Revenue Authority (KRA). To use this API, prior integration is required with KRA for tax declaration, payment registration number (PRN) generation, and exchange of other tax-related information.
        /// </summary>
        /// <param name="taxRemittanceRequest"></param>
        /// <param name="accesstoken"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        MpesaResponse RemitTax(TaxRemittanceRequest taxRemittanceRequest, string accesstoken, CancellationToken cancellationToken = default);
    }
}
