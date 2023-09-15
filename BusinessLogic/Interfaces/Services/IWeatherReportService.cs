using BusinessLogic.Models.DTOs.Outbound;
using BusinessLogic.Models.Enums;

namespace BusinessLogic.Interfaces.Services
{
    public interface IWeatherReportService
    {
        Task<PeriodWeatherReportDTO> BuildPeriodWeatherReportAsync(string cityName, DateTime fromDate, DateTime toDate, TemperatureUnit temperatureUnit);
    }
}
