using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Interfaces.Services;
using BusinessLogic.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories;
using Infrastructure.Models.Options;
using Microsoft.Extensions.Configuration;
using RepoDb;

[assembly: FunctionsStartup(typeof(Functions.Startup))]

namespace Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            GlobalConfiguration.Setup().UseSqlServer();

            builder.Services.AddScoped<IMetrologicalInstituteService, MetrologicalInstituteService>();
            builder.Services.AddScoped<ICityRepository, CityRepository>();
            builder.Services.AddScoped<IDailyWeatherReportRepository, DailyWeatherReportRepository>();

            builder.Services.AddOptions<SqlClientOptions>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("SqlClientOptions").Bind(settings);
            });
        }
    }
}