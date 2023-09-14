using BusinessLogic.Models.Entities;

namespace BusinessLogic.Interfaces.Repositories
{
    public interface ICityRepository
    {
        public Task<bool> Contains(string cityName);
        public Task<IEnumerable<City>> GetAllAsync();
    }
}
