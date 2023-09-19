using BusinessLogic.Interfaces.Services;
using BusinessLogic.Models.DTOs.Outbound;
using BusinessLogic.Models.Enums;
using HttpApi.Validators;
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
            if (WeatherReportControllerParameterValidator.IsCityNameInvalid(cityName)) throw new ArgumentException($"cityName is invalid");

            if (WeatherReportControllerParameterValidator.IsDateOrderInvalid(fromDate, toDate)) throw new ArgumentException($"order of fromDate and toDate is invalid");

            if (WeatherReportControllerParameterValidator.IsTemperatureUnitOutOfRange(temperatureUnit)) throw new ArgumentException($"temperatureUnit is out of range");

            return await weatherReportService.BuildPeriodWeatherReportAsync(cityName, fromDate, toDate, temperatureUnit);
        }
    }
}