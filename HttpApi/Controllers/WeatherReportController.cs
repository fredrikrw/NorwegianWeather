using BusinessLogic.Interfaces.Services;
using BusinessLogic.Models.DTOs.Outbound;
using BusinessLogic.Models.Enums;
using HttpApi.Interfaces.Validators;
using HttpApi.Validators;
using Microsoft.AspNetCore.Mvc;

namespace HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherReportController : ControllerBase
    {
        private readonly IWeatherReportService weatherReportService;
        private readonly IWeatherReportControllerParameterValidator weatherReportControllerParameterValidator;

        public WeatherReportController(IWeatherReportService weatherReportService, IWeatherReportControllerParameterValidator weatherReportControllerParameterValidator)
        {
            this.weatherReportService = weatherReportService;
            this.weatherReportControllerParameterValidator = weatherReportControllerParameterValidator;
        }

        [HttpGet(Name = "GetPeriodWeatherReport")]
        public async Task<PeriodWeatherReportDTO> GetPeriodWeatherReportAsync(string cityName, DateTime fromDate, DateTime toDate, TemperatureUnit temperatureUnit)
        {
            if (weatherReportControllerParameterValidator.IsCityNameInvalid(cityName)) throw new ArgumentException($"cityName is invalid");

            if (weatherReportControllerParameterValidator.IsDateOrderInvalid(fromDate, toDate)) throw new ArgumentException($"order of fromDate and toDate is invalid");

            if (weatherReportControllerParameterValidator.IsTemperatureUnitOutOfRange(temperatureUnit)) throw new ArgumentException($"temperatureUnit is out of range");

            return await weatherReportService.BuildPeriodWeatherReportAsync(cityName, fromDate, toDate, temperatureUnit);
        }
    }
}