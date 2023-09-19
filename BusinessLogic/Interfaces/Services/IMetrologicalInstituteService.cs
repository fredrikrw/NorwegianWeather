namespace BusinessLogic.Interfaces.Services
{
    public interface IMetrologicalInstituteService
    {
        Task RetrieveDataAndBuildDailyWeatherReportForAllCities();
    }
}
