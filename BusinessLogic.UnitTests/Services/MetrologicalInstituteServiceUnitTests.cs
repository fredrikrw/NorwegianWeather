using BusinessLogic.Interfaces.Infrastructure.HttpClients;
using BusinessLogic.Models.DTOs.Inbound;
using BusinessLogic.Models.Entities;
using BusinessLogic.Models.Enums;
using BusinessLogic.Services;

namespace BusinessLogic.UnitTests.Services
{
    public class MetrologicalInstituteServiceUnitTests
    {
        private readonly List<LocationForecastTimeSeriesEntry> timeSeries = new List<LocationForecastTimeSeriesEntry>
            {
                new LocationForecastTimeSeriesEntry
                {
                    Time = DateTime.Now.AddDays(1),
                    Data = new LocationForecastTimeSeriesEntryData
                    {
                        Instant = new LocationForecastInstant
                        {
                            Details = new LocationForecastInstantDetails
                            {
                                Air_temperature = 20,
                                Cloud_area_fraction = 50,
                                Wind_speed = 10
                            }
},
                        Next_1_hours = new LocationForecastNext1Hours
                        {
                            Details = new LocationForecastNext1HoursDetails
                            {
                                Precipitation_amount = 30,
                            }
                        }
                    }
                },
                new LocationForecastTimeSeriesEntry
                {
                    Time = DateTime.Now.AddDays(1),
                    Data = new LocationForecastTimeSeriesEntryData
                    {
                        Instant = new LocationForecastInstant
                        {
                            Details = new LocationForecastInstantDetails
                            {
                                Air_temperature = 40,
                                Cloud_area_fraction = 100,
                                Wind_speed = 20
                            }
                        },
                        Next_1_hours = new LocationForecastNext1Hours
                        {
                            Details = new LocationForecastNext1HoursDetails
                            {
                                Precipitation_amount = 60,
                            }
                        }
                    }
                }
            };

        private readonly City city = new City
        {
            Name = "Oslo",
            Latitude = 10,
            Longitude = 10,
        };

        private readonly Mock<IMetrologicalInstituteHttpClient> metrologicalInstituteHttpClient = new Mock<IMetrologicalInstituteHttpClient>();

        public MetrologicalInstituteServiceUnitTests()
        {
            metrologicalInstituteHttpClient.Setup(metrologicalInstituteHttpClient => metrologicalInstituteHttpClient.GetCompactLocationForcastAsync(It.IsAny<double>(), It.IsAny<double>()))
                    .Returns(Task.FromResult(
                        new LocationForecastCompactDTO
                        {
                            Properties = new LocationForecastProperties
                            {
                                Meta = new LocationForecastMetaData
                                {
                                    Units = new LocationForecastUnits
                                    {
                                        AirTemperature = "f",
                                    }
                                },
                                TimeSeries = timeSeries
                            }
                        }
                ));
        }

        [Fact]
        public void GetTommorrowsForecastTimeSeries_NoMatchingDates_EmptyResult()
        {
            var locationForecast = new LocationForecastCompactDTO
            {
                Properties = new LocationForecastProperties
                {
                    TimeSeries = new List<LocationForecastTimeSeriesEntry> {
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(0)
                        },
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(2)
                        }
                    }
                }
            };
            var result = MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(locationForecast);

            Assert.Empty(result);
        }

        [Fact]
        public void GetTommorrowsForecastTimeSeries_MatchingDates_SingleResult()
        {
            var locationForecast = new LocationForecastCompactDTO
            {
                Properties = new LocationForecastProperties
                {
                    TimeSeries = new List<LocationForecastTimeSeriesEntry> {
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(0)
                        },
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(1)
                        },
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(2)
                        }
                    }
                }
            };
            var result = MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(locationForecast);

            Assert.Single(result);
        }

        [Fact]
        public void GetTommorrowsForecastTimeSeries_MatchingDates_ManyResult()
        {
            var locationForecast = new LocationForecastCompactDTO
            {
                Properties = new LocationForecastProperties
                {
                    TimeSeries = new List<LocationForecastTimeSeriesEntry> {
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(0)
                        },
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(1)
                        },
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(1)
                        },
                        new LocationForecastTimeSeriesEntry {
                            Time = DateTime.Now.AddDays(2)
                        }
                    }
                }
            };
            var result = MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(locationForecast);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void GetTommorrowsForecastTimeSeries_EmptyTimeSeries_DontThrowException()
        {
            var locationForecast = new LocationForecastCompactDTO
            {
                Properties = new LocationForecastProperties
                {
                    TimeSeries = new List<LocationForecastTimeSeriesEntry> { }
                }
            };

            var exception = Record.Exception(() => MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(locationForecast));

            Assert.Null(exception);
        }

        [Fact]
        public void GetTommorrowsForecastTimeSeries_EmptyProperties_DontThrowException()
        {
            var locationForecast = new LocationForecastCompactDTO
            {
                Properties = new LocationForecastProperties { }
            };

            var exception = Record.Exception(() => MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(locationForecast));

            Assert.Null(exception);
        }

        [Fact]
        public void GetTommorrowsForecastTimeSeries_EmptyLocationForecastCompactDTO_DontThrowException()
        {
            var locationForecast = new LocationForecastCompactDTO { };

            var exception = Record.Exception(() => MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(locationForecast));

            Assert.Null(exception);
        }


        [Fact]
        public void GetTommorrowsForecastTimeSeries_NullLocationForecastCompactDTO_DontThrowException()
        {
            var exception = Record.Exception(() => MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(null));

            Assert.Null(exception);
        }

        [Fact]
        public void GetTommorrowsForecastTimeSeries_NoTimeValue_DontThrowException()
        {
            var locationForecast = new LocationForecastCompactDTO
            {
                Properties = new LocationForecastProperties
                {
                    TimeSeries = new List<LocationForecastTimeSeriesEntry> {
                        new LocationForecastTimeSeriesEntry {  }
                    }
                }
            };

            var exception = Record.Exception(() => MetrologicalInstituteService.GetTommorrowsForecastTimeSeries(locationForecast));

            Assert.Null(exception);
        }

        [Theory]
        [InlineData("c", TemperatureUnit.Celsius)]
        [InlineData("C", TemperatureUnit.Celsius)]
        [InlineData("k", TemperatureUnit.Kelvin)]
        [InlineData("K", TemperatureUnit.Kelvin)]
        [InlineData("f", TemperatureUnit.Fahrenheit)]
        [InlineData("F", TemperatureUnit.Fahrenheit)]
        [InlineData("", TemperatureUnit.Celsius)]
        [InlineData(" ", TemperatureUnit.Celsius)]
        [InlineData(null, TemperatureUnit.Celsius)]
        public void ConvertMetrologicalInstituteTemperatureUnitToEnum(string temperatureUnit, TemperatureUnit expectedResult)
        {
            var result = MetrologicalInstituteService.ConvertMetrologicalInstituteTemperatureUnitToEnum(temperatureUnit);

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_TemperatureMax()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(40, result.TemperatureMax);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_TemperatureAverage()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(30, result.TemperatureAverage);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_TemperatureMin()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(20, result.TemperatureMin);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_City()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(cityName, result.CityName);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_Date()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(DateTime.Now.AddDays(1).Date, result.Date);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_NoDateFoundOnFirstTimeSeriesEntry_ShouldFindNextDate()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;
            var timeSeriesWithNoTimeOnTheFirstEntry = new List<LocationForecastTimeSeriesEntry>
            {
                new LocationForecastTimeSeriesEntry
                {
                    Data = new LocationForecastTimeSeriesEntryData
                    {
                        Instant = new LocationForecastInstant
                        {
                            Details = new LocationForecastInstantDetails
                            {
                                Air_temperature = 20,
                                Cloud_area_fraction = 50,
                                Wind_speed = 10
                            }
                        },
                        Next_1_hours = new LocationForecastNext1Hours
                        {
                            Details = new LocationForecastNext1HoursDetails
                            {
                                Precipitation_amount = 30,
                            }
                        }
                    }
                },
                new LocationForecastTimeSeriesEntry
                {
                    Time = DateTime.Now.AddDays(1),
                    Data = new LocationForecastTimeSeriesEntryData
                    {
                        Instant = new LocationForecastInstant
                        {
                            Details = new LocationForecastInstantDetails
                            {
                                Air_temperature = 40,
                                Cloud_area_fraction = 100,
                                Wind_speed = 20
                            }
                        },
                        Next_1_hours = new LocationForecastNext1Hours
                        {
                            Details = new LocationForecastNext1HoursDetails
                            {
                                Precipitation_amount = 60,
                            }
                        }
                    }
                }
            };

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeriesWithNoTimeOnTheFirstEntry, cityName, temperatureUnit);

            Assert.Equal(DateTime.Now.AddDays(1).Date, result.Date);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_NoDateFoundOnAnyTimeSeriesEntries_ShouldThrowException()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var timeSeriesWithNoTimeOnAnyEntry = new List<LocationForecastTimeSeriesEntry>
            {
                new LocationForecastTimeSeriesEntry
                {
                    Data = new LocationForecastTimeSeriesEntryData
                    {
                        Instant = new LocationForecastInstant
                        {
                            Details = new LocationForecastInstantDetails
                            {
                                Air_temperature = 20,
                                Cloud_area_fraction = 50,
                                Wind_speed = 10
                            }
},
                        Next_1_hours = new LocationForecastNext1Hours
                        {
                            Details = new LocationForecastNext1HoursDetails
                            {
                                Precipitation_amount = 30,
                            }
                        }
                    }
                },
                new LocationForecastTimeSeriesEntry
                {
                    Data = new LocationForecastTimeSeriesEntryData
                    {
                        Instant = new LocationForecastInstant
                        {
                            Details = new LocationForecastInstantDetails
                            {
                                Air_temperature = 40,
                                Cloud_area_fraction = 100,
                                Wind_speed = 20
                            }
                        },
                        Next_1_hours = new LocationForecastNext1Hours
                        {
                            Details = new LocationForecastNext1HoursDetails
                            {
                                Precipitation_amount = 60,
                            }
                        }
                    }
                }
            };

            var exception = Record.Exception(() => MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeriesWithNoTimeOnAnyEntry, cityName, temperatureUnit));

            Assert.NotNull(exception);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_CloudCoverAverage()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(75, result.CloudCoverAverage);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_Percipitation()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(90, result.Percipitation);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_WindSpeedAverage()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Celsius;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(15, result.WindSpeedAverage);
        }

        [Fact]
        public void BuildDailyWeahterReportForCityAsync_TemperatureUnit()
        {
            var cityName = "oslo";
            var temperatureUnit = TemperatureUnit.Fahrenheit;

            var result = MetrologicalInstituteService.BuildDailyWeahterReportForCityAsync(timeSeries, cityName, temperatureUnit);

            Assert.Equal(temperatureUnit, result.TemperatureUnit);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_City()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(city.Name, result.CityName);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_Date()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(DateTime.Now.AddDays(1).Date, result.Date);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_TemperatureMax()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(40, result.TemperatureMax);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_TemperatureAverage()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(30, result.TemperatureAverage);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_TemperatureMin()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(20, result.TemperatureMin);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_CloudCoverAverage()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(75, result.CloudCoverAverage);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_Percipitation()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(90, result.Percipitation);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_WindSpeedAverage()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(15, result.WindSpeedAverage);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_TemperatureUnit()
        {
            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var result = await unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city);

            Assert.Equal(TemperatureUnit.Fahrenheit, result.TemperatureUnit);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_LocationForecastUnitsNull_ShouldNotThrowException()
        {
            var metrologicalInstituteHttpClient = new Mock<IMetrologicalInstituteHttpClient>();
            metrologicalInstituteHttpClient.Setup(metrologicalInstituteHttpClient => metrologicalInstituteHttpClient.GetCompactLocationForcastAsync(It.IsAny<double>(), It.IsAny<double>()))
                   .Returns(Task.FromResult(
                       new LocationForecastCompactDTO
                       {
                           Properties = new LocationForecastProperties
                           {
                               Meta = new LocationForecastMetaData
                               {
                               },
                               TimeSeries = timeSeries
                           }
                       }
               ));

            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city));

            Assert.Null(exception);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_LocationForecastUnitsIsNull_ShouldNotThrowException()
        {
            var metrologicalInstituteHttpClient = new Mock<IMetrologicalInstituteHttpClient>();
            metrologicalInstituteHttpClient.Setup(metrologicalInstituteHttpClient => metrologicalInstituteHttpClient.GetCompactLocationForcastAsync(It.IsAny<double>(), It.IsAny<double>()))
                   .Returns(Task.FromResult(
                       new LocationForecastCompactDTO
                       {
                           Properties = new LocationForecastProperties
                           {
                               Meta = new LocationForecastMetaData
                               {
                               },
                               TimeSeries = timeSeries
                           }
                       }
               ));

            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city));

            Assert.Null(exception);
        }

        [Fact]
        public async Task RetrieveDataAndBuildDailyWeahterReportForCityAsync_LocationForecastPropertiesIsNull_ShouldThrowException()
        {
            var metrologicalInstituteHttpClient = new Mock<IMetrologicalInstituteHttpClient>();
            metrologicalInstituteHttpClient.Setup(metrologicalInstituteHttpClient => metrologicalInstituteHttpClient.GetCompactLocationForcastAsync(It.IsAny<double>(), It.IsAny<double>()))
                   .Returns(Task.FromResult(
                       new LocationForecastCompactDTO
                       {
                       }
               ));

            var unitUnderTest = new MetrologicalInstituteService(null, metrologicalInstituteHttpClient.Object, null);

            var exception = await Record.ExceptionAsync(() => unitUnderTest.RetrieveDataAndBuildDailyWeahterReportForCityAsync(city));

            Assert.NotNull(exception);
        }
    }
}