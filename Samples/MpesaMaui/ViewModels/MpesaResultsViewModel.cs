using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using MpesaMaui.Models;
using MpesaMaui.Navigation;
using MpesaSdk.Dtos;
using MpesaSdk.Exceptions;
using MpesaSdk.Interfaces;
using MpesaSdk.Response;

namespace MpesaMaui.ViewModels
{
	[QueryProperty(nameof(PushStKResponse), nameof(PushStKResponse))]
	public partial class MpesaResultsViewModel : ViewModelBase
	{
		#region Properties
		private readonly IMpesaClient _mpesaClient;

		[ObservableProperty]
		[NotifyCanExecuteChangedFor(nameof(MpesaStkQueryCommand))]
		public LipaNaMpesaOnlinePushStkResponse _pushStKResponse;

		[ObservableProperty]
		private string _mpesaResultStatus;

		public IAsyncRelayCommand MpesaStkQueryCommand { get; }

		protected IUserDialogs _dialogs { get; }

		private MpesaAPIConfiguration mpesaAPIConfiguration = new MpesaAPIConfiguration();
		#endregion

		#region Methods
		public MpesaResultsViewModel(INavigationService navigationService, IMpesaClient mpesaClient, IUserDialogs dialogs) : base(navigationService)
		{
			_mpesaClient = mpesaClient;
			_dialogs = dialogs;

			if (PushStKResponse.ResponseCode.Equals("0"))
			{
				MpesaResultStatus = $"{PushStKResponse.ResponseDescription}{Environment.NewLine}Inform customer to check his/her phone and to complete the Mpesa transaction by entering the M-PESA PIN.{Environment.NewLine}Customer Phone Number: {PushStKResponse.PhoneNumber}{Environment.NewLine}After completing the payment. Click the below button for confirmation.";
			}
			else
			{
				MpesaResultStatus = $"Something went wrong while processing request, please try again{Environment.NewLine}Customer Phone Number: {PushStKResponse.PhoneNumber}{Environment.NewLine}Response Description: {PushStKResponse.ResponseDescription}{Environment.NewLine}Customer Message: {PushStKResponse.CustomerMessage}";
			}

			MpesaStkQueryCommand = new AsyncRelayCommand(x => SendMpesaStkQueryAsync(PushStKResponse));

			this.BindBusy(MpesaStkQueryCommand);
		}

		private bool CanSendQuery()
		{
			return !string.IsNullOrWhiteSpace(PushStKResponse.CheckoutRequestID);
		}

		[RelayCommand(CanExecute = nameof(CanSendQuery))]
		private async Task SendMpesaStkQueryAsync(LipaNaMpesaOnlinePushStkResponse response)
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

				var stkQueryResults = await _mpesaClient.QueryLipaNaMpesaTransactionAsync(LipaNaMpesaOnlineQuery, await RetrieveAccessToken());

				if (stkQueryResults.ResultCode.Equals("0"))
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						_dialogs.Alert(new AlertConfig()
							.SetMessage("Thank you for your payment!")
							.SetTitle(stkQueryResults.ResponseDescription)
							.SetAction(async () => await NavigationService.NavigateToAsync("//MpesaPushStkPage")));
					});
				}
				else
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						_dialogs.Alert(new AlertConfig()
							.SetMessage("Something went wrong with the transaction. Please try again")
							.SetTitle(stkQueryResults.ResponseDescription)
							.SetAction(async () => await NavigationService.NavigateToAsync("//MpesaPushStkPage")));
					});
				}
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
