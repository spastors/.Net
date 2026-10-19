using Libreria.Api.Data;
using Libreria.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Api.Repositories;

public class LibroRepository : ILibroRepository
{
    private readonly LibreriaDbContext _db;
    public LibroRepository(LibreriaDbContext db) => _db = db;

    public async Task<(List<Libro> Elementos, int Total)> BuscarAsync(
        string? texto, int pagina, int tamano, CancellationToken ct)
    {
        var consulta = _db.Libros.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(texto))
            consulta = consulta.Where(l => l.Titulo.Contains(texto) || l.Isbn == texto);

        // Where sigue siendo IQueryable: EF traduce el filtro antes de leer las filas.
        int total = await consulta.CountAsync(ct);
        var elementos = await consulta.Include(l => l.Editorial).OrderBy(l => l.Id)
            .Skip((pagina - 1) * tamano).Take(tamano).ToListAsync(ct);
        return (elementos, total);
    }

    public Task<Libro?> ObtenerAsync(int id, CancellationToken ct) =>
        _db.Libros.AsNoTracking().Include(l => l.Editorial)
            .FirstOrDefaultAsync(l => l.Id == id, ct);

    // FindAsync devuelve una entidad seguida: los cambios se detectan al guardar.
    public async Task<Libro?> ObtenerParaEditarAsync(int id, CancellationToken ct) =>
        await _db.Libros.FindAsync([id], ct);

    public Task<bool> ExisteIsbnAsync(string isbn, int? excluirId, CancellationToken ct) =>
        _db.Libros.AnyAsync(l => l.Isbn == isbn && (!excluirId.HasValue || l.Id != excluirId.Value), ct);

    // Agregar/Eliminar preparan cambios; GuardarAsync es quien los envía a SQL Server.
    public void Agregar(Libro libro) => _db.Libros.Add(libro);
    public void Eliminar(Libro libro) => _db.Libros.Remove(libro);
    public async Task GuardarAsync(CancellationToken ct) => await _db.SaveChangesAsync(ct);
}
