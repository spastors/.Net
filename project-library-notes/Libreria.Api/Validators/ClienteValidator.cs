using FluentValidation;
using Libreria.Api.DTOs;

namespace Libreria.Api.Validators;

public class ClienteValidator : AbstractValidator<ClienteCrearDto>
{
    public ClienteValidator()
    {
        RuleFor(c => c.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(254);
        // La lambda obtiene la fecha actual al validar, no solo al crear el validador.
        RuleFor(c => c.FechaNacimiento)
            .Must(fecha => fecha != default && fecha <= DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Indica una fecha de nacimiento válida, no futura.");
    }
}
