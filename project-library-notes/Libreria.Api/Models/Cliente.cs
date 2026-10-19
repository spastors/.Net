namespace Libreria.Api.Models;

// Datos persistidos del cliente; los DTO controlan lo que se recibe y devuelve.
public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Email { get; set; } = "";
    public DateOnly FechaNacimiento { get; set; }
}
