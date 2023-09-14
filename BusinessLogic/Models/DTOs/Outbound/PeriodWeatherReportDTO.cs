using BusinessLogic.Models.Enums;

namespace BusinessLogic.Models.DTOs.Outbound
{
    public class PeriodWeatherReportDTO
    {
        public double TemperatureMax { get; set; }
        public double TemperatureAverage { get; set; }
        public double TemperatureMin { get; set; }
        public double CloudCoverAverage { get; set; }
        public int NumberOfDaysWithPercipitation { get; set; }
        public double PercipitationAverage { get; set; }
        public double WindSpeedAverage { get; set; }
        public WeatherSummary WeatherSummary { get; set; }
    }
}
