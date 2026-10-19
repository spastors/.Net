namespace Libreria.Api.Examples;

public interface IPoliticaNombre
{
    bool EsValido(string nombre);
}

// Dos implementaciones del mismo contrato: una clave permite elegir entre ellas.
public class NombreBasico : IPoliticaNombre
{
    public bool EsValido(string nombre) => !string.IsNullOrWhiteSpace(nombre);
}

public class NombreLargo : IPoliticaNombre
{
    public bool EsValido(string nombre) => nombre.Trim().Length >= 5;
}
