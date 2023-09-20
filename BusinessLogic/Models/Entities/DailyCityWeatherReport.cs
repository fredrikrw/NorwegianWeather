using BusinessLogic.Models.Enums;

namespace BusinessLogic.Models.Entities
{
    public class DailyWeatherReport
    {
        public string CityName { get; set; }
        public DateTime Date { get; set; }
        public double TemperatureMax { get; set; }
        public double TemperatureAverage { get; set; }
        public double TemperatureMin { get; set; }
        public double CloudCoverAverage { get; set; }
        public double Percipitation { get; set; }
        public double WindSpeedAverage { get; set; }
        public TemperatureUnit TemperatureUnit { get; set; }
    }
}
