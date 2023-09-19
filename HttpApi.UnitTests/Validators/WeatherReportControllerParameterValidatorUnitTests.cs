using BusinessLogic.Models.Enums;
using HttpApi.Validators;

namespace HttpApi.UnitTests.Validators
{
    public class WeatherReportControllerParameterValidatorUnitTests
    {
        [Fact]
        public void IsCityNameInvalid_EmptyString_ShouldReturnTrue()
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var result = unitUnderTest.IsCityNameInvalid("");

            Assert.True(result);
        }

        [Fact]
        public void IsCityNameInvalid_SpaceString_ShouldReturnTrue()
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var result = unitUnderTest.IsCityNameInvalid(" ");

            Assert.True(result);
        }

        [Fact]
        public void IsCityNameInvalid_NullString_ShouldReturnTrue()
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var result = unitUnderTest.IsCityNameInvalid(null);

            Assert.True(result);
        }


        [Fact]
        public void IsCityNameInvalid_String_ShouldReturnFalse()
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var result = unitUnderTest.IsCityNameInvalid("Oslo");

            Assert.False(result);
        }

        [Fact]
        public void IsDateOrderInvalid_ProperOrder_ShouldReturnFalse()
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 20);

            var result = unitUnderTest.IsDateOrderInvalid(fromDate, toDate);

            Assert.False(result);
        }

        [Fact]
        public void IsDateOrderInvalid_SameDate_ShouldReturnFalse()
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var fromDate = new DateTime(2023, 9, 19);
            var toDate = new DateTime(2023, 9, 19);

            var result = unitUnderTest.IsDateOrderInvalid(fromDate, toDate);

            Assert.False(result);
        }

        [Fact]
        public void IsDateOrderInvalid_ImproperOrder_ShouldReturnTrue()
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var fromDate = new DateTime(2023, 9, 21);
            var toDate = new DateTime(2023, 9, 20);

            var result = unitUnderTest.IsDateOrderInvalid(fromDate, toDate);

            Assert.True(result);
        }

        [Theory]
        [InlineData((TemperatureUnit)(-1), true)]
        [InlineData((TemperatureUnit)0, false)]
        [InlineData((TemperatureUnit)1, false)]
        [InlineData((TemperatureUnit)2, false)]
        [InlineData((TemperatureUnit)3, true)]
        public void IsTemperatureUnitOutOfRange_Theory(TemperatureUnit temperatureUnit, bool expectedResult)
        {
            var unitUnderTest = new WeatherReportControllerParameterValidator();

            var result = unitUnderTest.IsTemperatureUnitOutOfRange(temperatureUnit);

            Assert.Equal(expectedResult, result);
        }
    }
}
