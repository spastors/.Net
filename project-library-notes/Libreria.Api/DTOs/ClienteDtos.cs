namespace Libreria.Api.DTOs;

// DTO de entrada: recibe datos del cliente; el Id lo genera la base de datos.
public class ClienteCrearDto
{
    public string Nombre { get; set; } = "";
    public string Email { get; set; } = "";
    public DateOnly FechaNacimiento { get; set; }
}

public record ClienteDto(int Id, string Nombre, string Email, DateOnly FechaNacimiento);
