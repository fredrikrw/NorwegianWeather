using BusinessLogic.Models.Enums;
using HttpApi.Interfaces.Validators;

namespace HttpApi.Validators
{
    public class WeatherReportControllerParameterValidator : IWeatherReportControllerParameterValidator
    {

        public bool IsCityNameInvalid(string cityName)
        {
            return string.IsNullOrEmpty(cityName);
        }

        public bool IsDateOrderInvalid(DateTime fromDate, DateTime toDate)
        {
            return fromDate.Date > toDate.Date;
        }

        public bool IsTemperatureUnitOutOfRange(TemperatureUnit temperatureUnit)
        {
            return (int)temperatureUnit < 0 || (int)temperatureUnit > 2;
        }
    }
}
