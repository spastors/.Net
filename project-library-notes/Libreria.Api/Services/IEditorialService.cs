using Libreria.Api.DTOs;

namespace Libreria.Api.Services;

// Operaciones de negocio de editoriales; DI entrega su implementación al controlador.
public interface IEditorialService
{
    Task<IReadOnlyList<EditorialDto>> ListarAsync(CancellationToken ct);
    Task<EditorialDto> ObtenerAsync(int id, CancellationToken ct);
    Task<EditorialDto> CrearAsync(EditorialEntradaDto datos, CancellationToken ct);
    Task<EditorialDto> ActualizarAsync(int id, EditorialEntradaDto datos, CancellationToken ct);
    Task EliminarAsync(int id, CancellationToken ct);
}
