using BusinessLogic.Interfaces.Infrastructure.HttpClients;
using BusinessLogic.Models.DTOs.Inbound;
using Infrastructure.Models.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.HttpClients
{
    public class MetrologicalInstituteHttpClient : IMetrologicalInstituteHttpClient
    {
        private readonly IOptions<MetrologicalInstituteHttpClientOptions> options;

        public MetrologicalInstituteHttpClient(IOptions<MetrologicalInstituteHttpClientOptions> options)
        {
            this.options = options;
        }

        public async Task<LocationForecastCompactDTO> GetCompactLocationForcastAsync(double latitude, double longitude)
        {
            var httpClient = new HttpClient();

            var message = new HttpRequestMessage(HttpMethod.Get, $"{options.Value.BaseUrl}/{options.Value.LocationForecastCompactPath}?lat={latitude}&lon={longitude}");

            message.Headers.TryAddWithoutValidation("User-Agent", options.Value.UserAgent);

            var result = await httpClient.SendAsync( message );

            var content = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
            {
                throw new Exception($"{result.StatusCode}: {content}");
            }

            return JsonSerializer.Deserialize<LocationForecastCompactDTO>( content );
        }
    }
}
