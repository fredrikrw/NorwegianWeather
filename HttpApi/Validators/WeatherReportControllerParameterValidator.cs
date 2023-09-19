using BusinessLogic.Models.Enums;

namespace HttpApi.Validators
{
    public class WeatherReportControllerParameterValidator
    {

        public static bool IsCityNameInvalid(string cityName)
        {
            return string.IsNullOrEmpty(cityName);
        }

        public static bool IsDateOrderInvalid(DateTime fromDate, DateTime toDate)
        {
            return fromDate.Date > toDate.Date;
        }

        public static bool IsTemperatureUnitOutOfRange(TemperatureUnit temperatureUnit)
        {
            return (int)temperatureUnit < 0 || (int)temperatureUnit > 2;
        }
    }
}
