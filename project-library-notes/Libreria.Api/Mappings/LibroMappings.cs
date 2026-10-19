using Libreria.Api.DTOs;
using Libreria.Api.Models;

namespace Libreria.Api.Mappings;

// Mapeo manual y visible. No requiere AutoMapper ni expone navegaciones completas de EF.
public static class LibroMappings
{
    public static LibroDto ADto(this Libro libro, string? editorial = null) =>
        new(libro.Id, libro.Titulo, libro.Isbn, libro.Precio, libro.EditorialId,
            editorial ?? libro.Editorial?.Nombre ?? "");

    public static void Aplicar(this LibroCrearDto datos, Libro existente)
    {
        // Modifica la misma instancia seguida por EF; nunca cambia su Id.
        existente.Titulo = datos.Titulo.Trim();
        existente.Isbn = datos.Isbn;
        existente.Precio = datos.Precio;
        existente.EditorialId = datos.EditorialId;
    }
}
