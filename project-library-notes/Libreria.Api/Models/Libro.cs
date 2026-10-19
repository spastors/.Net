namespace Libreria.Api.Models;

// Entidad persistida: un libro del catálogo, identificado por su ISBN-13.
public class Libro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public string Isbn { get; set; } = "";
    public decimal Precio { get; set; }
    public int EditorialId { get; set; }

    // Navegación: EF la carga con Include; no se expone directamente al cliente.
    public Editorial? Editorial { get; set; }
}
