using Foundation;
using Microsoft.Extensions.DependencyInjection;
using MpesaCross.Extension;
using Prism;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using UIKit;

namespace MpesaCross.iOS
{
    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterServices(services =>
            {
                // Initializing mpesaclient httpclient instance using Dependency Injection
                services.AddSingleton<NSUrlSessionHandler>(handler => new NSUrlSessionHandler());
                services.AddNativeHandlerMpesaService<NSUrlSessionHandler>();
            });
        }
    }
}