using MpesaSdk.Dtos;
using MpesaSdk.Interfaces;
using MpesaSdk.Response;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MpesaSdk.Extensions
{
    /// <summary>
    /// Convenience overloads that acquire an OAuth token before invoking a Daraja API.
    /// </summary>
    public static class MpesaClientAuthenticationExtensions
    {
        public static Task<LipaNaMpesaOnlinePushStkResponse> MakeLipaNaMpesaOnlinePaymentAsync(
            this IMpesaClient client, LipaNaMpesaOnline request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeLipaNaMpesaOnlinePaymentAsync(request, token, ct));

        public static LipaNaMpesaOnlinePushStkResponse MakeLipaNaMpesaOnlinePayment(
            this IMpesaClient client, LipaNaMpesaOnline request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeLipaNaMpesaOnlinePayment(request, token, ct));

        public static Task<LipaNaMpesaQueryStkResponse> QueryLipaNaMpesaTransactionAsync(
            this IMpesaClient client, LipaNaMpesaQuery request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryLipaNaMpesaTransactionAsync(request, token, ct));

        public static LipaNaMpesaQueryStkResponse QueryLipaNaMpesaTransaction(
            this IMpesaClient client, LipaNaMpesaQuery request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryLipaNaMpesaTransaction(request, token, ct));

        public static Task<MpesaResponse> QueryAccountBalanceAsync(
            this IMpesaClient client, AccountBalance request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryAccountBalanceAsync(request, token, ct));

        public static MpesaResponse QueryAccountBalance(
            this IMpesaClient client, AccountBalance request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryAccountBalance(request, token, ct));

        public static Task<MpesaResponse> MakeB2BPaymentAsync(
            this IMpesaClient client, BusinessToBusiness request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeB2BPaymentAsync(request, token, ct));

        public static MpesaResponse MakeB2BPayment(
            this IMpesaClient client, BusinessToBusiness request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeB2BPayment(request, token, ct));

        public static Task<MpesaResponse> BusinessPayBillAsync(
            this IMpesaClient client, BusinessPayBillRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BusinessPayBillAsync(request, token, ct));

        public static MpesaResponse BusinessPayBill(
            this IMpesaClient client, BusinessPayBillRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BusinessPayBill(request, token, ct));

        public static Task<MpesaResponse> BusinessBuyGoodsAsync(
            this IMpesaClient client, BusinessBuyGoodsRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BusinessBuyGoodsAsync(request, token, ct));

        public static MpesaResponse BusinessBuyGoods(
            this IMpesaClient client, BusinessBuyGoodsRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BusinessBuyGoods(request, token, ct));

        public static Task<MpesaResponse> MakeB2CPaymentAsync(
            this IMpesaClient client, BusinessToCustomer request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeB2CPaymentAsync(request, token, ct));

        public static MpesaResponse MakeB2CPayment(
            this IMpesaClient client, BusinessToCustomer request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeB2CPayment(request, token, ct));

        public static Task<MpesaResponse> MakeC2BPaymentAsync(
            this IMpesaClient client, CustomerToBusinessSimulate request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeC2BPaymentAsync(request, token, ct));

        public static MpesaResponse MakeC2BPayment(
            this IMpesaClient client, CustomerToBusinessSimulate request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.MakeC2BPayment(request, token, ct));

        public static Task<MpesaResponse> RegisterC2BUrlAsync(
            this IMpesaClient client, CustomerToBusinessRegisterUrl request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.RegisterC2BUrlAsync(request, token, ct));

        public static MpesaResponse RegisterC2BUrl(
            this IMpesaClient client, CustomerToBusinessRegisterUrl request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.RegisterC2BUrl(request, token, ct));

        public static Task<MpesaResponse> ReverseMpesaTransactionAsync(
            this IMpesaClient client, MpesaReversal request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.ReverseMpesaTransactionAsync(request, token, ct));

        public static MpesaResponse ReverseMpesaTransaction(
            this IMpesaClient client, MpesaReversal request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.ReverseMpesaTransaction(request, token, ct));

        public static Task<MpesaResponse> QueryMpesaTransactionStatusAsync(
            this IMpesaClient client, MpesaTransactionStatus request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryMpesaTransactionStatusAsync(request, token, ct));

        public static MpesaResponse QueryMpesaTransactionStatus(
            this IMpesaClient client, MpesaTransactionStatus request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryMpesaTransactionStatus(request, token, ct));

        public static Task<PullTransactionRegisterResponse> RegisterPullTransactionAsync(
            this IMpesaClient client, PullTransactionRegisterUrl request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.RegisterPullTransactionAsync(request, token, ct));

        public static PullTransactionRegisterResponse RegisterPullTransaction(
            this IMpesaClient client, PullTransactionRegisterUrl request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.RegisterPullTransaction(request, token, ct));

        public static Task<PullTransactionResponse> QueryPullTransactionAsync(
            this IMpesaClient client, PullTransactionQuery request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryPullTransactionAsync(request, token, ct));

        public static PullTransactionResponse QueryPullTransaction(
            this IMpesaClient client, PullTransactionQuery request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.QueryPullTransaction(request, token, ct));

        public static Task<DynamicMpesaQRResponse> GenerateDynamicMpesaQRAsync(
            this IMpesaClient client, DynamicMpesaQR request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.GenerateDynamicMpesaQRAsync(request, token, ct));

        public static DynamicMpesaQRResponse GenerateDynamicMpesaQR(
            this IMpesaClient client, DynamicMpesaQR request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.GenerateDynamicMpesaQR(request, token, ct));

        public static Task<MpesaResponse> RemitTaxAsync(
            this IMpesaClient client, TaxRemittanceRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.RemitTaxAsync(request, token, ct));

        public static MpesaResponse RemitTax(
            this IMpesaClient client, TaxRemittanceRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.RemitTax(request, token, ct));

        public static Task<BillManagerResponse> BillManagerOnboardingAsync(
            this IMpesaClient client, BillManagerOnboardingRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerOnboardingAsync(request, token, ct));

        public static BillManagerResponse BillManagerOnboarding(
            this IMpesaClient client, BillManagerOnboardingRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerOnboarding(request, token, ct));

        public static Task<BillManagerResponse> BillManagerSingleInvoiceAsync(
            this IMpesaClient client, BillManagerInvoicingRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerSingleInvoiceAsync(request, token, ct));

        public static BillManagerResponse BillManagerSingleInvoice(
            this IMpesaClient client, BillManagerInvoicingRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerSingleInvoice(request, token, ct));

        public static Task<BillManagerResponse> BillManagerBulkInvoiceAsync(
            this IMpesaClient client, System.Collections.Generic.List<BillManagerInvoicingRequest> requests,
            string consumerKey, string consumerSecret, CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerBulkInvoiceAsync(requests, token, ct));

        public static BillManagerResponse BillManagerBulkInvoice(
            this IMpesaClient client, System.Collections.Generic.List<BillManagerInvoicingRequest> requests,
            string consumerKey, string consumerSecret, CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerBulkInvoice(requests, token, ct));

        public static Task<BillManagerResponse> BillManagerPaymentReconcilliationAsync(
            this IMpesaClient client, BillManagerPaymentReconcilliationRequest request,
            string consumerKey, string consumerSecret, CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerPaymentReconcilliationAsync(request, token, ct));

        public static BillManagerResponse BillManagerPaymentReconcilliation(
            this IMpesaClient client, BillManagerPaymentReconcilliationRequest request,
            string consumerKey, string consumerSecret, CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerPaymentReconcilliation(request, token, ct));

        public static Task<BillManagerResponse> BillManagerUpdateInvoiceAsync(
            this IMpesaClient client, BillManagerInvoicingRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            InvokeAsync(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerUpdateInvoiceAsync(request, token, ct));

        public static BillManagerResponse BillManagerUpdateInvoice(
            this IMpesaClient client, BillManagerInvoicingRequest request, string consumerKey, string consumerSecret,
            CancellationToken cancellationToken = default) =>
            Invoke(client, consumerKey, consumerSecret, cancellationToken,
                (mpesaClient, token, ct) => mpesaClient.BillManagerUpdateInvoice(request, token, ct));

        private static async Task<TResponse> InvokeAsync<TResponse>(
            IMpesaClient client, string consumerKey, string consumerSecret, CancellationToken cancellationToken,
            Func<IMpesaClient, string, CancellationToken, Task<TResponse>> operation)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var accessToken = await client.GetAuthTokenAsync(consumerKey, consumerSecret, cancellationToken)
                .ConfigureAwait(false);
            return await operation(client, accessToken, cancellationToken).ConfigureAwait(false);
        }

        private static TResponse Invoke<TResponse>(
            IMpesaClient client, string consumerKey, string consumerSecret, CancellationToken cancellationToken,
            Func<IMpesaClient, string, CancellationToken, TResponse> operation)
        {
            if (client is null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            var accessToken = client.GetAuthToken(consumerKey, consumerSecret, cancellationToken);
            return operation(client, accessToken, cancellationToken);
        }
    }
}
