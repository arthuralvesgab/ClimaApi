using System.Text.Json;

public class CidadeService
{
    private readonly HttpClient _http;
    public CidadeService(HttpClient http)
    {
        _http = http;
    }
    public async Task<(string? nome, string? uf, double lat, double lon)> BuscarCidade(string nome) 
    {
        var url = $"https://brasilapi.com.br/api/cptec/v1/cidade/{nome}";
        var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        return (null, null, 0, 0);

        var json = await response.Content.ReadAsStringAsync();
        var cidades = JsonSerializer.Deserialize<List<dynamic>>(json);

        if (cidades == null || cidades.Count == 0)
        return (null, null, 0, 0);

        var cidade = cidades[0];

        return (cidade.nome,
         cidade.estado,
         0, //acho que a api vai fornecer mais caso de erro ou seja 0 venha alterar
         0);
    }
}