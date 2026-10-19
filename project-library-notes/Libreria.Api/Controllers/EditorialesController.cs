using Libreria.Api.DTOs;
using Libreria.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EditorialesController : ControllerBase
{
    private readonly IEditorialService _service;
    public EditorialesController(IEditorialService service) => _service = service;

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<EditorialDto>>> Listar(CancellationToken ct)
        => Ok(await _service.ListarAsync(ct));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EditorialDto>> Obtener(int id, CancellationToken ct)
        => Ok(await _service.ObtenerAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<EditorialDto>> Crear([FromBody] EditorialEntradaDto datos, CancellationToken ct)
    {
        var creada = await _service.CrearAsync(datos, ct);
        return CreatedAtAction(nameof(Obtener), new { id = creada.Id }, creada);
    }

    // El Id llega por URL; el DTO solo contiene los campos editables.
    [HttpPut("{id:int}")]
    public async Task<ActionResult<EditorialDto>> Actualizar(
        int id, [FromBody] EditorialEntradaDto datos, CancellationToken ct)
        => Ok(await _service.ActualizarAsync(id, datos, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        // El servicio rechaza el borrado si todavía hay libros asociados.
        await _service.EliminarAsync(id, ct);
        return NoContent();
    }
}
