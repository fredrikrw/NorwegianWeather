using BusinessLogic.Models.DTOs.Inbound;

namespace BusinessLogic.Interfaces.Infrastructure
{
    public interface IMetrologicalInstituteHttpClient
    {
        public Task<LocationForecastCompactDTO> GetCompactLocationForcastAsync(double latitude, double longitude);
    }
}
