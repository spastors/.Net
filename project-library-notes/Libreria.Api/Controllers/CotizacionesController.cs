using Libreria.Api.DTOs;
using Libreria.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CotizacionesController : ControllerBase
{
    private readonly ICotizacionService _service;
    public CotizacionesController(ICotizacionService service) => _service = service;

    // Calcula un presupuesto; no crea un pedido ni consulta precios de la base de datos.
    [HttpPost]
    public async Task<ActionResult<CotizacionDto>> Calcular(
        [FromBody] CotizacionEntradaDto datos, CancellationToken ct)
        => Ok(await _service.CalcularAsync(datos, ct));
}
