using BusinessLogic.Interfaces.Infrastructure.HttpClients;
using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Interfaces.Services;
using BusinessLogic.Models.DTOs.Inbound;
using BusinessLogic.Models.Entities;
using BusinessLogic.Models.Enums;

namespace BusinessLogic.Services
{
    public class MetrologicalInstituteService : IMetrologicalInstituteService
    {
        private readonly ICityRepository cityRepository;
        private readonly IMetrologicalInstituteHttpClient metrologicalInstituteHttpClient;
        private readonly IDailyWeatherReportRepository dailyWeatherReportRepository;

        public MetrologicalInstituteService(
            ICityRepository cityRepository,
            IMetrologicalInstituteHttpClient metrologicalInstituteHttpClient,
            IDailyWeatherReportRepository dailyWeatherReportRepository)
        {
            this.cityRepository = cityRepository;
            this.metrologicalInstituteHttpClient = metrologicalInstituteHttpClient;
            this.dailyWeatherReportRepository = dailyWeatherReportRepository;
        }

        public async Task RetrieveDataAndBuildDailyWeatherReportForAllCities()
        {
            var cities = await RetrieveCitiesAsync();

            var dailyWeatherReports = await BuildDailyWeahterReportForCitiesAsync(cities);

            await StoreDailyWeatherReportsAsync(dailyWeatherReports);
        }

        public async Task<List<City>> RetrieveCitiesAsync()
        {
            return await cityRepository.GetAllAsync();
        }

        public async Task<List<DailyWeatherReport>> BuildDailyWeahterReportForCitiesAsync(List<City> cities)
        {
            var individualCityTasks = cities.Select(city => RetrieveDataAndBuildDailyWeahterReportForCityAsync(city));

            await Task.WhenAll(individualCityTasks);

            return individualCityTasks.Select(individualCityTask => individualCityTask.Result).ToList();
        }

        public async Task StoreDailyWeatherReportsAsync(List<DailyWeatherReport> dailyWeatherReports)
        {
            await dailyWeatherReportRepository.InsertDailyWeatherReportsAsync(dailyWeatherReports);
        }

        public async Task<DailyWeatherReport> RetrieveDataAndBuildDailyWeahterReportForCityAsync(City city)
        {
            var locationForecast = await RetrieveLocationForecastForCityAsync(city);

            var timeSeriesForTommorrow = GetTommorrowsForecastTimeSeries(locationForecast);

            var temperatureUnit = ConvertMetrologicalInstituteTemperatureUnitToEnum(locationForecast.Properties?.Meta?.Units?.AirTemperature);

            return BuildDailyWeahterReportForCityAsync(timeSeriesForTommorrow, city.Name, temperatureUnit);
        }

        public async Task<LocationForecastCompactDTO> RetrieveLocationForecastForCityAsync(City city)
        {
            return await metrologicalInstituteHttpClient.GetCompactLocationForcastAsync(city.Latitude, city.Longitude);
        }

        public static List<LocationForecastTimeSeriesEntry> GetTommorrowsForecastTimeSeries(LocationForecastCompactDTO locationForecast)
        {
            var tommorrow = DateTime.Now.AddDays(1).Date;

            return locationForecast?.Properties?.TimeSeries?.Where(timeSeries => timeSeries.Time.HasValue && timeSeries.Time.Value.Date == tommorrow).ToList() ?? new List<LocationForecastTimeSeriesEntry>();
        }

        public static TemperatureUnit ConvertMetrologicalInstituteTemperatureUnitToEnum(string temperatureUnit)
        {
            if (string.IsNullOrEmpty(temperatureUnit)) return TemperatureUnit.Celsius;

            return temperatureUnit.ToUpper() switch
            {
                "K" => TemperatureUnit.Kelvin,
                "KELVIN" => TemperatureUnit.Kelvin,
                "F" => TemperatureUnit.Fahrenheit,
                "FAHRENHEIT" => TemperatureUnit.Fahrenheit,
                _ => TemperatureUnit.Celsius,
            };
        }

        public static DailyWeatherReport BuildDailyWeahterReportForCityAsync(List<LocationForecastTimeSeriesEntry> timeSeries, string cityName, TemperatureUnit temperatureUnit)
        {
            var date = timeSeries.First(timeSeriesEntry => timeSeriesEntry.Time.HasValue).Time.Value.Date;
            var temperatureMax = timeSeries.Max(entry => entry.Data.Instant.Details.AirTemperature);
            var temperatureAverage = timeSeries.Average(entry => entry.Data.Instant.Details.AirTemperature);
            var temperatureMin = timeSeries.Min(entry => entry.Data.Instant.Details.AirTemperature);
            var cloudCoverAverage = timeSeries.Average(entry => entry.Data.Instant.Details.CloudAreaFraction);
            var percipitation = timeSeries.Sum(entry => entry.Data.NextHour.Details.PrecipitationAmount);
            var windSpeedAverage = timeSeries.Average(entry => entry.Data.Instant.Details.WindSpeed);

            return new DailyWeatherReport
            {
                CityName = cityName,
                Date = date,
                TemperatureMax = temperatureMax,
                TemperatureAverage = temperatureAverage,
                TemperatureMin = temperatureMin,
                CloudCoverAverage = cloudCoverAverage,
                Percipitation = percipitation,
                WindSpeedAverage = windSpeedAverage,
                TemperatureUnit = temperatureUnit,
            };
        }
    }
}
