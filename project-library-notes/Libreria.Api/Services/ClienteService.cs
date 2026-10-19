using FluentValidation;
using Libreria.Api.DTOs;
using Libreria.Api.Models;
using Libreria.Api.Repositories;

namespace Libreria.Api.Services;

// Valida, normaliza el correo y coordina el alta; no conoce HTTP ni escribe SQL.
public class ClienteService : IClienteService
{
    private readonly IClienteRepository _repository;
    private readonly IValidator<ClienteCrearDto> _validator;

    public ClienteService(IClienteRepository repository, IValidator<ClienteCrearDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IReadOnlyList<ClienteDto>> BuscarAsync(string? nombre, CancellationToken ct)
    {
        if (nombre?.Length > 100)
            throw new ReglaNegocioException("La búsqueda admite hasta 100 caracteres.");
        return (await _repository.BuscarAsync(nombre?.Trim(), ct)).Select(ADto).ToList();
    }

    public async Task<ClienteDto> ObtenerAsync(int id, CancellationToken ct) =>
        ADto(await _repository.ObtenerAsync(id, ct)
            ?? throw new RecursoNoEncontradoException("Cliente no encontrado."));

    public async Task<ClienteDto> CrearAsync(ClienteCrearDto datos, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(datos, ct);
        string email = datos.Email.Trim().ToLowerInvariant();
        if (await _repository.ExisteEmailAsync(email, ct))
            throw new ConflictoException("Ya existe un cliente con ese correo.");

        var cliente = new Cliente
        {
            Nombre = datos.Nombre.Trim(), Email = email, FechaNacimiento = datos.FechaNacimiento
        };
        _repository.Agregar(cliente);
        await _repository.GuardarAsync(ct);
        return ADto(cliente);
    }

    private static ClienteDto ADto(Cliente cliente) =>
        new(cliente.Id, cliente.Nombre, cliente.Email, cliente.FechaNacimiento);
}
