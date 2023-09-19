using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Interfaces.Services;
using BusinessLogic.Models.DTOs.Outbound;
using BusinessLogic.Models.Enums;
using System.Data;

namespace BusinessLogic.Services
{
    public class WeatherReportService : IWeatherReportService
    {
        private readonly ICityRepository cityRepository;
        private readonly IDailyCityWeatherReportRepository dailyCityWeatherReportRepository;

        public WeatherReportService(ICityRepository cityRepository, IDailyCityWeatherReportRepository dailyCityWeatherReportRepository)
        {
            this.cityRepository = cityRepository;
            this.dailyCityWeatherReportRepository = dailyCityWeatherReportRepository;
        }

        public async Task<PeriodWeatherReportDTO> BuildPeriodWeatherReportAsync(string cityName, DateTime fromDate, DateTime toDate, TemperatureUnit requestedTemperatureUnit)
        {
            var cityNameIsValid = await cityRepository.Contains(cityName);
            if (cityNameIsValid)
            {
                var dailyReports = await dailyCityWeatherReportRepository.GetDailyCityWeatherReportsAsync(cityName, fromDate, toDate);

                var temperatureMax = dailyReports.Max(dailyReport => dailyReport.TemperatureMax);
                var temperatureAverage = dailyReports.Average(dailyReport => dailyReport.TemperatureAverage);
                var temperatureMin = dailyReports.Min(dailyReport => dailyReport.TemperatureMin);
                var cloudCoverAverage = dailyReports.Average(dailyReport => dailyReport.CloudCoverAverage);
                var numberOfDaysWithPercipitation = dailyReports.Where(dailyReport => dailyReport.Percipitation > 0).Count();
                var percipitationAverage = dailyReports.Average(dailyReport => dailyReport.Percipitation);
                var windSpeedAverage = dailyReports.Average(dailyReport => dailyReport.WindSpeedAverage);
                var temperatureUnit = dailyReports.First().TemperatureUnit;

                var weatherSummary = WeatherSummary.Fair;

                if (numberOfDaysWithPercipitation >= (dailyReports.Count() / 2) || cloudCoverAverage > 75 || windSpeedAverage >= 10)
                {
                    weatherSummary = WeatherSummary.Bad;
                }else if (numberOfDaysWithPercipitation < (dailyReports.Count() / 4) && cloudCoverAverage < 25 &&  windSpeedAverage < 10)
                {
                    weatherSummary = WeatherSummary.Great;
                }

                return new PeriodWeatherReportDTO
                {
                    TemperatureMax = ConvertTemperatureToAnotherUnit(temperatureMax, temperatureUnit, requestedTemperatureUnit),
                    TemperatureAverage = ConvertTemperatureToAnotherUnit(temperatureAverage, temperatureUnit, requestedTemperatureUnit),
                    TemperatureMin = ConvertTemperatureToAnotherUnit(temperatureMin, temperatureUnit, requestedTemperatureUnit),
                    CloudCoverAverage = cloudCoverAverage,
                    NumberOfDaysWithPercipitation = numberOfDaysWithPercipitation,
                    PercipitationAverage = percipitationAverage,
                    WindSpeedAverage = windSpeedAverage,
                    WeatherSummary = weatherSummary,
                };
            }
            else
            {
                throw new ArgumentException($"This service does not have any data for cityName [{cityName}]");
            }
        }

        private static double ConvertTemperatureToAnotherUnit(double temperature, TemperatureUnit fromUnit, TemperatureUnit toUnit)
        {
            return fromUnit switch
            {
                TemperatureUnit.Fahrenheit => toUnit switch
                {
                    TemperatureUnit.Kelvin => (temperature * (5d / 9d)) - 241.15d,
                    TemperatureUnit.Celsius => (temperature * (5d / 9d)) - 32d,
                    _ => temperature,
                },
                TemperatureUnit.Kelvin => toUnit switch
                {
                    TemperatureUnit.Fahrenheit => (temperature * (9d / 5d)) - 241.15d,
                    TemperatureUnit.Celsius => temperature - 273.15d,
                    _ => temperature,
                },
                _ => toUnit switch
                {
                    TemperatureUnit.Kelvin => temperature + 273.15d,
                    TemperatureUnit.Fahrenheit => (temperature * (9d / 5d)) + 32d,
                    _ => temperature,
                },
            };
        }
    }
}
