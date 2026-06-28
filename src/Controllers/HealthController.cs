using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/health")] // rota da api
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        // Retorna infomações da API esta ok
        return Ok(new
        {
            status = "Healthy",
            versao = "1.0.0",
            timestamp = DateTime.UtcNow
        });
    }
}