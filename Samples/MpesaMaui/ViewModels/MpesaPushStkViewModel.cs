using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using MpesaMaui.Models;
using MpesaMaui.Navigation;
using MpesaSdk;
using MpesaSdk.Dtos;
using MpesaSdk.Exceptions;
using MpesaSdk.Interfaces;

namespace MpesaMaui.ViewModels
{
	public partial class MpesaPushStkViewModel : ViewModelBase
	{
		#region Properties
		private readonly IMpesaClient _mpesaClient;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(SendMpesaStkCommand))]
		private string _amount;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(SendMpesaStkCommand))]
		private string _accountReference;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(SendMpesaStkCommand))]
		private string _transactionDescription;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(SendMpesaStkCommand))]
		private string _phoneNumber;

		protected IUserDialogs _dialogs { get; }

		private MpesaAPIConfiguration mpesaAPIConfiguration = new MpesaAPIConfiguration();
		#endregion

		#region Methods
		public MpesaPushStkViewModel(INavigationService navigationService, IMpesaClient mpesaClient, IUserDialogs dialogs) : base(navigationService)
		{
			Title = "Mpesa Push Stk Payment";
			_mpesaClient = mpesaClient;
			_dialogs = dialogs;
		}

		private bool CanSendPrompt()
		{
			return !string.IsNullOrWhiteSpace(Amount) && !string.IsNullOrWhiteSpace(AccountReference) && !string.IsNullOrWhiteSpace(TransactionDescription) && !string.IsNullOrWhiteSpace(PhoneNumber);
		}

		[RelayCommand(CanExecute = nameof(CanSendPrompt))]
		private async Task SendMpesaStkAsync()
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

				var stkResults = await _mpesaClient.MakeLipaNaMpesaOnlinePaymentAsync(mpesaPayment, await RetrieveAccessToken());
				stkResults.PhoneNumber = PhoneNumber;

				var navigationParams = new Dictionary<string, object>();
				navigationParams.Add("PushStKResponse", stkResults);

				await NavigationService.NavigateToAsync("MpesaResultsPage", navigationParams);
			}
			catch (MpesaAPIException ex)
			{
				MainThread.BeginInvokeOnMainThread(() =>
				{
					_dialogs.Alert(new AlertConfig()
						.SetMessage(ex.Message.ToString())
						.SetTitle(ex.StatusCode.ToString()));
				});
			}
			catch (Exception ex)
			{
				MainThread.BeginInvokeOnMainThread(() =>
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
			return await _mpesaClient.GetAuthTokenAsync(mpesaAPIConfiguration.ConsumerKey, mpesaAPIConfiguration.ConsumerSecret);
		}
		#endregion
	}
}
