using Libreria.Api.Data;
using Libreria.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Api.Repositories;

// Sustituye la lista static del ejemplo de personas por datos persistidos.
public class ClienteRepository : IClienteRepository
{
    private readonly LibreriaDbContext _db;
    public ClienteRepository(LibreriaDbContext db) => _db = db;

    public Task<List<Cliente>> BuscarAsync(string? nombre, CancellationToken ct)
    {
        var consulta = _db.Clientes.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(nombre))
            consulta = consulta.Where(c => c.Nombre.Contains(nombre));
        return consulta.OrderBy(c => c.Id).Take(100).ToListAsync(ct);
    }

    public Task<Cliente?> ObtenerAsync(int id, CancellationToken ct) =>
        _db.Clientes.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);
    public Task<bool> ExisteEmailAsync(string email, CancellationToken ct) =>
        _db.Clientes.AnyAsync(c => c.Email == email, ct);
    public void Agregar(Cliente cliente) => _db.Clientes.Add(cliente);
    public async Task GuardarAsync(CancellationToken ct) => await _db.SaveChangesAsync(ct);
}
