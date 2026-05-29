using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/clima")]
public class ClimaController : ControllerBase
{
    private readonly CidadeService _cidadeService;
    private readonly ClimaService _climaService;

    public ClimaController(CidadeService cidadeService, ClimaService climaService)
    {
        _cidadeService = cidadeService;
        _climaService = climaService;
    }

    [HttpGet("{nome}")]
    public async Task<IActionResult> Get(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome) || nome.Length < 2)
        {
            return BadRequest(new
            {
                erro = true,
                codigo = "NOME_INVALIDO"
            });
        }

        var cidade = await _cidadeService.BuscarCidade(nome);

        if (cidade.nome == null)
        {
            return NotFound(new
            {
                erro = true,
                codigo = "CIDADE_NAO_ENCONTRADA"
            });
        }

        var clima = await _climaService.BuscarClima(cidade.lat, cidade.lon);

        if (clima == null)
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