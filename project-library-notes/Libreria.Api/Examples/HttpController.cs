using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Examples;

public record ConsultaDemoDto(string Titulo);

[ApiController]
[Route("api/ejemplos/http")]
public class HttpController : ControllerBase
{
    [HttpPost]
    public IActionResult Recibir(
        [FromBody] ConsultaDemoDto datos,
        [FromHeader(Name = "X-Canal")] string? canal)
    {
        // [FromBody] lee JSON; [FromHeader] lee una cabecera. Solo puede haber un parámetro FromBody.
        // Una cabecera enviada por el cliente no demuestra su identidad ni sus permisos.
        return Ok(new { canal, titulo = datos.Titulo });
    }
}
