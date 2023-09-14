using BusinessLogic.Interfaces.Services;
using BusinessLogic.Models.DTOs.Outbound;
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
        public async Task<PeriodWeatherReportDTO> GetPeriodWeatherReportAsync(string cityName, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrEmpty(cityName))
            {
                throw new ArgumentException($"cityName cannot be Null");
            }
            if (fromDate.Date > toDate.Date)
            {
                throw new ArgumentException($"fromDate cannot be after toDate");
            }

            return await weatherReportService.BuildPeriodWeatherReportAsync(cityName, fromDate, toDate);
        }
    }
}