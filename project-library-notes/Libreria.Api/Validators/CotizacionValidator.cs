using FluentValidation;
using Libreria.Api.DTOs;

namespace Libreria.Api.Validators;

// RuleFor selecciona un campo; las reglas encadenadas definen sus límites permitidos.
public class CotizacionValidator : AbstractValidator<CotizacionEntradaDto>
{
    public CotizacionValidator()
    {
        RuleFor(c => c.PrecioUnitario).GreaterThan(0).LessThanOrEqualTo(1_000_000m)
            .PrecisionScale(12, 2, true);
        RuleFor(c => c.Unidades).InclusiveBetween(1, 10000);
        RuleFor(c => c.DescuentoPorcentaje).InclusiveBetween(0, 100);
    }
}
