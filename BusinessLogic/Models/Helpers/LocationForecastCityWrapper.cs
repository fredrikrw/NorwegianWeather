using BusinessLogic.Models.DTOs.Inbound;

namespace BusinessLogic.Models.Helpers
{
    public class LocationForecastCityWrapper
    {
        public string CityName { get; set; }
        public LocationForecastCompactDTO LocationForecast { get; set; }
    }
}
