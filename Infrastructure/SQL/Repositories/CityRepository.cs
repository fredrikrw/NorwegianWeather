using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Models.Entities;
using Infrastructure.Models.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using RepoDb;

namespace Infrastructure.SQL.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IOptions<SqlConnectionOptions> options;

        public CityRepository(IOptions<SqlConnectionOptions> options)
        {
            this.options = options;
        }

        public async Task<bool> ContainsAsync(string cityName)
        {
            if (string.IsNullOrEmpty(cityName)) return false;

            using var connection = new SqlConnection(options.Value.ConnectionString);

            return await connection.ExistsAsync<City>(cityName);
        }

        public async Task<List<City>> GetAllAsync()
        {
            using var connection = new SqlConnection(options.Value.ConnectionString);

            var cities = await connection.QueryAllAsync<City>();

            return cities.ToList();
        }
    }
}
