using FluentValidation;
using Libreria.Api.DTOs;

namespace Libreria.Api.Validators;

// Un nombre debe tener contenido y caber en la columna SQL configurada para la entidad.
public class EditorialValidator : AbstractValidator<EditorialEntradaDto>
{
    public EditorialValidator()
    {
        RuleFor(e => e.Nombre).NotEmpty().MaximumLength(100);
    }
}
