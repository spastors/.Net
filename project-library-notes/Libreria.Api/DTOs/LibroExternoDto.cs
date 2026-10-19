using System.Text.Json.Serialization;

namespace Libreria.Api.DTOs;

public record LibroExternoDto(string Isbn, string Titulo,
    IReadOnlyList<string> Autores, IReadOnlyList<string> Editoriales);

// Formato específico de Open Library. No se guarda como entidad en nuestra BD.
public class LibroOpenLibraryDto
{
    [JsonPropertyName("title")]
    public string Titulo { get; set; } = "";

    [JsonPropertyName("authors")]
    public List<NombreExternoDto> Autores { get; set; } = [];

    [JsonPropertyName("publishers")]
    public List<NombreExternoDto> Editoriales { get; set; } = [];
}

public class NombreExternoDto
{
    [JsonPropertyName("name")]
    public string Nombre { get; set; } = "";
}
