using BusinessLogic.Models.DTOs.Inbound;

namespace BusinessLogic.Interfaces.Infrastructure.HttpClients
{
    public interface IMetrologicalInstituteHttpClient
    {
        public Task<LocationForecastCompactDTO> GetCompactLocationForcastAsync(double latitude, double longitude);
    }
}
