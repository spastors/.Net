using Libreria.Api.Models;

namespace Libreria.Api.Repositories;

// Consultas concretas: el servicio no recibe un IQueryable ni necesita conocer SQL.
public interface ILibroRepository
{
    Task<(List<Libro> Elementos, int Total)> BuscarAsync(
        string? texto, int pagina, int tamano, CancellationToken ct);
    Task<Libro?> ObtenerAsync(int id, CancellationToken ct);
    Task<Libro?> ObtenerParaEditarAsync(int id, CancellationToken ct);
    Task<bool> ExisteIsbnAsync(string isbn, int? excluirId, CancellationToken ct);
    void Agregar(Libro libro);
    void Eliminar(Libro libro);
    Task GuardarAsync(CancellationToken ct);
}
