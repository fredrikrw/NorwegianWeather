using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Models.Entities;
using Infrastructure.Models.Options;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using RepoDb;

namespace Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly IOptions<SqlClientOptions> options;

        public CityRepository(IOptions<SqlClientOptions> options)
        {
            this.options = options;
        }

        public async Task<bool> Contains(string cityName)
        {
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
