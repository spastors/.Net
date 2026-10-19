using System.Net.Http.Json;
using System.Text.Json;
using Libreria.Api.DTOs;
using Libreria.Api.Validators;
using Microsoft.Extensions.Caching.Memory;

namespace Libreria.Api.Services;

// Cliente tipado: AddHttpClient configura BaseAddress, timeout y cabeceras una sola vez por cliente.
public class CatalogoExternoService : ICatalogoExternoService
{
    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;

    public CatalogoExternoService(HttpClient http, IMemoryCache cache)
    {
        _http = http;
        _cache = cache;
    }

    public async Task<LibroExternoDto> ObtenerPorIsbnAsync(string isbn, CancellationToken ct)
    {
        if (!Isbn13.EsValido(isbn))
            throw new ReglaNegocioException("Indica un ISBN-13 válido, sin guiones.");

        string clave = "openlibrary:" + isbn;
        if (_cache.TryGetValue(clave, out LibroExternoDto? guardado) && guardado is not null)
            return guardado;

        try
        {
            using var respuesta = await _http.GetAsync(
                $"api/books?bibkeys=ISBN:{Uri.EscapeDataString(isbn)}&format=json&jscmd=data", ct);
            respuesta.EnsureSuccessStatusCode();

            var libros = await respuesta.Content
                .ReadFromJsonAsync<Dictionary<string, LibroOpenLibraryDto>>(ct)
                ?? throw new HttpRequestException("El catálogo devolvió un cuerpo vacío.");

            if (!libros.TryGetValue("ISBN:" + isbn, out var libro))
                throw new RecursoNoEncontradoException("El ISBN no aparece en Open Library.");

            if (libro is null || string.IsNullOrWhiteSpace(libro.Titulo))
                throw new HttpRequestException("El catálogo devolvió un libro sin título.");

            var dto = new LibroExternoDto(isbn, libro.Titulo,
                (libro.Autores ?? []).Where(a => a is not null).Select(a => a.Nombre).ToArray(),
                (libro.Editoriales ?? []).Where(e => e is not null).Select(e => e.Nombre).ToArray());
            // Evita repetir consultas de un mismo libro durante cinco minutos.
            _cache.Set(clave, dto, TimeSpan.FromMinutes(5));
            return dto;
        }
        catch (OperationCanceledException) when (!ct.IsCancellationRequested)
        {
            throw new TimeoutException("El catálogo externo agotó el tiempo de espera.");
        }
        catch (JsonException ex)
        {
            throw new HttpRequestException("El catálogo devolvió JSON no válido.", ex);
        }
    }
}
