using Libreria.Api.DTOs;
using Libreria.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _service;
    public ClientesController(IClienteService service) => _service = service;

    // La búsqueda se traduce a SQL; no recorre una lista estática en memoria.
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ClienteDto>>> Buscar(
        [FromQuery] string? nombre, CancellationToken ct)
        => Ok(await _service.BuscarAsync(nombre, ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> Obtener(int id, CancellationToken ct)
        => Ok(await _service.ObtenerAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Crear([FromBody] ClienteCrearDto datos, CancellationToken ct)
    {
        var creado = await _service.CrearAsync(datos, ct);
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }
}
