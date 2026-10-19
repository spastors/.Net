using Libreria.Api.DTOs;
using Libreria.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Controllers;

[ApiController]
[Route("api/catalogo-externo")]
public class CatalogoExternoController : ControllerBase
{
    private readonly ICatalogoExternoService _service;
    public CatalogoExternoController(ICatalogoExternoService service) => _service = service;

    // Consume Open Library; el ISBN consultado no se guarda automáticamente en nuestra BD.
    [HttpGet("{isbn}")]
    public async Task<ActionResult<LibroExternoDto>> Obtener(string isbn, CancellationToken ct)
        => Ok(await _service.ObtenerPorIsbnAsync(isbn, ct));
}
