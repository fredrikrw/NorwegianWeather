using BusinessLogic.Interfaces.Infrastructure.HttpClients;
using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ICityRepository cityRepository;
        private readonly IDailyWeatherReportRepository dailyWeatherReportRepository;
        private readonly IMetrologicalInstituteHttpClient metrologicalInstituteHttpClient;
        private readonly IMetrologicalInstituteService metrologicalInstituteService;

        public TestController(
            ICityRepository cityRepository, 
            IDailyWeatherReportRepository dailyWeatherReportRepository,
            IMetrologicalInstituteHttpClient metrologicalInstituteHttpClient,
            IMetrologicalInstituteService metrologicalInstituteService)
        {
            this.cityRepository = cityRepository;
            this.dailyWeatherReportRepository = dailyWeatherReportRepository;
            this.metrologicalInstituteHttpClient = metrologicalInstituteHttpClient;
            this.metrologicalInstituteService = metrologicalInstituteService;
        }
        

        [HttpGet("TestHttpClient")]
        public async Task<dynamic> TestHttpClient()
        {
            var city = (await cityRepository.GetAllAsync()).FirstOrDefault(city => city.Name == "Kristiansand");

            var locationForecastCompactDTO = await metrologicalInstituteHttpClient.GetCompactLocationForcastAsync(city.Latitude, city.Longitude);

            return new { cityName = city.Name, locationForecastCompactDTO };
        }

        [HttpGet("TestMetInstitute")]
        public async Task TestMetInstitute()
        {
            await metrologicalInstituteService.RetrieveDataAndBuildDailyWeatherReportForAllCities();
        }

        [HttpGet("Test")]
        public async Task<dynamic> Test()
        {
            var contains = await cityRepository.ContainsAsync("Kristiansand");
            var cities = await cityRepository.GetAllAsync();

            //await dailyWeatherReportRepository.InsertDailyWeatherReportsAsync(new List<DailyWeatherReport>
            //{
            //    new DailyWeatherReport
            //    {
            //        CityName = "Søgne",
            //        Date = DateTime.Now.Date,
            //        TemperatureMax = 20,
            //        TemperatureAverage = 20,
            //        TemperatureMin = 20,
            //        CloudCoverAverage = 20,
            //        Percipitation = 20,
            //        WindSpeedAverage = 20,
            //        TemperatureUnit = TemperatureUnit.Fahrenheit
            //    }
            //});

            var reports = await dailyWeatherReportRepository.GetDailyWeatherReportsAsync("Kristiansand", new DateTime(2023, 9, 24), new DateTime(2023, 9, 25));

            return new { contains, cities, reports };
        }
    }
}