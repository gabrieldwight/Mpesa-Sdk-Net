using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using MpesaMaui.Navigation;

namespace MpesaMaui.ViewModels
{
	public abstract partial class ViewModelBase : ObservableObject
	{
		public bool IsNotConnected { get; set; }

		[ObservableProperty]
		private string _title;

		[ObservableProperty]
		private bool _isBusy;

		public bool IsNotBusy
		{
			get
			{
				return !IsBusy;
			}
		}

		public INavigationService NavigationService { get; set; }

		public ViewModelBase(INavigationService navigationService)
		{
			NavigationService = navigationService;
			Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
			IsNotConnected = Connectivity.NetworkAccess != NetworkAccess.Internet;
		}

		~ViewModelBase()
		{
			Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
		}

		void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
		{
			ToastConfig toastConfig = new ToastConfig();
			toastConfig.Position = ToastPosition.Bottom;

			if (e.NetworkAccess != NetworkAccess.Internet)
			{
				toastConfig.Message = "Oops, looks like you don't have internet connection";
				toastConfig.BackgroundColor = Colors.Red;
				UserDialogs.Instance.ShowToast(toastConfig);
			}
			else
			{
				toastConfig.Message = "Your internet connection is back";
				toastConfig.BackgroundColor = Colors.Green;
				UserDialogs.Instance.ShowToast(toastConfig);
			}
		}
	}
}
