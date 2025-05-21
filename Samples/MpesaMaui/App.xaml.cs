using MpesaMaui.Navigation;

namespace MpesaMaui
{
	public partial class App : Application
	{
		private readonly INavigationService _navigationService;

		public App(INavigationService navigationService)
		{
			_navigationService = navigationService;

			InitializeComponent();
		}

		protected override Window CreateWindow(IActivationState? activationState)
		{
			return new Window(new AppShell(_navigationService));
		}
	}
}
