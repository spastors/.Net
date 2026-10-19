using Libreria.Api.Data;
using Libreria.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Api.Repositories;

// Las lecturas no siguen entidades; la edición sí conserva seguimiento.
public class EditorialRepository : IEditorialRepository
{
    private readonly LibreriaDbContext _db;
    public EditorialRepository(LibreriaDbContext db) => _db = db;

    public Task<List<Editorial>> ListarAsync(CancellationToken ct) =>
        _db.Editoriales.AsNoTracking().OrderBy(e => e.Id).Take(100).ToListAsync(ct);
    public Task<Editorial?> ObtenerAsync(int id, CancellationToken ct) =>
        _db.Editoriales.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
    public async Task<Editorial?> ObtenerParaEditarAsync(int id, CancellationToken ct) =>
        await _db.Editoriales.FindAsync([id], ct);
    public Task<bool> TieneLibrosAsync(int id, CancellationToken ct) =>
        _db.Libros.AnyAsync(l => l.EditorialId == id, ct);
    public void Agregar(Editorial editorial) => _db.Editoriales.Add(editorial);
    public void Eliminar(Editorial editorial) => _db.Editoriales.Remove(editorial);
    public async Task GuardarAsync(CancellationToken ct) => await _db.SaveChangesAsync(ct);
}
