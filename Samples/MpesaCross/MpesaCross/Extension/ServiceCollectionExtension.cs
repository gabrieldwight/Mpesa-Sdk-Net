using Microsoft.Extensions.DependencyInjection;
using MpesaSdk.Extensions;
using System.Net.Http;

namespace MpesaCross.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddNativeHandlerMpesaService<THandler>(this IServiceCollection services) where THandler : HttpMessageHandler
        {
            services.AddMpesaService<THandler>();
        }
    }
}
