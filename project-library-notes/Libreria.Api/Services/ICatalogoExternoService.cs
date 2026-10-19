using Libreria.Api.DTOs;

namespace Libreria.Api.Services;

// El controlador pide datos externos mediante este contrato; desconoce HttpClient y el proveedor.
public interface ICatalogoExternoService
{
    Task<LibroExternoDto> ObtenerPorIsbnAsync(string isbn, CancellationToken ct);
}
