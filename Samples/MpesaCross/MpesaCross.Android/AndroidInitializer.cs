using Microsoft.Extensions.DependencyInjection;
using MpesaCross.Extension;
using Prism;
using Prism.Ioc;
using System.Net;
using Xamarin.Android.Net;

namespace MpesaCross.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterServices(services =>
            {
                // Initializing mpesaclient httpclient instance using Dependency Injection
                services.AddSingleton<AndroidClientHandler>(handler => new AndroidClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                });
                services.AddNativeHandlerMpesaService<AndroidClientHandler>();
            });
        }
    }
}