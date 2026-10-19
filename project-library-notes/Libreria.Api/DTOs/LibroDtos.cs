namespace Libreria.Api.DTOs;

// Entrada: el Id lo genera SQL Server; en PUT se toma exclusivamente de la URL.
public class LibroCrearDto
{
    public string Titulo { get; set; } = "";
    public string Isbn { get; set; } = "";
    public decimal Precio { get; set; }
    public int EditorialId { get; set; }
}

public class LibroActualizarDto : LibroCrearDto { }

// Salida inmutable: record permite declarar propiedades sin exponer una entidad de EF.
public record LibroDto(int Id, string Titulo, string Isbn, decimal Precio,
    int EditorialId, string Editorial);
