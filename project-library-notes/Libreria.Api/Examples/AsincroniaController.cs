using Microsoft.AspNetCore.Mvc;

namespace Libreria.Api.Examples;

[ApiController]
[Route("api/ejemplos/esperas")]
public class AsincroniaController : ControllerBase
{
    // Ejemplo deliberado de bloqueo: el hilo no puede atender otro trabajo durante la espera.
    [HttpGet("sincrona")]
    public IActionResult Sincrona()
    {
        Thread.Sleep(500);
        return Ok(new { mensaje = "Espera síncrona finalizada." });
    }

    [HttpGet("asincrona")]
    public async Task<IActionResult> Asincrona(CancellationToken ct)
    {
        // Simula una espera de E/S. await libera el hilo mientras el temporizador está pendiente.
        await Task.Delay(500, ct);
        return Ok(new { mensaje = "Espera asíncrona finalizada." });
    }

    [HttpGet("concurrente")]
    public async Task<IActionResult> Concurrente(CancellationToken ct)
    {
        // Las dos esperas avanzan a la vez: ~500 ms, frente a ~1000 ms si fueran consecutivas.
        // No usar Task.WhenAll con dos operaciones sobre el mismo DbContext.
        await Task.WhenAll(Task.Delay(500, ct), Task.Delay(500, ct));
        return Ok(new { mensaje = "Dos esperas concurrentes finalizadas." });
    }
}
