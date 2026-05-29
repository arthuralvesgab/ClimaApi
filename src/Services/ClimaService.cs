using System.Text.Json;
using ClimaApi.Models;

public class ClimaService
{
    private readonly HttpClient _http;
    public ClimaService(HttpClient http)
    {
        _http = http;
    }
    public async Task<ClimaResponse?> BuscarClima(double lat, double lon)
{
    var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
    var response = await _http.GetAsync(url);

    if (!response.IsSuccessStatusCode)
        return null;

    var json = await response.Content.ReadAsStringAsync();
    return JsonSerializer.Deserialize<ClimaResponse>(json);
}

}