using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Interfaces.Services;
using BusinessLogic.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Models.Options;
using Microsoft.Extensions.Configuration;
using RepoDb;
using Infrastructure.SQL.Repositories;
using BusinessLogic.Interfaces.Infrastructure.HttpClients;
using Infrastructure.HttpClients;

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
            builder.Services.AddScoped<IMetrologicalInstituteHttpClient, MetrologicalInstituteHttpClient>();
            
            builder.Services.AddOptions<SqlConnectionOptions>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("SqlConnectionOptions").Bind(settings);
            });
            builder.Services.AddOptions<MetrologicalInstituteHttpClientOptions>().Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("MetrologicalInstituteHttpClientOptions").Bind(settings);
            });
        }
    }
}