using System.Net.Http.Json;
using XiletradeAuth.Models;

namespace XiletradeAuth.Services;

public class WeatherService
{
    private readonly HttpClient _http;

    public WeatherService(HttpClient http)
    {
        _http = http;
    }

    public async Task<IEnumerable<WeatherForecast>> GetForecastAsync()
    {
        return await _http?.GetFromJsonAsync<IEnumerable<WeatherForecast>>("sample-data/weather.json");
    }
}