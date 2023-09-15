namespace BusinessLogic.Models.DTOs.Inbound
{
    public class LocationForecastCompactDTO
    {
        public LocationForecastProperties Properties { get; set; }
    }

    public class LocationForecastProperties
    {
        public LocationForecastMetaData Meta { get; set; }
        public IEnumerable<LocationForecastTimeSeriesEntry> TimeSeries { get; set; }
    }

    public class LocationForecastMetaData
    {
        public LocationForecastUnits Units { get; set; }
    }

    public class LocationForecastUnits
    {
        public string Air_temperature { get; set; }
    }

    public class LocationForecastTimeSeriesEntry
    {
        public DateTime? Time { get; set; }
        public LocationForecastTimeSeriesEntryData Data { get; set; }
    }

    public class LocationForecastTimeSeriesEntryData
    {
        public LocationForecastInstant Instant { get; set; }
        public LocationForecastNext1Hours Next_1_hours { get; set; }
    }

    public class LocationForecastInstant
    {
        public LocationForecastInstantDetails Details { get; set; }
    }

    public class LocationForecastInstantDetails
    {
        public double Air_temperature { get; set; }
        public double Cloud_area_fraction { get; set; }
        public double Wind_speed { get; set; }
    }

    public class LocationForecastNext1Hours
    {
        public LocationForecastNext1HoursDetails Details { get; set; }
    }

    public class LocationForecastNext1HoursDetails
    {
        public double Precipitation_amount { get; set; }
    }
}
