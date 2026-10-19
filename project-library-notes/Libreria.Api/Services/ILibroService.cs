using Libreria.Api.DTOs;

namespace Libreria.Api.Services;

// Contrato de casos de uso de libros. Task representa un resultado que puede completarse después.
public interface ILibroService
{
    Task<PaginaDto<LibroDto>> BuscarAsync(string? texto, int pagina, int tamano, CancellationToken ct);
    Task<LibroDto> ObtenerAsync(int id, CancellationToken ct);
    Task<LibroDto> CrearAsync(LibroCrearDto datos, CancellationToken ct);
    Task<LibroDto> ActualizarAsync(int id, LibroActualizarDto datos, CancellationToken ct);
    Task EliminarAsync(int id, CancellationToken ct);
}
