using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MpesaSdk.Dtos;
using MpesaSdk.Enums;
using MpesaSdk.Exceptions;
using MpesaSdk.Interfaces;
using MpesaSdk.Response;
using MpesaSdk.Web.Extensions.Alerts;
using MpesaSdk.Web.Models;
using MpesaSdk.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace MpesaSdk.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly MpesaApiConfiguration _mpesaApiConfiguration;
        private readonly IMpesaClient _mpesaClient;
        private readonly ILogger<HomeController> _logger;
        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;

        public HomeController(IOptions<MpesaApiConfiguration> mpesaApiConfiguration, IMpesaClient mpesaClient, 
            ILogger<HomeController> logger, LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor,
            IMemoryCache memoryCache)
        {
            _mpesaApiConfiguration = mpesaApiConfiguration.Value;
            _mpesaClient = mpesaClient;
            _logger = logger;
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MpesaPayment()
        {
            return View();
        }

        public IActionResult DynamicQR()
        {
            DynamicQRViewModel dynamicQRView = new DynamicQRViewModel();
            dynamicQRView.QRType = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Dynamic", Value = QRType.Dynamic },
                new SelectListItem() { Text = "Static", Value = QRType.Static }
            };

            dynamicQRView.QRFormat = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Image Format", Value = ((int)QRFormat.ImageFormat).ToString() },
                new SelectListItem() { Text = "QR String Format", Value = ((int)QRFormat.QRStringFormat).ToString() },
                new SelectListItem() { Text = "Binary Data Format", Value = ((int)QRFormat.BinaryDataFormat).ToString() },
                new SelectListItem() { Text = "PDF Format", Value = ((int)QRFormat.PDFFormat).ToString() },
            };

            dynamicQRView.TransactionCode = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "Pay Merchant (Buy Goods)", Value = QRTransactionType.PayMerchant },
                new SelectListItem() { Text = "Withdraw Cash at Agent Till", Value = QRTransactionType.WithdrawCash },
                new SelectListItem() { Text = "Paybill or Business Number", Value = QRTransactionType.PayBill },
                new SelectListItem() { Text = "Send Money(Mobile number)", Value = QRTransactionType.SendMoney },
                new SelectListItem() { Text = "Sent to Business. Business number CPI in MSISDN format", Value = QRTransactionType.SendBusiness }
            };

            return View(dynamicQRView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DynamicQR(DynamicQRViewModel dynamicQR)
        {
            DynamicMpesaQR dynamicMpesaQR = new DynamicMpesaQR(qrVersion: "01",
                qrFormat: Convert.ToInt32(dynamicQR.SelectedQRFormat),
                qrType: dynamicQR.SelectedQRType,
                merchantName: dynamicQR.MerchantName,
                refNo: dynamicQR.Reference,
                amount: dynamicQR.Amount,
                trxCode: dynamicQR.SelectedTransactionCode,
                cpi: dynamicQR.CPI);

            try
            {
                DynamicMpesaQRResponse dynamicMpesaQRResponse = await _mpesaClient.GenerateDynamicMpesaQRAsync(dynamicMpesaQR, await RetrieveAccessToken(), MpesaRequestEndpoint.DynamicMpesaQR);

                if (dynamicMpesaQRResponse.ResponseCode.Equals("00"))
                {
                    dynamicQR.GeneratedQRCode = dynamicMpesaQRResponse.QRCode;

                    if (dynamicQR.GeneratedQRFileExtension.Equals("png"))
                    {
                        return View(dynamicQR).WithSuccess("Success", "Mpesa QR Code generated successfully");
                    }
                    else if (dynamicQR.GeneratedQRFileExtension.Equals("pdf"))
                    {
                        var stream = new FileStream(@"generatedMpesaQR.pdf", FileMode.Open);
                        return new FileStreamResult(stream, "application/pdf");
                    }
                }
            }
            catch (MpesaAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return RedirectToAction(nameof(DynamicQR)).WithDanger("Error", ex.Message);
            }
            return RedirectToAction(nameof(DynamicQR));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MpesaPayment(LipaNaMpesaOnlineViewModel lipaNaMpesaOnline)
        {
            LipaNaMpesaOnlinePushStkResponse mpesaPaymentRequest;

            try
            {
#if DEBUG
                var generatedCallBackUrl = _mpesaApiConfiguration.CallBackUrl.Replace("{requestId}", Guid.NewGuid().ToString());
#else
                var generatedCallBackUrl = _linkGenerator.GetUriByAction(_httpContextAccessor.HttpContext, 
                    action: nameof(APIControllers.PaymentCallbackController.LipaNaMpesaCallback), 
                    controller: nameof(APIControllers.PaymentCallbackController).Replace("Controller", ""),
                    values: new { requestId = Guid.NewGuid() },
                    scheme: Request.Scheme);
#endif
                var mpesaPayment = new LipaNaMpesaOnline
                (
                    businessShortCode: _mpesaApiConfiguration.LNMOshortCode,
                    timeStamp: DateTime.Now,
                    transactionType: Transaction_Type.CustomerPayBillOnline,
                    amount: lipaNaMpesaOnline.Amount,
                    partyA: lipaNaMpesaOnline.PhoneNumber,
                    partyB: _mpesaApiConfiguration.LNMOshortCode,
                    phoneNumber: lipaNaMpesaOnline.PhoneNumber,
                    callBackUrl: generatedCallBackUrl,
                    accountReference: lipaNaMpesaOnline.AccountReference,
                    transactionDescription: lipaNaMpesaOnline.TransactionDesc,
                    passkey: _mpesaApiConfiguration.PassKey
                );

                mpesaPaymentRequest = await _mpesaClient.MakeLipaNaMpesaOnlinePaymentAsync(mpesaPayment, await RetrieveAccessToken(), MpesaRequestEndpoint.LipaNaMpesaOnline);
            }
            catch (MpesaAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return View().WithDanger("Error", ex.Message);
            }

            var pushStkResponse = mpesaPaymentRequest;
            pushStkResponse.PhoneNumber = lipaNaMpesaOnline.PhoneNumber;

            return RedirectToAction(nameof(ShowMpesaResult), pushStkResponse).WithInfo("Info", "Please wait for confirmation message to arrive on your phone.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MpesaOnlineTransactionStatus(LipaNaMpesaOnlinePushStkResponse response)
        {
            LipaNaMpesaQueryStkResponse queryResult;

            try
            {
                var LipaNaMpesaOnlineQuery = new LipaNaMpesaQuery
                (
                    businessShortCode: _mpesaApiConfiguration.LNMOshortCode,
                    passkey: _mpesaApiConfiguration.PassKey,
                    timeStamp: DateTime.Now,
                    checkoutRequestId: response.CheckoutRequestID
                );

                queryResult = await _mpesaClient.QueryLipaNaMpesaTransactionAsync(LipaNaMpesaOnlineQuery, await RetrieveAccessToken(), MpesaRequestEndpoint.QueryLipaNaMpesaOnlineTransaction);
            }
            catch (MpesaAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return View().WithDanger("Error", ex.Message);
            }            

            var LNMOQueryResponse = queryResult;
            LNMOQueryResponse.PhoneNumber = response.PhoneNumber;

            return RedirectToAction(nameof(MpesaPaymentConfirmation), LNMOQueryResponse);
        }


        public IActionResult ShowMpesaResult(LipaNaMpesaOnlinePushStkResponse response)
        {
            return View(response);
        }

        public IActionResult MpesaPaymentConfirmation(LipaNaMpesaQueryStkResponse response)
        {
            return View(response);
        }


        private async Task<string> RetrieveAccessToken()
        {
            var cacheKey = "MpesaAccessToken";

            if (_memoryCache.TryGetValue(cacheKey, out string mpesaAccessToken))
            {
                _logger.LogInformation("Getting token from memory...");
                return mpesaAccessToken;
            }
            else
            {
                _logger.LogInformation("Getting token from Mpesa Server...");
                mpesaAccessToken = await _mpesaClient.GetAuthTokenAsync(_mpesaApiConfiguration.ConsumerKey, _mpesaApiConfiguration.ConsumerSecret, MpesaRequestEndpoint.AuthToken);

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(59));

                // Save data in cache
                _memoryCache.Set(cacheKey, mpesaAccessToken, cacheEntryOptions);

                return mpesaAccessToken;
            }
        }

        public IActionResult C2BRegister()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> C2BRegister(CustomerToBusinessRegisterViewModel customerToBusinessRegisterViewModel)
        {
            MpesaResponse c2BRegisterResults;

            try
            {
                var c2BRegisterCallback = new CustomerToBusinessRegisterUrl
                    (
                        shortCode: _mpesaApiConfiguration.C2BShortCode,
                        responseType: customerToBusinessRegisterViewModel.customerToBusinessResponse.ToString(),
                        confirmationUrl: customerToBusinessRegisterViewModel.ConfirmationUrl,
                        validationUrl: customerToBusinessRegisterViewModel.ValidationUrl
                    );

                c2BRegisterResults = await _mpesaClient.RegisterC2BUrlAsync(c2BRegisterCallback, await RetrieveAccessToken(), MpesaRequestEndpoint.RegisterC2BUrl);
            }
            catch (MpesaAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return View().WithDanger("Error", ex.Message);
            }

            return View().WithSuccess("Success", "Successfully added C2B confirmation and validation URLs");
        }

        public IActionResult C2BPayment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> C2BPayment(CustomerToBusinessViewModel customerToBusinessViewModel)
        {
            MpesaResponse c2BResults;

            try
            {
                var c2BPayment = new CustomerToBusinessSimulate
                (
                    shortCode: _mpesaApiConfiguration.C2BShortCode,
                    commandId: Transaction_Type.CustomerPayBillOnline,
                    amount: customerToBusinessViewModel.Amount,
                    msisdn: _mpesaApiConfiguration.C2BMSISDNTest,
                    billReferenceNumber: customerToBusinessViewModel.PaymentReference
                );

                c2BResults = await _mpesaClient.MakeC2BPaymentAsync(c2BPayment, await RetrieveAccessToken(), MpesaRequestEndpoint.CustomerToBusinessSimulate);
            }
            catch (MpesaAPIException ex)
            {
                _logger.LogError(ex, ex.Message);
                return View().WithDanger("Error", ex.Message);
            }

            return View().WithSuccess("Success", "You have successfully submitted C2B payment.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
