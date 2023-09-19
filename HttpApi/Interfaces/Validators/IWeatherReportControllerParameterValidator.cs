using BusinessLogic.Models.Enums;

namespace HttpApi.Interfaces.Validators
{
    public interface IWeatherReportControllerParameterValidator
    {
        public abstract bool IsCityNameInvalid(string cityName);
        public abstract bool IsDateOrderInvalid(DateTime fromDate, DateTime toDate);
        public abstract bool IsTemperatureUnitOutOfRange(TemperatureUnit temperatureUnit);
    }
}
