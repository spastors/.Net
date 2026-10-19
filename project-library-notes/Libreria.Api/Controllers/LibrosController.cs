using Libreria.Api.DTOs;
using Libreria.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

// [controller] se sustituye por "Libros"; el nombre del método no se añade a la URL.
[ApiController]
[Route("api/[controller]")]
public class LibrosController : ControllerBase
{
    private readonly ILibroService _service;

    // DI entrega el servicio: el controlador no crea repositorios ni DbContext.
    public LibrosController(ILibroService service) => _service = service;

    // GET /api/libros?texto=aventura&pagina=1&tamano=25. El token cancela la consulta si el cliente se desconecta.
    [HttpGet]
    public async Task<ActionResult<PaginaDto<LibroDto>>> Buscar(
        [FromQuery] string? texto, CancellationToken ct,
        [FromQuery] int pagina = 1, [FromQuery] int tamano = 25)
        => Ok(await _service.BuscarAsync(texto, pagina, tamano, ct));

    // {id:int} toma el identificador de la ruta; una ruta no numérica no coincide.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<LibroDto>> Obtener(int id, CancellationToken ct)
        => Ok(await _service.ObtenerAsync(id, ct));

    [HttpPost]
    public async Task<ActionResult<LibroDto>> Crear([FromBody] LibroCrearDto datos, CancellationToken ct)
    {
        var creado = await _service.CrearAsync(datos, ct);
        // 201 + cuerpo + cabecera Location: URL del GET que permite consultar lo creado.
        return CreatedAtAction(nameof(Obtener), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<LibroDto>> Actualizar(
        int id, [FromBody] LibroActualizarDto datos, CancellationToken ct)
        => Ok(await _service.ActualizarAsync(id, datos, ct));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
    {
        await _service.EliminarAsync(id, ct);
        return NoContent(); // 204: operación realizada, sin cuerpo.
    }
}
