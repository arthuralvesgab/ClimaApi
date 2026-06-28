using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/clima")] // rota da api
public class ClimaController : ControllerBase
{
    private readonly CidadeService _cidadeService; // busca dados da cidade 
    private readonly ClimaService _climaService; //busca dados do clima com as coordenadas

    public ClimaController(CidadeService cidadeService, ClimaService climaService)
    {
        _cidadeService = cidadeService;
        _climaService = climaService;
    }

    [HttpGet("{nome}")] // recebe o nome da cidade pela url
    public async Task<IActionResult> Get(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome) || nome.Length < 2) // valida o nome da cidade, retorna erro 400 se tiver erro
        {
            return BadRequest(new
            {
                erro = true,
                codigo = "NOME_INVALIDO"
            });
        }

        var cidade = await _cidadeService.BuscarCidade(nome);

        if (cidade.nome == null) // verifica se a cidade foi encontrada
        {
            return NotFound(new
            {
                erro = true,
                codigo = "CIDADE_NAO_ENCONTRADA"
            });
        }

        var clima = await _climaService.BuscarClima(cidade.lat, cidade.lon); // busca o clima

        if (clima == null) // verifica se o serviço esta disponível
        {
            return StatusCode(503, new
            {
                erro = true,
                codigo = "SERVICO_EXTERNO_INDISPONIVEL"
            });
        }

        return Ok(new
        {
            nome = cidade.nome,
            estado = cidade.uf,
            clima = clima.current_weather,
            consultado_em = DateTime.UtcNow
        });
    }
}