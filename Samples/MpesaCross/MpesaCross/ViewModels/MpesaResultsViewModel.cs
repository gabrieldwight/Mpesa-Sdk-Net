using Acr.UserDialogs;
using MpesaCross.Models;
using MpesaSdk;
using MpesaSdk.Dtos;
using MpesaSdk.Exceptions;
using MpesaSdk.Interfaces;
using MpesaSdk.Response;
using Newtonsoft.Json;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MpesaCross.ViewModels
{
    public class MpesaResultsViewModel : ViewModelBase
    {
        #region Properties
        private readonly IMpesaClient _mpesaClient;

        private LipaNaMpesaOnlinePushStkResponse _pushStKResponse;
        public LipaNaMpesaOnlinePushStkResponse PushStkResponse
        {
            get
            {
                return _pushStKResponse;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _pushStKResponse, value);
            }
        }

        private string _mpesaResultStatus;

        public string MpesaResultStatus
        {
            get
            {
                return _mpesaResultStatus;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _mpesaResultStatus, value);
            }
        }

        public ReactiveCommand<Unit, Unit> MpesaStkQueryCommand { get; }
        private INavigationService _navigationService { get; }
        protected IUserDialogs _dialogs { get; }

        private MpesaAPIConfiguration mpesaAPIConfiguration = new MpesaAPIConfiguration();
        #endregion

        #region Methods
        public MpesaResultsViewModel(INavigationService navigationService, IMpesaClient mpesaClient, IUserDialogs dialogs) : base(navigationService)
        {
            _navigationService = navigationService;
            _mpesaClient = mpesaClient;
            _dialogs = dialogs;

            MpesaStkQueryCommand = ReactiveCommand.CreateFromTask(x => ExecuteMpesaStkQueryCommand(PushStkResponse),
                this.WhenAnyValue(x => x.PushStkResponse)
                .Where(x => x != null)
                .Select(_pushResponse =>
                {
                    if (string.IsNullOrEmpty(_pushResponse.CheckoutRequestID))
                    {
                        return false;
                    }
                    return true;
                }));
            this.BindBusy(MpesaStkQueryCommand);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            PushStkResponse = JsonConvert.DeserializeObject<LipaNaMpesaOnlinePushStkResponse>(parameters.GetValue<string>("PushSTKResponse"));

            if (PushStkResponse.ResponseCode.Equals("0"))
            {
                MpesaResultStatus = $"{PushStkResponse.ResponseDescription}{Environment.NewLine}Inform customer to check his/her phone and to complete the Mpesa transaction by entering the M-PESA PIN.{Environment.NewLine}Customer Phone Number: {PushStkResponse.PhoneNumber}{Environment.NewLine}After completing the payment. Click the below button for confirmation.";
            }
            else
            {
                MpesaResultStatus = $"Something went wrong while processing request, please try again{Environment.NewLine}Customer Phone Number: {PushStkResponse.PhoneNumber}{Environment.NewLine}Response Description: {PushStkResponse.ResponseDescription}{Environment.NewLine}Customer Message: {PushStkResponse.CustomerMessage}";
            }
        }

        private async Task ExecuteMpesaStkQueryCommand(LipaNaMpesaOnlinePushStkResponse response)
        {
            try
            {
                var LipaNaMpesaOnlineQuery = new LipaNaMpesaQuery
                    (
                        businessShortCode: mpesaAPIConfiguration.LNMOshortCode,
                        timeStamp: DateTime.Now,
                        passkey: mpesaAPIConfiguration.PassKey,
                        checkoutRequestId: response.CheckoutRequestID
                    );

                var stkQueryResults = await _mpesaClient.QueryLipaNaMpesaTransactionAsync(LipaNaMpesaOnlineQuery, await RetrieveAccessToken(), MpesaRequestEndpoint.QueryLipaNaMpesaOnlineTransaction);

                if (stkQueryResults.ResultCode.Equals("0"))
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _dialogs.Alert(new AlertConfig()
                            .SetMessage("Thank you for your payment!")
                            .SetTitle(stkQueryResults.ResponseDescription)
                            .SetAction(async () => await _navigationService.NavigateAsync("/NavigationPage/MpesaPushStkPage")));
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _dialogs.Alert(new AlertConfig()
                            .SetMessage("Something went wrong with the transaction. Please try again")
                            .SetTitle(stkQueryResults.ResponseDescription)
                            .SetAction(async () => await _navigationService.NavigateAsync("/NavigationPage/MpesaPushStkPage")));
                    });
                }
            }
            catch (MpesaAPIException ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _dialogs.Alert(new AlertConfig()
                        .SetMessage(ex.Message.ToString())
                        .SetTitle(ex.StatusCode.ToString()));
                });
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _dialogs.Alert(new AlertConfig()
                        .SetMessage(ex.Message.ToString())
                        .SetTitle("Error"));
                });
            }
        }

        // To-Do refresh access token after every one hour
        private async Task<string> RetrieveAccessToken()
        {
            return await _mpesaClient.GetAuthTokenAsync(mpesaAPIConfiguration.ConsumerKey, mpesaAPIConfiguration.ConsumerSecret, MpesaRequestEndpoint.AuthToken);
        }
        #endregion
    }
}
