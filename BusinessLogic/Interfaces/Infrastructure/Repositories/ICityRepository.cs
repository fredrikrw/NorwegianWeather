using BusinessLogic.Models.Entities;

namespace BusinessLogic.Interfaces.Infrastructure.Repositories
{
    public interface ICityRepository
    {
        public Task<bool> Contains(string cityName);
        public Task<IEnumerable<City>> GetAllAsync();
    }
}
