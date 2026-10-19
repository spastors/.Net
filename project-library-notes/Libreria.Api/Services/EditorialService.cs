using FluentValidation;
using Libreria.Api.DTOs;
using Libreria.Api.Models;
using Libreria.Api.Repositories;

namespace Libreria.Api.Services;

// Reglas de editoriales: validar nombres e impedir borrados cuando existen libros asociados.
public class EditorialService : IEditorialService
{
    private readonly IEditorialRepository _repository;
    private readonly IValidator<EditorialEntradaDto> _validator;

    public EditorialService(IEditorialRepository repository, IValidator<EditorialEntradaDto> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IReadOnlyList<EditorialDto>> ListarAsync(CancellationToken ct) =>
        (await _repository.ListarAsync(ct)).Select(e => new EditorialDto(e.Id, e.Nombre)).ToList();

    public async Task<EditorialDto> ObtenerAsync(int id, CancellationToken ct)
    {
        var editorial = await _repository.ObtenerAsync(id, ct)
            ?? throw new RecursoNoEncontradoException("Editorial no encontrada.");
        return new(editorial.Id, editorial.Nombre);
    }

    public async Task<EditorialDto> CrearAsync(EditorialEntradaDto datos, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(datos, ct);
        var editorial = new Editorial { Nombre = datos.Nombre.Trim() };
        _repository.Agregar(editorial);
        await _repository.GuardarAsync(ct);
        return new(editorial.Id, editorial.Nombre);
    }

    public async Task<EditorialDto> ActualizarAsync(int id, EditorialEntradaDto datos, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(datos, ct);
        var editorial = await _repository.ObtenerParaEditarAsync(id, ct)
            ?? throw new RecursoNoEncontradoException("Editorial no encontrada.");
        editorial.Nombre = datos.Nombre.Trim();
        await _repository.GuardarAsync(ct);
        return new(editorial.Id, editorial.Nombre);
    }

    public async Task EliminarAsync(int id, CancellationToken ct)
    {
        var editorial = await _repository.ObtenerParaEditarAsync(id, ct)
            ?? throw new RecursoNoEncontradoException("Editorial no encontrada.");
        if (await _repository.TieneLibrosAsync(id, ct))
            throw new ConflictoException("La editorial tiene libros asociados.");
        _repository.Eliminar(editorial);
        await _repository.GuardarAsync(ct);
    }
}
