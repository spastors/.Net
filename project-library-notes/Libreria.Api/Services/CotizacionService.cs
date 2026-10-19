using FluentValidation;
using Libreria.Api.DTOs;

namespace Libreria.Api.Services;

public interface ICotizacionService
{
    Task<CotizacionDto> CalcularAsync(CotizacionEntradaDto datos, CancellationToken ct);
}

public class CotizacionService : ICotizacionService
{
    private readonly IValidator<CotizacionEntradaDto> _validator;
    public CotizacionService(IValidator<CotizacionEntradaDto> validator) => _validator = validator;

    public async Task<CotizacionDto> CalcularAsync(CotizacionEntradaDto datos, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(datos, ct);
        return Calcular(datos);
    }

    // Función pura: misma entrada, mismo resultado; no lee BD ni modifica datos externos.
    private static CotizacionDto Calcular(CotizacionEntradaDto datos)
    {
        decimal subtotal = datos.PrecioUnitario * datos.Unidades;
        decimal descuento = decimal.Round(subtotal * datos.DescuentoPorcentaje / 100m,
            2, MidpointRounding.AwayFromZero);
        return new(subtotal, descuento, subtotal - descuento);
    }
}
