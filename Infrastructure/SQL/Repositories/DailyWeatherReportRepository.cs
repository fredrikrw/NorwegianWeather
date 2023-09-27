using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Models.Entities;
using Infrastructure.Models.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RepoDb;

namespace Infrastructure.SQL.Repositories
{
    public class DailyWeatherReportRepository : IDailyWeatherReportRepository
    {
        private readonly DateTime MinimumAllowedRequestDate = new DateTime(2023, 1, 1);

        private readonly IOptions<SqlConnectionOptions> options;

        public DailyWeatherReportRepository(IOptions<SqlConnectionOptions> options)
        {
            this.options = options;
        }

        public async Task<List<DailyWeatherReport>> GetDailyWeatherReportsAsync(string cityName, DateTime fromDate, DateTime toDate)
        {
            if (string.IsNullOrEmpty(cityName)) return new List<DailyWeatherReport>();

            if (fromDate < MinimumAllowedRequestDate) fromDate = MinimumAllowedRequestDate;
            if (toDate < MinimumAllowedRequestDate) toDate = MinimumAllowedRequestDate;

            using var connection = new SqlConnection(options.Value.ConnectionString);

            var dailyWeatherReports = await connection.QueryAsync<DailyWeatherReport>(dailyWeatherReport =>
                dailyWeatherReport.CityName == cityName &&
                dailyWeatherReport.Date >= fromDate &&
                dailyWeatherReport.Date <= toDate);

            return dailyWeatherReports.ToList();
        }

        public async Task InsertDailyWeatherReportsAsync(List<DailyWeatherReport> dailyWeatherReports)
        {
            if(dailyWeatherReports is null || dailyWeatherReports.Count == 0) return;

            using var connection = new SqlConnection(options.Value.ConnectionString);

            await connection.InsertAllAsync(dailyWeatherReports);
        }
    }
}
