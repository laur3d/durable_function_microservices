using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using zeLaur.OrderService;


[assembly: FunctionsStartup(typeof(Startup))]
namespace zeLaur.OrderService
{


    public class Startup: FunctionsStartup
    {
        private IConfiguration configuration;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            this.configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            ServiceRegistrations.ConfigureServices(builder.Services, this.configuration);
        }

    }
}
