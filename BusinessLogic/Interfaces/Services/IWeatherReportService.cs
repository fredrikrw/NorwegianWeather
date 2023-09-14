using BusinessLogic.Models.DTOs.Outbound;

namespace BusinessLogic.Interfaces.Services
{
    public interface IWeatherReportService
    {
        Task<PeriodWeatherReportDTO> BuildPeriodWeatherReportAsync(string cityName, DateTime fromDate, DateTime toDate);
    }
}
