namespace Libreria.Api.DTOs;

// Alta y edición comparten los mismos campos; el Id nunca llega en el body.
public class EditorialEntradaDto
{
    public string Nombre { get; set; } = "";
}

public record EditorialDto(int Id, string Nombre);
