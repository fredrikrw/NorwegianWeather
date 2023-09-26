using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Models.Entities;
using Infrastructure.Models.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RepoDb;

namespace Infrastructure.Repositories
{
    public class DailyWeatherReportRepository : IDailyWeatherReportRepository
    {
        private readonly IOptions<SqlClientOptions> options;

        public DailyWeatherReportRepository(IOptions<SqlClientOptions> options)
        {
            this.options = options;
        }

        public async Task<List<DailyWeatherReport>> GetDailyWeatherReportsAsync(string cityName, DateTime fromDate, DateTime toDate)
        {
            using var connection = new SqlConnection(options.Value.ConnectionString);

            var dailyWeatherReports = await connection.QueryAsync<DailyWeatherReport>(dailyWeatherReport =>
                dailyWeatherReport.CityName == cityName &&
                dailyWeatherReport.Date >= fromDate &&
                dailyWeatherReport.Date <= toDate);

            return dailyWeatherReports.ToList();
        }

        public async Task InsertDailyWeatherReportsAsync(List<DailyWeatherReport> dailyWeatherReports)
        {
            using var connection = new SqlConnection(options.Value.ConnectionString);

            await connection.InsertAllAsync(dailyWeatherReports);
        }
    }
}
