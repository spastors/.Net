using FluentValidation;
using Libreria.Api.DTOs;

namespace Libreria.Api.Validators;

// Reutilizamos las reglas de entrada en creación y actualización.
public class LibroCrearValidator : AbstractValidator<LibroCrearDto>
{
    public LibroCrearValidator()
    {
        RuleFor(l => l.Titulo).NotEmpty().MaximumLength(200);
        RuleFor(l => l.Isbn).Must(Isbn13.EsValido)
            .WithMessage("El ISBN-13 requiere prefijo 978/979, 13 dígitos y un dígito de control válido.");
        RuleFor(l => l.Precio).GreaterThan(0).WithMessage("El precio debe ser mayor que cero.")
            .LessThanOrEqualTo(9999999999.99m)
            .PrecisionScale(12, 2, true);
        RuleFor(l => l.EditorialId).GreaterThan(0);
        // Que la editorial exista se comprueba en el servicio, consultando el repositorio.
    }
}

public class LibroActualizarValidator : AbstractValidator<LibroActualizarDto>
{
    public LibroActualizarValidator()
    {
        Include(new LibroCrearValidator());
    }
}
