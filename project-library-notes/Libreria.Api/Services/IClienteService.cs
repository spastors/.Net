using Libreria.Api.DTOs;

namespace Libreria.Api.Services;

// Casos de uso de clientes. Devuelve DTOs, sin exponer entidades ni códigos HTTP.
public interface IClienteService
{
    Task<IReadOnlyList<ClienteDto>> BuscarAsync(string? nombre, CancellationToken ct);
    Task<ClienteDto> ObtenerAsync(int id, CancellationToken ct);
    Task<ClienteDto> CrearAsync(ClienteCrearDto datos, CancellationToken ct);
}
