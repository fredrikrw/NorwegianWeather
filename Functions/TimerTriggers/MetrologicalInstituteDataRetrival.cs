using BusinessLogic.Interfaces.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Functions.TimerTriggers
{
    public class MetrologicalInstituteDataRetrival
    {
        private readonly IMetrologicalInstituteService metrologicalInstituteService;

        public MetrologicalInstituteDataRetrival(IMetrologicalInstituteService metrologicalInstituteService)
        {
            this.metrologicalInstituteService = metrologicalInstituteService;
        }

        [FunctionName("NightlyMetrologicalInstituteDataRetrivalTimmerTrigger")]
        public async Task NightlyMetrologicalInstituteDataRetrivalTimmerTrigger([TimerTrigger("0 0 5 * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            await metrologicalInstituteService.RetrieveDataAndBuildDailyWeatherReportForAllCities();
        }
    }
}
