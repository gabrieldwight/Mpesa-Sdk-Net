using MpesaMaui.Navigation;
using MpesaMaui.Views;

namespace MpesaMaui
{
	public partial class AppShell : Shell
	{
		private readonly INavigationService _navigationService;

		public AppShell(INavigationService navigationService)
		{
			_navigationService = navigationService;

			AppShell.InitializeRouting();
			InitializeComponent();
		}

		protected override async void OnHandlerChanged()
		{
			base.OnHandlerChanged();

			if (Handler is not null)
			{
				await _navigationService.NavigateToAsync("//MpesaPushStkPage");
			}
		}

		private static void InitializeRouting()
		{
			Routing.RegisterRoute(nameof(MpesaResultsPage), typeof(MpesaResultsPage));
		}
	}
}
