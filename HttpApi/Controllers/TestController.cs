using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Models.Entities;
using BusinessLogic.Models.Enums;
using Microsoft.AspNetCore.Mvc;



namespace HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ICityRepository cityRepository;
        private readonly IDailyWeatherReportRepository dailyWeatherReportRepository;

        public TestController(ICityRepository cityRepository, IDailyWeatherReportRepository dailyWeatherReportRepository)
        {
            this.cityRepository = cityRepository;
            this.dailyWeatherReportRepository = dailyWeatherReportRepository;
        }

        [HttpGet(Name = "Test")]
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