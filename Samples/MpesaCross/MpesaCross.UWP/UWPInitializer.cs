using Microsoft.Extensions.DependencyInjection;
using MpesaCross.Extension;
using Prism;
using Prism.Ioc;
using System.Net;
using System.Net.Http;

namespace MpesaCross.UWP
{
    public class UWPInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterServices(services =>
            {
                // Initializing mpesaclient httpclient instance using Dependency Injection
                services.AddSingleton<WinHttpHandler>(handler => new WinHttpHandler()
                {
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                });
                services.AddNativeHandlerMpesaService<WinHttpHandler>();
            });
        }
    }
}
