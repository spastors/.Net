using Libreria.Api.Models;

namespace Libreria.Api.Repositories;

// Contrato de acceso a editoriales; permite sustituir la implementación SQL.
public interface IEditorialRepository
{
    Task<List<Editorial>> ListarAsync(CancellationToken ct);
    Task<Editorial?> ObtenerAsync(int id, CancellationToken ct);
    Task<Editorial?> ObtenerParaEditarAsync(int id, CancellationToken ct);
    Task<bool> TieneLibrosAsync(int id, CancellationToken ct);
    void Agregar(Editorial editorial);
    void Eliminar(Editorial editorial);
    Task GuardarAsync(CancellationToken ct);
}
