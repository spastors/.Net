using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Examples;

[ApiController]
[Route("api/ejemplos/di")]
public class DependenciasController : ControllerBase
{
    private readonly object _instancias;

    public DependenciasController(
        [FromKeyedServices("transient")] IInstanciaDemo transientA,
        [FromKeyedServices("transient")] IInstanciaDemo transientB,
        [FromKeyedServices("scoped")] IInstanciaDemo scopedA,
        [FromKeyedServices("scoped")] IInstanciaDemo scopedB,
        [FromKeyedServices("singleton")] IInstanciaDemo singletonA,
        [FromKeyedServices("singleton")] IInstanciaDemo singletonB)
    {
        // Mismo Id = mismo objeto. Compara también entre dos peticiones.
        _instancias = new
        {
            transient = new { a = transientA.Id, b = transientB.Id },
            scoped = new { a = scopedA.Id, b = scopedB.Id },
            singleton = new { a = singletonA.Id, b = singletonB.Id }
        };
    }

    [HttpGet]
    public IActionResult Obtener() => Ok(_instancias);

    [HttpGet("nombre")]
    public IActionResult ComprobarNombre(
        [FromQuery] string nombre,
        [FromKeyedServices("basico")] IPoliticaNombre basico,
        [FromKeyedServices("largo")] IPoliticaNombre largo)
        => Ok(new { basico = basico.EsValido(nombre), largo = largo.EsValido(nombre) });
}
