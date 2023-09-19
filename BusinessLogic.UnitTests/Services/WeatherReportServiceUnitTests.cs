using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Models.Entities;
using BusinessLogic.Models.Enums;
using BusinessLogic.Services;

namespace BusinessLogic.UnitTests.Services
{
    public class WeatherReportServiceUnitTests
    {
        [Fact]
        public async Task CheckIfDataExistsForCityAsync_CityNameIsNull_ShouldReturnFalse()
        {
            var unitUnderTest = new WeatherReportService(null, null);

            var result = await unitUnderTest.CheckIfDataExistsForCityAsync(null);

            Assert.False(result);
        }

        [Fact]
        public async Task CheckIfDataExistsForCityAsync_CityNameIsEmpty_ShouldReturnFalse()
        {
            var unitUnderTest = new WeatherReportService(null, null);

            var result = await unitUnderTest.CheckIfDataExistsForCityAsync("");

            Assert.False(result);
        }

        [Fact]
        public async Task CheckIfDataExistsForCityAsync_CityNameIsSpace_ShouldReturnFalse()
        {
            var unitUnderTest = new WeatherReportService(null, null);

            var result = await unitUnderTest.CheckIfDataExistsForCityAsync(" ");

            Assert.False(result);
        }

        [Fact]
        public async Task CheckIfDataExistsForCityAsync_UnknownCity_ShouldReturnFalse()
        {
            var cityRepository = new Mock<ICityRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(false));

            var unitUnderTest = new WeatherReportService(cityRepository.Object, null);

            var result = await unitUnderTest.CheckIfDataExistsForCityAsync("Singapore");

            Assert.False(result);
        }

        [Fact]
        public async Task CheckIfDataExistsForCityAsync_KnownCity_ShouldReturnTrue()
        {
            var cityRepository = new Mock<ICityRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));

            var unitUnderTest = new WeatherReportService(cityRepository.Object, null);

            var result = await unitUnderTest.CheckIfDataExistsForCityAsync("Oslo");

            Assert.True(result);
        }

        [Fact]
        public void AreDatesImproperlyOrdered_ProperOrder_ShouldReturnFalse()
        {
            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);

            var result = WeatherReportService.AreDatesImproperlyOrdered(fromDate, toDate);

            Assert.False(result);
        }

        [Fact]
        public void AreDatesImproperlyOrdered_SameDate_ShouldReturnFalse()
        {
            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 19);

            var result = WeatherReportService.AreDatesImproperlyOrdered(fromDate, toDate);

            Assert.False(result);
        }

        [Fact]
        public void AreDatesImproperlyOrdered_ImproperOrder_ShouldReturnTrue()
        {
            var fromDate = new DateTime(2023, 9, 21);
            var toDate = new DateTime(2023, 9, 20);

            var result = WeatherReportService.AreDatesImproperlyOrdered(fromDate, toDate);

            Assert.True(result);
        }

        [Fact]
        public void ConvertTemperaturesInDailyWeatherReport_ReturnedReportShouldHaveNewUnit()
        {
            var dailyWeatherReport = new DailyWeatherReport
            {
                TemperatureMax = 1,
                TemperatureAverage = 1,
                TemperatureMin = 1,
                TemperatureUnit = TemperatureUnit.Celsius
            };

            var requestedTemperatureUnit = TemperatureUnit.Kelvin;

            var result = WeatherReportService.ConvertTemperaturesInDailyWeatherReport(dailyWeatherReport, TemperatureUnit.Kelvin);

            Assert.Equal(requestedTemperatureUnit, result.TemperatureUnit);
        }

        [Theory]
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Celsius, 1)]
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Kelvin, 274.15)]
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, 33.8)]
        [InlineData(1, TemperatureUnit.Celsius, (TemperatureUnit)(-1), 1)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Celsius, -272.15)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Kelvin, 1)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Fahrenheit, -457.87)]
        [InlineData(1, TemperatureUnit.Kelvin, (TemperatureUnit)(-1), 1)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, -17.22)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin, 255.92)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Fahrenheit, 1)]
        [InlineData(1, TemperatureUnit.Fahrenheit, (TemperatureUnit)(-1), 1)]
        public void ConvertTemperaturesInDailyWeatherReport_Theory(double temperature, TemperatureUnit originalTemperatureUnit, TemperatureUnit requestedTemperatureUnit, double expectedResult)
        {
            var dailyWeatherReport = new DailyWeatherReport
            {
                TemperatureMax = temperature,
                TemperatureAverage = temperature,
                TemperatureMin = temperature,
                TemperatureUnit = originalTemperatureUnit
            };

            var result = WeatherReportService.ConvertTemperaturesInDailyWeatherReport(dailyWeatherReport, requestedTemperatureUnit);

            Assert.Equal(expectedResult, result.TemperatureMax, 1);
            Assert.Equal(expectedResult, result.TemperatureAverage, 1);
            Assert.Equal(expectedResult, result.TemperatureMin, 1);
        }

        [Theory]
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Celsius, 1)]
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Kelvin, 274.15)]
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, 33.8)]
        [InlineData(1, TemperatureUnit.Celsius, (TemperatureUnit)(-1), 1)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Celsius, -272.15)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Kelvin, 1)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Fahrenheit, -457.87)]
        [InlineData(1, TemperatureUnit.Kelvin, (TemperatureUnit)(-1), 1)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, -17.22)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin, 255.92)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Fahrenheit, 1)]
        [InlineData(1, TemperatureUnit.Fahrenheit, (TemperatureUnit)(-1), 1)]
        public void ConvertTemperatureToAnotherUnit_Theory(double temperature, TemperatureUnit originalTemperatureUnit, TemperatureUnit requestedTemperatureUnit, double expectedResult)
        {
            var result = WeatherReportService.ConvertTemperatureToAnotherUnit(temperature, originalTemperatureUnit, requestedTemperatureUnit);

            Assert.Equal(expectedResult, result, 1);
        }
    }
}
