using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using zeLaur.OrderService.Clients;

namespace zeLaur.OrderService
{
    internal static class ServiceRegistrations
    {
        public static void ConfigureServices(IServiceCollection builderServices, IConfiguration configuration)
        {
            builderServices.AddFeature(configuration)
                .AddHttpClients(configuration);

        }

        private static IServiceCollection AddFeature(this IServiceCollection serviceCollection, IConfiguration config)
        {
            // here we would group all feature related DI code
            serviceCollection.TryAddSingleton<AppConfig>(sp => new AppConfig()
            {
                CartServiceUrl = config.GetValue<string>("CartServiceUrl"),
            });
            return serviceCollection;
        }

        private static IServiceCollection AddHttpClients(this IServiceCollection serviceCollection,
            IConfiguration config)
        {
            serviceCollection.AddHttpClient<IShoppingCartClient, ShoppingCartClient>((provider, client) =>
            {
                // add the required headers
                var config = provider.GetService<AppConfig>();

                client.BaseAddress = new Uri(config.CartServiceUrl);
            });

            return serviceCollection;
        }
    }
}
