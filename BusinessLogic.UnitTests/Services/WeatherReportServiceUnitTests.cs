using BusinessLogic.Interfaces.Infrastructure.Repositories;
using BusinessLogic.Models.Entities;
using BusinessLogic.Models.Enums;
using BusinessLogic.Services;

namespace BusinessLogic.UnitTests.Services
{
    public class WeatherReportServiceUnitTests
    {
        private readonly List<DailyWeatherReport> dailyWeatherReports = new List<DailyWeatherReport>
            {
                new DailyWeatherReport
                {
                    TemperatureMax = 14,
                    TemperatureAverage = 12,
                    TemperatureMin = 10,
                    CloudCoverAverage = 14,
                    Percipitation = 12,
                    WindSpeedAverage = 10,
                    TemperatureUnit = TemperatureUnit.Celsius
                },
                new DailyWeatherReport
                {
                    TemperatureMax = 8,
                    TemperatureAverage = 6,
                    TemperatureMin = 4,
                    CloudCoverAverage = 8,
                    Percipitation = 6,
                    WindSpeedAverage = 4,
                    TemperatureUnit = TemperatureUnit.Celsius
                },
                new DailyWeatherReport
                {
                    TemperatureMax = 10,
                    TemperatureAverage = 10,
                    TemperatureMin = 10,
                    CloudCoverAverage = 0,
                    Percipitation = 0,
                    WindSpeedAverage = 0,
                    TemperatureUnit = TemperatureUnit.Celsius
                },
                new DailyWeatherReport
                {
                    TemperatureMax = 10,
                    TemperatureAverage = 10,
                    TemperatureMin = 10,
                    CloudCoverAverage = 10,
                    Percipitation = 0,
                    WindSpeedAverage = 10,
                    TemperatureUnit = TemperatureUnit.Celsius
                },
            };

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
        public void ConvertTemperaturesInDailyWeatherReport_ReturnedReportShouldHaveNewTemperatureUnit()
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
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Kelvin, 274.2)]
        [InlineData(1, TemperatureUnit.Celsius, TemperatureUnit.Fahrenheit, 33.8)]
        [InlineData(1, TemperatureUnit.Celsius, (TemperatureUnit)(-1), 1)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Celsius, -272.2)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Kelvin, 1)]
        [InlineData(1, TemperatureUnit.Kelvin, TemperatureUnit.Fahrenheit, -457.9)]
        [InlineData(1, TemperatureUnit.Kelvin, (TemperatureUnit)(-1), 1)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Celsius, -17.2)]
        [InlineData(1, TemperatureUnit.Fahrenheit, TemperatureUnit.Kelvin, 255.9)]
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

        [Fact]
        public void BuildPeriodWeatherReport_TemperatureMax()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(14, result.TemperatureMax);
        }

        [Fact]
        public void BuildPeriodWeatherReport_TemperatureAverage()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(9.5, result.TemperatureAverage);
        }

        [Fact]
        public void BuildPeriodWeatherReport_TemperatureMin()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(4, result.TemperatureMin);
        }

        [Fact]
        public void BuildPeriodWeatherReport_CloudCoverAverage()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(8, result.CloudCoverAverage);
        }

        [Fact]
        public void BuildPeriodWeatherReport_TemperatureNumberOfDaysWithPercipitation()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(2, result.NumberOfDaysWithPercipitation);
        }

        [Fact]
        public void BuildPeriodWeatherReport_TemperaturePercipitationAverage()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(4.5, result.PercipitationAverage);
        }

        [Fact]
        public void BuildPeriodWeatherReport_TemperatureWindSpeedAverage()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(6, result.WindSpeedAverage);
        }

        [Fact]
        public void BuildPeriodWeatherReport_TemperatureWeatherSummary()
        {
            var result = WeatherReportService.BuildPeriodWeatherReport(dailyWeatherReports);

            Assert.Equal(WeatherSummary.Bad, result.WeatherSummary);
        }

        [Theory]
        [InlineData(8, 4, 0, 0, WeatherSummary.Bad)]
        [InlineData(8, 0, 76, 0, WeatherSummary.Bad)]
        [InlineData(8, 0, 0, 12, WeatherSummary.Bad)]
        [InlineData(8, 4, 76, 12, WeatherSummary.Bad)]

        [InlineData(8, 2, 75, 11, WeatherSummary.Fair)]
        [InlineData(8, 3, 0, 0, WeatherSummary.Fair)]
        [InlineData(8, 0, 75, 0, WeatherSummary.Fair)]
        [InlineData(8, 0, 0, 11, WeatherSummary.Fair)]
        [InlineData(8, 2, 0, 0, WeatherSummary.Fair)]
        [InlineData(8, 0, 25, 0, WeatherSummary.Fair)]
        [InlineData(8, 0, 0, 8, WeatherSummary.Fair)]
        [InlineData(8, 3, 25, 8, WeatherSummary.Fair)]

        [InlineData(8, 1, 24, 7, WeatherSummary.Great)]
        [InlineData(8, 1, 0, 0, WeatherSummary.Great)]
        [InlineData(8, 0, 24, 0, WeatherSummary.Great)]
        [InlineData(8, 0, 0, 7, WeatherSummary.Great)]
        [InlineData(8, 0, 0, 0, WeatherSummary.Great)]

        public void SummarizeWeather_Theory(int numberOfDays, int numberOfDaysWithPercipitation, double cloudCoverAverage, double windSpeedAverage, WeatherSummary expectedResult)
        {
            var result = WeatherReportService.SummarizeWeather(numberOfDays, numberOfDaysWithPercipitation, cloudCoverAverage, windSpeedAverage);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_CityDataFound_NoExceptionThrown()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Celsius;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit));

            Assert.Null(exception);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_CityDataNotFound_InvalidCityName_ExceptionThrown()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Celsius;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.BuildPeriodWeatherReportAsync("", fromDate, toDate, requestedTemperatureUnit));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_CityDataNotFound_NoDataInDb_ExceptionThrown()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(false));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Celsius;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_DatesAreNotOrderedProperly_ExceptionThrown()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 21);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Celsius;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit));

            Assert.NotNull(exception);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_DatesAreOrderedProperly_NoExceptionThrown()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Celsius;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit));

            Assert.Null(exception);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_TemperatureMax()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(57.2, result.TemperatureMax);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_TemperatureAverage()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(49.1, result.TemperatureAverage);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_TemperatureMin()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(39.2, result.TemperatureMin);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_CloudCoverAverage()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(8, result.CloudCoverAverage);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_NumberOfDaysWithPercipitation()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(2, result.NumberOfDaysWithPercipitation);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_PercipitationAverage()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(4.5, result.PercipitationAverage);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_WindSpeedAverage()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(6, result.WindSpeedAverage);
        }

        [Fact]
        public async Task BuildPeriodWeatherReportAsync_ReportValues_WeatherSummary()
        {
            var cityRepository = new Mock<ICityRepository>();
            var dailyWeatherReportRepository = new Mock<IDailyWeatherReportRepository>();

            cityRepository.Setup(cityRepository => cityRepository.Contains(It.IsAny<string>())).Returns(Task.FromResult(true));
            dailyWeatherReportRepository.Setup(dailyWeatherReportRepository => dailyWeatherReportRepository.GetDailyWeatherReportsAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(dailyWeatherReports));

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);
            var requestedTemperatureUnit = TemperatureUnit.Fahrenheit;

            var unitUnderTest = new WeatherReportService(cityRepository.Object, dailyWeatherReportRepository.Object);

            var result = await unitUnderTest.BuildPeriodWeatherReportAsync("oslo", fromDate, toDate, requestedTemperatureUnit);

            Assert.Equal(WeatherSummary.Bad, result.WeatherSummary);
        }
    }
}
