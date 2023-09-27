using BusinessLogic.Models.Entities;

namespace BusinessLogic.Interfaces.Infrastructure.Repositories
{
    public interface ICityRepository
    {
        public Task<bool> ContainsAsync(string cityName);
        public Task<List<City>> GetAllAsync();
    }
}
