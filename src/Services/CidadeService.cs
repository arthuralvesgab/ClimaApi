using System.Text.Json;

public class CidadeService
{
    private readonly HttpClient _http; //Faz o resquisito externo 
    public CidadeService(HttpClient http)
    {
        _http = http;
    }

    public async Task<(string? nome, string? uf, double lat, double lon)> BuscarCidade(string nome) 
    {
        var url = $"https://brasilapi.com.br/api/cptec/v1/cidade/{nome}"; // Api retorna o nome e informações da cidade, caso não encontre retorna 404
        var response = await _http.GetAsync(url); // Faz a requisição para a Api

        if (!response.IsSuccessStatusCode) // Tratamento de erro HTTP, caso a Api de erro retorna o nome invalido e api fora do ar
        return (null, null, 0, 0);

        var json = await response.Content.ReadAsStringAsync(); // converte o json
        var cidades = JsonSerializer.Deserialize<List<dynamic>>(json);

        if (cidades == null || cidades.Count == 0) //validação da resposta
        return (null, null, 0, 0);

        var cidade = cidades[0]; //pega a primeira cidade da lista

        return (cidade.nome,
         cidade.estado,
         0, //acho que a api vai fornecer mais caso de erro ou seja 0 venha alterar
         0);
    }
}