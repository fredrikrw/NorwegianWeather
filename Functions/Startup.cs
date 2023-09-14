using BusinessLogic.Interfaces.Services;
using BusinessLogic.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Functions.Startup))]

namespace Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddScoped<IMetrologicalInstituteService, MetrologicalInstituteService>();
        }
    }
}