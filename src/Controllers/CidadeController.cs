using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

[ApiController]
[Route("api/v1/cidade")] //rota da api

public class CidadeController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _http = new HttpClient();
    public CidadeController(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    [HttpGet("{uf}")]
    public async Task<IActionResult> Get(string uf, [FromQuery] int limite = 10)
    {
        if(uf.Length != 2) // verifica se a sigla tem 2 caracteres, se não tiver  erro 400
        {
            return BadRequest(new { erro = true, codigo = "SIGLA_UF_INVALIDA" });
        }
         var url = $"https://brasilapi.com.br/api/ibge/municipios/v1/{uf}";
         var response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode) // retorna erro 404
        {
            return NotFound(new { erro = true, codigo = "UF_NAO_ENCONTRADA"});
        }
        // abaixo tem a conversão do json e para funcionar precisa da classe cidade
        var json = await response.Content.ReadAsStringAsync();
        var cidades = JsonSerializer.Deserialize<List<Cidade>>(json);

        var resultado = cidades?.Take(limite).Select(c => new { nome = c.nome}); // aplica limite na responta

        return Ok(new
        {
            uf,
            quantidade_retornada = resultado?.Count(),
            cidades = resultado,
            consultado_em = DateTime.UtcNow
        });

    }

}