namespace Libreria.Api.Validators;

// ISBN-13: 13 dígitos y suma ponderada divisible por 10 (incluye el dígito de control).
public static class Isbn13
{
    public static bool EsValido(string? valor)
    {
        if (valor is null || valor.Length != 13 || !valor.All(char.IsAsciiDigit)
            || !(valor.StartsWith("978", StringComparison.Ordinal) || valor.StartsWith("979", StringComparison.Ordinal)))
            return false;

        int suma = 0;
        for (int i = 0; i < valor.Length; i++)
            suma += (valor[i] - '0') * (i % 2 == 0 ? 1 : 3);
        return suma % 10 == 0;
    }
}
