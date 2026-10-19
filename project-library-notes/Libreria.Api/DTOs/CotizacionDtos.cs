namespace Libreria.Api.DTOs;

// Entrada del cálculo: precio, cantidad y descuento; no representa una tabla.
public class CotizacionEntradaDto
{
    public decimal PrecioUnitario { get; set; }
    public int Unidades { get; set; }
    public decimal DescuentoPorcentaje { get; set; }
}

public record CotizacionDto(decimal Subtotal, decimal Descuento, decimal Total);
