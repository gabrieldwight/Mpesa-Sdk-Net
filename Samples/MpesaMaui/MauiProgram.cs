using Controls.UserDialogs.Maui;
using Microsoft.Extensions.Logging;
using MpesaMaui.Navigation;
using MpesaMaui.ViewModels;
using MpesaMaui.Views;
using MpesaSdk.Extensions;
using System.Net;
#if ANDROID
using Xamarin.Android.Net;
#endif

namespace MpesaMaui
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseUserDialogs(() =>
				{
					//setup your default styles for dialogs
					AlertConfig.DefaultBackgroundColor = Colors.Green;
#if ANDROID
					AlertConfig.DefaultMessageFontFamily = "OpenSansRegular.ttf";
#else
					AlertConfig.DefaultMessageFontFamily = "OpenSansRegular";
#endif

					ToastConfig.DefaultCornerRadius = 15;
				})
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSansRegular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSansSemibold.ttf", "OpenSansSemibold");
				})
				.RegisterAppServices()
				.RegisterViewModels()
				.RegisterViews();

#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}

		public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
		{
			mauiAppBuilder.Services.AddSingleton<INavigationService, MauiNavigationService>();
			mauiAppBuilder.Services.AddSingleton<IUserDialogs>(UserDialogs.Instance);
#if ANDROID
			mauiAppBuilder.Services.AddSingleton<AndroidMessageHandler>(handler => new AndroidMessageHandler()
			{
				AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
			});
			mauiAppBuilder.Services.AddNativeHandlerMpesaService<AndroidMessageHandler>();
#elif WINDOWS
			mauiAppBuilder.Services.AddSingleton<WinHttpHandler>(handler => new WinHttpHandler()
            {
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
            });
            mauiAppBuilder.Services.AddNativeHandlerMpesaService<WinHttpHandler>();
#elif IOS || MACCATALYST
			mauiAppBuilder.Services.AddSingleton<NSUrlSessionHandler>(handler => new NSUrlSessionHandler());
            mauiAppBuilder.Services.AddNativeHandlerMpesaService<NSUrlSessionHandler>();
#endif

			return mauiAppBuilder;
		}

		public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
		{
			mauiAppBuilder.Services.AddTransient<MpesaPushStkViewModel>();
			mauiAppBuilder.Services.AddTransient<MpesaResultsViewModel>();

			return mauiAppBuilder;
		}

		public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
		{
			mauiAppBuilder.Services.AddTransient<MpesaPushStkPage>();
			mauiAppBuilder.Services.AddTransient<MpesaResultsPage>();

			return mauiAppBuilder;
		}

		public static void AddNativeHandlerMpesaService<THandler>(this IServiceCollection services) where THandler : HttpMessageHandler
		{
#if DEBUG
			services.AddMpesaService<THandler>(MpesaSdk.Enums.Environment.Sandbox);
#else
            services.AddMpesaService<THandler>(MpesaSdk.Enums.Environment.Live);
#endif
		}
	}
}
