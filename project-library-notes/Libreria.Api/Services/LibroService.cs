using FluentValidation;
using Libreria.Api.DTOs;
using Libreria.Api.Mappings;
using Libreria.Api.Models;
using Libreria.Api.Repositories;

namespace Libreria.Api.Services;

// Coordina validación, reglas, persistencia y DTO. No conoce ControllerBase ni IActionResult.
public class LibroService : ILibroService
{
    private readonly ILibroRepository _libros;
    private readonly IEditorialRepository _editoriales;
    private readonly IValidator<LibroCrearDto> _crearValidator;
    private readonly IValidator<LibroActualizarDto> _actualizarValidator;

    public LibroService(ILibroRepository libros, IEditorialRepository editoriales,
        IValidator<LibroCrearDto> crearValidator, IValidator<LibroActualizarDto> actualizarValidator)
    {
        _libros = libros;
        _editoriales = editoriales;
        _crearValidator = crearValidator;
        _actualizarValidator = actualizarValidator;
    }

    public async Task<PaginaDto<LibroDto>> BuscarAsync(
        string? texto, int pagina, int tamano, CancellationToken ct)
    {
        if (pagina is < 1 or > 1_000_000 || tamano is < 1 or > 100)
            throw new ReglaNegocioException("Página: 1..1000000. Tamaño: 1..100.");
        if (texto?.Length > 200)
            throw new ReglaNegocioException("La búsqueda admite hasta 200 caracteres.");

        var (elementos, total) = await _libros.BuscarAsync(texto?.Trim(), pagina, tamano, ct);
        return new(elementos.Select(l => l.ADto()).ToList(), total, pagina, tamano);
    }

    public async Task<LibroDto> ObtenerAsync(int id, CancellationToken ct)
    {
        var libro = await _libros.ObtenerAsync(id, ct)
            ?? throw new RecursoNoEncontradoException("Libro no encontrado.");
        return libro.ADto();
    }

    public async Task<LibroDto> CrearAsync(LibroCrearDto datos, CancellationToken ct)
    {
        // El servicio valida aunque lo invoque un proceso distinto del controlador.
        await _crearValidator.ValidateAndThrowAsync(datos, ct);
        var editorial = await ComprobarReglasAsync(datos, null, ct);
        var libro = new Libro();
        datos.Aplicar(libro);
        _libros.Agregar(libro);
        await _libros.GuardarAsync(ct);
        return libro.ADto(editorial.Nombre); // El Id ya fue generado por SQL Server.
    }

    public async Task<LibroDto> ActualizarAsync(int id, LibroActualizarDto datos, CancellationToken ct)
    {
        await _actualizarValidator.ValidateAndThrowAsync(datos, ct);
        var libro = await _libros.ObtenerParaEditarAsync(id, ct)
            ?? throw new RecursoNoEncontradoException("Libro no encontrado.");
        var editorial = await ComprobarReglasAsync(datos, id, ct);

        // Se aplica EditorialId del DTO, corrigiendo la autoasignación del ejemplo original.
        datos.Aplicar(libro);
        await _libros.GuardarAsync(ct);
        return libro.ADto(editorial.Nombre);
    }

    public async Task EliminarAsync(int id, CancellationToken ct)
    {
        var libro = await _libros.ObtenerParaEditarAsync(id, ct)
            ?? throw new RecursoNoEncontradoException("Libro no encontrado.");
        _libros.Eliminar(libro);
        await _libros.GuardarAsync(ct);
    }

    private async Task<Editorial> ComprobarReglasAsync(
        LibroCrearDto datos, int? excluirId, CancellationToken ct)
    {
        var editorial = await _editoriales.ObtenerAsync(datos.EditorialId, ct)
            ?? throw new ReglaNegocioException("La editorial indicada no existe.");
        if (await _libros.ExisteIsbnAsync(datos.Isbn, excluirId, ct))
            throw new ConflictoException("Ya existe un libro con ese ISBN.");
        // El índice único y la FK siguen protegiendo estas reglas ante peticiones simultáneas.
        return editorial;
    }
}
