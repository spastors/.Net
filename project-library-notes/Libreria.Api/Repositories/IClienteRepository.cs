using Libreria.Api.Models;

namespace Libreria.Api.Repositories;

// Contrato de persistencia: el servicio pide estas operaciones sin conocer Entity Framework.
public interface IClienteRepository
{
    Task<List<Cliente>> BuscarAsync(string? nombre, CancellationToken ct);
    Task<Cliente?> ObtenerAsync(int id, CancellationToken ct);
    Task<bool> ExisteEmailAsync(string email, CancellationToken ct);
    void Agregar(Cliente cliente);
    Task GuardarAsync(CancellationToken ct);
}
