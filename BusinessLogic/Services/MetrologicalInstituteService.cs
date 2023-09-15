using BusinessLogic.Interfaces.Infrastructure;
using BusinessLogic.Interfaces.Repositories;
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
        private readonly IDailyCityWeatherReportRepository dailyCityWeatherReportRepository;

        public MetrologicalInstituteService(
            ICityRepository cityRepository, 
            IMetrologicalInstituteHttpClient metrologicalInstituteHttpClient, 
            IDailyCityWeatherReportRepository dailyCityWeatherReportRepository)
        {
            this.cityRepository = cityRepository;
            this.metrologicalInstituteHttpClient = metrologicalInstituteHttpClient;
            this.dailyCityWeatherReportRepository = dailyCityWeatherReportRepository;
        }

        public async Task RetrieveDataAndBuildDailyReportForTommorrowForAllCities()
        {
            var cities = await cityRepository.GetAllAsync();

            var httpRequestTasks = cities.Select(RetrieveDataAndBuildDailyReportForTommorrow);

            await Task.WhenAll(httpRequestTasks);

            var dailyCityWeatherReports = httpRequestTasks.Select(httpRequestTask => httpRequestTask.Result);

            await dailyCityWeatherReportRepository.InsertDailyCityWeatherReportsAsync(dailyCityWeatherReports);
        }

        private async Task<DailyCityWeatherReport> RetrieveDataAndBuildDailyReportForTommorrow(City city)
        {
            var locationForecast = await metrologicalInstituteHttpClient.GetCompactLocationForcastAsync(city.Latitude, city.Longitude);

            var tommorrow = DateTime.Now.AddDays(1).Date;

            var timeSeriesForTommorrow = locationForecast.Properties.TimeSeries.Where(timeSeries => timeSeries.Time.Value.Date == tommorrow).ToList();

            var temperatureUnit = ConvertMetrologicalInstituteTemperatureUnitToTemperatureUnitEnum(locationForecast.Properties.Meta.Units.Air_temperature);

            return BuildDailyCityWeatherReports(timeSeriesForTommorrow, city.Name, temperatureUnit);
        }

        private static DailyCityWeatherReport BuildDailyCityWeatherReports(List<LocationForecastTimeSeriesEntry> timeSeries, string cityName, TemperatureUnit temperatureUnit)
        {
            var city = cityName;
            var date = timeSeries.First().Time.Value.Date;
            var temperatureMax = timeSeries.Max(entry => entry.Data.Instant.Details.Air_temperature);
            var temperatureAverage = timeSeries.Average(entry => entry.Data.Instant.Details.Air_temperature);
            var temperatureMin = timeSeries.Min(entry => entry.Data.Instant.Details.Air_temperature);
            var cloudCoverAverage = timeSeries.Average(entry => entry.Data.Instant.Details.Cloud_area_fraction);
            var percipitation = timeSeries.Sum(entry => entry.Data.Next_1_hours.Details.Precipitation_amount);
            var windSpeedAverage = timeSeries.Average(entry => entry.Data.Instant.Details.Wind_speed);

            return new DailyCityWeatherReport
            {
                City = city,
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

        private static TemperatureUnit ConvertMetrologicalInstituteTemperatureUnitToTemperatureUnitEnum(string temperatureUnit)
        {
            return temperatureUnit.ToUpper() switch
            {
                "K" => TemperatureUnit.Kelvin,
                "F" => TemperatureUnit.Fahrenheit,
                _ => TemperatureUnit.Celsius,
            };
        }
    }
}
