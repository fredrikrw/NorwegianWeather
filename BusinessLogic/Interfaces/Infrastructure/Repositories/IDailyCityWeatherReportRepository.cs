using BusinessLogic.Models.Entities;

namespace BusinessLogic.Interfaces.Infrastructure.Repositories
{
    public interface IDailyCityWeatherReportRepository
    {
        public Task<IEnumerable<DailyCityWeatherReport>> GetDailyCityWeatherReportsAsync(string cityName, DateTime fromDate, DateTime toDate);

        public Task InsertDailyCityWeatherReportsAsync(IEnumerable<DailyCityWeatherReport> dailyCityWeatherReports);
    }
}
