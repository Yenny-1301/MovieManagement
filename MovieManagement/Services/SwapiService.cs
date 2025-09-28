using Microsoft.Extensions.Options;
using MovieManagement.Config;
using MovieManagement.DTOs.Responses;
using MovieManagement.Services.Interfaces;

namespace MovieManagement.Services
{
    public class SwapiService : ISwapiService
    {
        private readonly HttpClient _httpClient;
        private readonly SwapiOptions _options;

        public SwapiService(HttpClient httpClient, IOptions<SwapiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<List<SwapiFilmResponse>> GetFilmsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<SwapiFilmsResponses>($"{_options.BaseUrl}films/");
            return response?.Results ?? new List<SwapiFilmResponse>();
        }
    }
}
