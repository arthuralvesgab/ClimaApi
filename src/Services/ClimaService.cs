using System.Text.Json;
using ClimaApi.Models;

public class ClimaService //Classe responsavel por buscar o clima na Api
{
    private readonly HttpClient _http; //faz chamados a api externa
    public ClimaService(HttpClient http)
    {
        _http = http;
    }
    public async Task<ClimaResponse?> BuscarClima(double lat, double lon)
{
    var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current_weather=true";
    var response = await _http.GetAsync(url);

    if (!response.IsSuccessStatusCode) // valida a resposta da api
        return null;

    var json = await response.Content.ReadAsStringAsync(); // converte a resposta da api em json para string
    return JsonSerializer.Deserialize<ClimaResponse>(json);// converte o json para um objeto em C#
}

}