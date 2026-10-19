namespace Libreria.Api.DTOs;

// Genérico: el mismo formato sirve para páginas de distintos tipos de DTO.
public record PaginaDto<T>(IReadOnlyList<T> Elementos, int Total, int Pagina, int Tamano);
