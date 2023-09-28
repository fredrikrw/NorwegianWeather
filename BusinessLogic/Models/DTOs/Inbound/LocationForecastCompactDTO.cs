using System.Text.Json.Serialization;

namespace BusinessLogic.Models.DTOs.Inbound
{
    public class LocationForecastCompactDTO
    {
        [JsonPropertyName("properties")]
        public LocationForecastProperties Properties { get; set; }
    }

    public class LocationForecastProperties
    {
        [JsonPropertyName("meta")]
        public LocationForecastMetaData Meta { get; set; }

        [JsonPropertyName("timeseries")]
        public List<LocationForecastTimeSeriesEntry> TimeSeries { get; set; }
    }

    public class LocationForecastMetaData
    {
        [JsonPropertyName("units")]
        public LocationForecastUnits Units { get; set; }
    }

    public class LocationForecastUnits
    {
        [JsonPropertyName("air_temperature")]
        public string AirTemperature { get; set; }
    }

    public class LocationForecastTimeSeriesEntry
    {
        [JsonPropertyName("time")]
        public DateTime? Time { get; set; }

        [JsonPropertyName("data")]
        public LocationForecastTimeSeriesEntryData Data { get; set; }
    }

    public class LocationForecastTimeSeriesEntryData
    {
        [JsonPropertyName("instant")]
        public LocationForecastInstant Instant { get; set; }

        [JsonPropertyName("next_1_hours")]
        public LocationForecastNext1Hours NextHour { get; set; }
    }

    public class LocationForecastInstant
    {
        [JsonPropertyName("details")]
        public LocationForecastInstantDetails Details { get; set; }
    }

    public class LocationForecastInstantDetails
    {
        [JsonPropertyName("air_temperature")]
        public double AirTemperature { get; set; }

        [JsonPropertyName("cloud_area_fraction")]
        public double CloudAreaFraction { get; set; }

        [JsonPropertyName("wind_speed")]
        public double WindSpeed { get; set; }
    }

    public class LocationForecastNext1Hours
    {
        [JsonPropertyName("details")]
        public LocationForecastNext1HoursDetails Details { get; set; }
    }

    public class LocationForecastNext1HoursDetails
    {
        [JsonPropertyName("precipitation_amount")]
        public double PrecipitationAmount { get; set; }
    }
}
