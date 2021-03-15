using Microsoft.Extensions.DependencyInjection;
using MpesaSdk.Dtos;
using MpesaSdk.Exceptions;
using MpesaSdk.Interfaces;
using MpesaSdk.Response;
using MpesaSdk.Validators;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MpesaSdk
{
    /// <summary>
    /// Mpesa client class provides all the implemented interface methods that make the different API calls to MPESA Server
    /// </summary>
    public class MpesaClient : IMpesaClient
    {
        private readonly HttpClient _client;
        Random jitterer = new Random();
        private JsonSerializer _serializer = new JsonSerializer();

        /// <summary>
        /// MpesaClient that creates a client using httpclientfactory
        /// </summary>
        /// <param name="client">HttpClient Instance from httpClientFactory</param>
        public MpesaClient(Enums.Environment environment)
        {
            var retryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(1, retryAttempt =>
                {
                    return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(jitterer.Next(0, 100));
                });

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            var services = new ServiceCollection();
            services.AddHttpClient("MpesaApiClient", client =>
            {
                switch (environment)
                {
                    case Enums.Environment.Sandbox:
                        client.BaseAddress = MpesaRequestEndpoint.SandboxBaseAddress;
                        client.Timeout = TimeSpan.FromMinutes(10);
                        break;

                    case Enums.Environment.Live:
                        client.BaseAddress = MpesaRequestEndpoint.LiveBaseAddress;
                        client.Timeout = TimeSpan.FromMinutes(10);
                        break;

                    default:
                        break;
                }
            }).ConfigurePrimaryHttpMessageHandler(messageHandler =>
            {
                var handler = new HttpClientHandler();

                if (handler.SupportsAutomaticDecompression)
                {
                    handler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                }

                return handler;
            }).AddPolicyHandler(request => request.Method.Equals(HttpMethod.Get) ? retryPolicy : noOpPolicy);

            var serviceProvider = services.BuildServiceProvider();

            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

            _client = httpClientFactory.CreateClient("MpesaApiClient");
        }

        /// <summary>
        /// MpesaClient takes in an instance of HttpClient which can be used in Dependency Injection Container
        /// </summary>
        /// <param name="client">HttpClient Instance</param>
        public MpesaClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// GetAuthTokenAsync is an asynchronous method that requests for and returns an accesstoken from MPESA API Server.
        /// </summary>
        /// <param name="consumerKey">ConsumerKey provided by Safaricom in Daraja Portal.</param>
        /// <param name="consumerSecret">ConsumerSecret provided by Safaricom in Daraja Portal.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.AuthToken</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A string of characters representing the accesstoken.</returns>
        public string GetAuthToken(string consumerKey, string consumerSecret, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            return RequestAccessTokenAsync(consumerKey, consumerSecret, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// GetAuthTokenAsync is an asynchronous method that requests for and returns an accesstoken from MPESA API Server.
        /// </summary>
        /// <param name="consumerKey">ConsumerKey provided by Safaricom in Daraja Portal.</param>
        /// <param name="consumerSecret">ConsumerSecret provided by Safaricom in Daraja Portal.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.AuthToken</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A string of characters representing the accesstoken.</returns>
        public async Task<string> GetAuthTokenAsync(string consumerKey, string consumerSecret, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            return await RequestAccessTokenAsync(consumerKey, consumerSecret, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Makes a Business to Business payment request between Paybill numbers.
        /// </summary>
        /// <param name="businessToBusiness">B2B data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.BusinessToBusiness</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public MpesaResponse MakeB2BPayment(BusinessToBusiness businessToBusiness, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            return MpesaPostRequestAsync<MpesaResponse>(businessToBusiness, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Makes a Business to Business payment request between Paybill numbers.
        /// </summary>
        /// <param name="businessToBusiness">B2B data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.BusinessToBusiness</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public async Task<MpesaResponse> MakeB2BPaymentAsync(BusinessToBusiness businessToBusiness, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            return await MpesaPostRequestAsync<MpesaResponse>(businessToBusiness, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Makes a Business to Customer payment request. Paybill to Phone Number.
        /// </summary>
        /// <param name="businessToCustomer">B2C data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.BusinessToCustomer</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Suitable for refunds, rewards or just about any transaction that involves a business paying a customer.
        /// </remarks>
        public MpesaResponse MakeB2CPayment(BusinessToCustomer businessToCustomer, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new BusinessToCustomerValidator();
            var results = validator.Validate(businessToCustomer);

            return !results.IsValid 
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<MpesaResponse>(businessToCustomer, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Makes a Business to Customer payment request. Paybill to Phone Number.
        /// </summary>
        /// <param name="businessToCustomer">B2C data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.BusinessToCustomer</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Suitable for refunds, rewards or just about any transaction that involves a business paying a customer.
        /// </remarks>
        public async Task<MpesaResponse> MakeB2CPaymentAsync(BusinessToCustomer businessToCustomer, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new BusinessToCustomerValidator();
            var results = await validator.ValidateAsync(businessToCustomer);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<MpesaResponse>(businessToCustomer, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Simulates a Customer to Business payment request.
        /// </summary>
        /// <param name="customerToBusinessSimulate">C2B data transfer object</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.CustomerToBusinessSimulate</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Use only for Simulation/testing. In production use <c>RegisterC2BUrlAsync</c> method to register 
        /// endpoints in your application that receive customer initiated transactions from the MPESA API
        /// for confirmation and/or validation
        /// </remarks>
        public MpesaResponse MakeC2BPayment(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new CustomerToBusinessSimulateTransactionValidator();
            var results = validator.Validate(customerToBusinessSimulate);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<MpesaResponse>(customerToBusinessSimulate, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Simulates a Customer to Business payment request.
        /// </summary>
        /// <param name="customerToBusinessSimulate">C2B data transfer object</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.CustomerToBusinessSimulate</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        /// <remarks>
        /// Use only for Simulation/testing. In production use <c>RegisterC2BUrlAsync</c> method to register 
        /// endpoints in your application that receive customer initiated transactions from the MPESA API
        /// for confirmation and/or validation
        /// </remarks>
        public async Task<MpesaResponse> MakeC2BPaymentAsync(CustomerToBusinessSimulate customerToBusinessSimulate, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new CustomerToBusinessSimulateTransactionValidator();
            var results = await validator.ValidateAsync(customerToBusinessSimulate);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<MpesaResponse>(customerToBusinessSimulate, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Makes an STK Push payment request to MPESA API Server.
        /// </summary>
        /// <param name="lipaNaMpesaOnline">
        /// Data transfer object containing properties for the Lipa Na Mpesa Online API endpoint.
        /// </param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.LipaNaMpesaOnline</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing LNMO response data from MPESA API Server</returns>
        public LipaNaMpesaOnlinePushStkResponse MakeLipaNaMpesaOnlinePayment(LipaNaMpesaOnline lipaNaMpesaOnline, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new LipaNaMpesaOnlineValidator();
            var results = validator.Validate(lipaNaMpesaOnline);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<LipaNaMpesaOnlinePushStkResponse>(lipaNaMpesaOnline, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Makes an STK Push payment request to MPESA API Server.
        /// </summary>
        /// <param name="lipaNaMpesaOnline">
        /// Data transfer object containing properties for the Lipa Na Mpesa Online API endpoint.
        /// </param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.LipaNaMpesaOnline</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing LNMO response data from MPESA API Server</returns>
        public async Task<LipaNaMpesaOnlinePushStkResponse> MakeLipaNaMpesaOnlinePaymentAsync(LipaNaMpesaOnline lipaNaMpesaOnline, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new LipaNaMpesaOnlineValidator();
            var results = await validator.ValidateAsync(lipaNaMpesaOnline);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<LipaNaMpesaOnlinePushStkResponse>(lipaNaMpesaOnline, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Queries MPESA Paybill Account balance.
        /// </summary>
        /// <param name="accountBalance">Account balance query data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.QueryAccountBalance</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public MpesaResponse QueryAccountBalance(AccountBalance accountBalance, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new AccountBalanceValidator();
            var results = validator.Validate(accountBalance);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<MpesaResponse>(accountBalance, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Queries MPESA Paybill Account balance.
        /// </summary>
        /// <param name="accountBalance">Account balance query data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.QueryAccountBalance</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public async Task<MpesaResponse> QueryAccountBalanceAsync(AccountBalance accountBalance, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new AccountBalanceValidator();
            var results = await validator.ValidateAsync(accountBalance);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<MpesaResponse>(accountBalance, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Queries Mpesa Online Transaction Status
        /// </summary>
        /// <param name="lipaNaMpesaQuery">Transaction Query Data transfer object</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.QueryLipaNaMpesaOnlieTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>
        /// A JSON string containing data from MPESA API reposnse
        /// </returns>
        /// <remarks>
        /// Use only for transactions initiated with <c>MakeLipaNaMpesaOnlinePayment</c> method.
        /// For Other transaction based methods (C2B,B2C,B2B) use <c>QueryMpesaTransactionStatusAsync</c> method.
        /// </remarks>
        public LipaNaMpesaQueryStkResponse QueryLipaNaMpesaTransaction(LipaNaMpesaQuery lipaNaMpesaQuery, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new LipaNaMpesaQueryValidator();
            var results = validator.Validate(lipaNaMpesaQuery);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<LipaNaMpesaQueryStkResponse>(lipaNaMpesaQuery, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Queries Mpesa Online Transaction Status
        /// </summary>
        /// <param name="lipaNaMpesaQuery">Transaction Query Data transfer object</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.QueryLipaNaMpesaOnlieTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>
        /// A JSON string containing data from MPESA API reposnse
        /// </returns>
        /// <remarks>
        /// Use only for transactions initiated with <c>MakeLipaNaMpesaOnlinePayment</c> method.
        /// For Other transaction based methods (C2B,B2C,B2B) use <c>QueryMpesaTransactionStatusAsync</c> method.
        /// </remarks>
        public async Task<LipaNaMpesaQueryStkResponse> QueryLipaNaMpesaTransactionAsync(LipaNaMpesaQuery lipaNaMpesaQuery, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new LipaNaMpesaQueryValidator();
            var results = await validator.ValidateAsync(lipaNaMpesaQuery);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<LipaNaMpesaQueryStkResponse>(lipaNaMpesaQuery, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Queries status of an Mpesa transaction
        /// </summary>
        /// <param name="mpesaTransactionStatus"></param>
        /// <param name="accesstoken"></param>
        /// <param name="mpesaRequestEndpoint"></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public MpesaResponse QueryMpesaTransactionStatus(MpesaTransactionStatus mpesaTransactionStatus, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new MpesaTransactionStatusValidator();
            var results = validator.Validate(mpesaTransactionStatus);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<MpesaResponse>(mpesaTransactionStatus, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Queries status of an Mpesa transaction
        /// </summary>
        /// <param name="mpesaTransactionStatus"></param>
        /// <param name="accesstoken"></param>
        /// <param name="mpesaRequestEndpoint"></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns></returns>
        public async Task<MpesaResponse> QueryMpesaTransactionStatusAsync(MpesaTransactionStatus mpesaTransactionStatus, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new MpesaTransactionStatusValidator();
            var results = await validator.ValidateAsync(mpesaTransactionStatus);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<MpesaResponse>(mpesaTransactionStatus, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Registers Customer to Business Confirmation and Validation URLs.
        /// </summary>
        /// <param name="customerToBusinessRegisterUrl">C2B Register URLs data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.RegisterC2BUrl</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public MpesaResponse RegisterC2BUrl(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new CustomerToBusinessRegisterUrlValidator();
            var results = validator.Validate(customerToBusinessRegisterUrl);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<MpesaResponse>(customerToBusinessRegisterUrl, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Registers Customer to Business Confirmation and Validation URLs.
        /// </summary>
        /// <param name="customerToBusinessRegisterUrl">C2B Register URLs data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.RegisterC2BUrl</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public async Task<MpesaResponse> RegisterC2BUrlAsync(CustomerToBusinessRegisterUrl customerToBusinessRegisterUrl, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new CustomerToBusinessRegisterUrlValidator();
            var results = await validator.ValidateAsync(customerToBusinessRegisterUrl);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<MpesaResponse>(customerToBusinessRegisterUrl, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Reverses a B2B, B2C or C2B M-Pesa transaction.
        /// </summary>
        /// <param name="mpesaReversal">Reversal data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.ReverseMpesaTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public MpesaResponse ReverseMpesaTransaction(MpesaReversal mpesaReversal, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new MpesaReversalValidator();
            var results = validator.Validate(mpesaReversal);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : MpesaPostRequestAsync<MpesaResponse>(mpesaReversal, accesstoken, mpesaRequestEndpoint, cancellationToken).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Reverses a B2B, B2C or C2B M-Pesa transaction.
        /// </summary>
        /// <param name="mpesaReversal">Reversal data transfer object.</param>
        /// <param name="accesstoken">Acccesstoken retrieved by the <c>GetAuthTokenAsync</c> method.</param>
        /// <param name="mpesaRequestEndpoint">Set to <c>MpesaRequestEndpoint.ReverseMpesaTransaction</c></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A JSON string containing data from MPESA API reposnse.</returns>
        public async Task<MpesaResponse> ReverseMpesaTransactionAsync(MpesaReversal mpesaReversal, string accesstoken, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var validator = new MpesaReversalValidator();
            var results = await validator.ValidateAsync(mpesaReversal);

            return !results.IsValid
                ? throw new MpesaAPIException(HttpStatusCode.BadRequest, string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage.ToString())))
                : await MpesaPostRequestAsync<MpesaResponse>(mpesaReversal, accesstoken, mpesaRequestEndpoint, cancellationToken);
        }

        /// <summary>
        /// Makes HttpRequest to mpesa api server
        /// </summary>
        /// <param name="mpesaDto">Data transfer object</param>
        /// <param name="accessToken">Mpesa Accesstoken</param>
        /// <param name="mpesaEndpoint">Request endpoint</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Mpesa API response based on the API requests made.</returns>
        private async Task<T> MpesaPostRequestAsync<T>(object mpesaDto, string accessToken, string mpesaEndpoint, CancellationToken cancellationToken = default) where T : new()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            T result = new();
            string json = JsonConvert.SerializeObject(mpesaDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _client.PostAsync(mpesaEndpoint, content, cancellationToken).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<T>(json);
                }, cancellationToken);
            }
            else
            {
                MpesaErrorResponse mpesaErrorResponse = new MpesaErrorResponse();
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    mpesaErrorResponse = _serializer.Deserialize<MpesaErrorResponse>(json);
                }, cancellationToken);
                throw new MpesaAPIException(new HttpRequestException(mpesaErrorResponse.ErrorMessage), response.StatusCode, mpesaErrorResponse);
            }
            return result;
        }

        /// <summary>
        /// Method makes the accesstoken request to mpesa api
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="mpesaRequestEndpoint"></param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>string representing accesstoken for the mpesa api calls</returns>
        private async Task<string> RequestAccessTokenAsync(string consumerKey, string consumerSecret, string mpesaRequestEndpoint, CancellationToken cancellationToken = default)
        {
            var keyBytes = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{consumerKey}:{consumerSecret}"));

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", keyBytes);
            MpesaAccessTokenResponse result = new MpesaAccessTokenResponse();
            cancellationToken.ThrowIfCancellationRequested();
            var response = await _client.GetAsync(mpesaRequestEndpoint, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    result = _serializer.Deserialize<MpesaAccessTokenResponse>(json);
                }, cancellationToken);
            }
            else
            {
                MpesaErrorResponse mpesaErrorResponse = new MpesaErrorResponse();
                await response.Content.ReadAsStreamAsync().ContinueWith((Task<Stream> stream) =>
                {
                    using var reader = new StreamReader(stream.Result);
                    using var json = new JsonTextReader(reader);
                    mpesaErrorResponse = _serializer.Deserialize<MpesaErrorResponse>(json);
                }, cancellationToken);
                throw new MpesaAPIException(new HttpRequestException(mpesaErrorResponse.ErrorMessage), response.StatusCode, mpesaErrorResponse);
            }
            return result.AccessToken;
        }
    }
}
