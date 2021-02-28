using Acr.UserDialogs;
using MpesaCross.Models;
using MpesaSdk;
using MpesaSdk.Dtos;
using MpesaSdk.Exceptions;
using MpesaSdk.Interfaces;
using Newtonsoft.Json;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MpesaCross.ViewModels
{
    public class MpesaPushStkViewModel : ViewModelBase
    {
        #region Properties
        private readonly IMpesaClient _mpesaClient;
        private string _amount;
        public string Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _amount, value);
            }
        }
        private string _accountReference;
        public string AccountReference
        {
            get
            {
                return _accountReference;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _accountReference, value);
            }
        }
        private string _transactionDescription;
        public string TransactionDescription
        {
            get
            {
                return _transactionDescription;
            }
            set 
            {
                this.RaiseAndSetIfChanged(ref _transactionDescription, value);
            }
        }
        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _phoneNumber, value);
            }
        }
        public ReactiveCommand<Unit, Unit> MpesaStkCommand { get; }
        private INavigationService _navigationService { get; }
        protected IUserDialogs _dialogs { get; }

        private MpesaAPIConfiguration mpesaAPIConfiguration = new MpesaAPIConfiguration();
        #endregion

        #region Methods
        public MpesaPushStkViewModel(INavigationService navigationService, IMpesaClient mpesaClient, IUserDialogs dialogs) : base(navigationService)
        {
            Title = "Mpesa Push Stk Payment";
            _navigationService = navigationService;
            _mpesaClient = mpesaClient;
            _dialogs = dialogs;
            MpesaStkCommand = ReactiveCommand.CreateFromTask(ExecuteMpesaStkCommand,
                this.WhenAnyValue(
                    x => x.Amount,
                    x => x.AccountReference,
                    x => x.TransactionDescription,
                    x => x.PhoneNumber,
                    (amount, accountReference, transactionDescription, phoneNumber) =>
                    {
                        if (string.IsNullOrEmpty(amount) && string.IsNullOrEmpty(accountReference) && string.IsNullOrEmpty(transactionDescription) && string.IsNullOrEmpty(phoneNumber))
                        {
                            return false;
                        }
                        return true;
                    }));
            this.BindBusy(MpesaStkCommand);
        }

        private async Task ExecuteMpesaStkCommand()
        {
            try
            {
                var mpesaPayment = new LipaNaMpesaOnline
                    (
                        businessShortCode: mpesaAPIConfiguration.LNMOshortCode,
                        timeStamp: DateTime.Now,
                        transactionType: Transaction_Type.CustomerPayBillOnline,
                        amount: Amount,
                        partyA: PhoneNumber,
                        partyB: mpesaAPIConfiguration.LNMOshortCode,
                        phoneNumber: PhoneNumber,
                        callBackUrl: mpesaAPIConfiguration.CallBackUrl.Replace("{requestId}", Guid.NewGuid().ToString()),
                        accountReference: AccountReference,
                        transactionDescription: TransactionDescription,
                        passkey: mpesaAPIConfiguration.PassKey
                    );

                var stkResults = await _mpesaClient.MakeLipaNaMpesaOnlinePaymentAsync(mpesaPayment, await RetrieveAccessToken(), MpesaRequestEndpoint.LipaNaMpesaOnline);
                stkResults.PhoneNumber = PhoneNumber;

                var navigationParams = new NavigationParameters();
                navigationParams.Add("PushSTKResponse", JsonConvert.SerializeObject(stkResults));
                await _navigationService.NavigateAsync("MpesaResultsPage", navigationParams);
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
