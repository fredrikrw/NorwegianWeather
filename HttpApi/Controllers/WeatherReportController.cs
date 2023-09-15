using BusinessLogic.Interfaces.Services;
using BusinessLogic.Models.DTOs.Outbound;
using BusinessLogic.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherReportController : ControllerBase
    {
        private readonly IWeatherReportService weatherReportService;

        public WeatherReportController(IWeatherReportService weatherReportService)
        {
            this.weatherReportService = weatherReportService;
        }

        [HttpGet(Name = "GetPeriodWeatherReport")]
        public async Task<PeriodWeatherReportDTO> GetPeriodWeatherReportAsync(string cityName, DateTime fromDate, DateTime toDate, TemperatureUnit temperatureUnit)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException($"cityName cannot be Null");
            }
            if (fromDate.Date > toDate.Date)
            {
                throw new ArgumentException($"fromDate cannot be after toDate");
            }
            if ((int)temperatureUnit < 0 || (int)temperatureUnit > 2)
            {
                throw new ArgumentException("temperatureUnit is out of range");
            }

            return await weatherReportService.BuildPeriodWeatherReportAsync(cityName, fromDate, toDate, temperatureUnit);
        }
    }
}