using BusinessLogic.Models.Entities;

namespace BusinessLogic.Interfaces.Infrastructure.Repositories
{
    public interface IDailyWeatherReportRepository
    {
        public Task<List<DailyWeatherReport>> GetDailyWeatherReportsAsync(string cityName, DateTime fromDate, DateTime toDate);

        public Task InsertDailyWeatherReportsAsync(List<DailyWeatherReport> dailyWeatherReports);
    }
}
