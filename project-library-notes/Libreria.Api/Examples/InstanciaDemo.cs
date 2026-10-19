namespace Libreria.Api.Examples;

public interface IInstanciaDemo
{
    Guid Id { get; }
}

// Cada objeto recibe un Guid distinto. Sirve para ver cuándo DI reutiliza una instancia.
public sealed class InstanciaDemo : IInstanciaDemo
{
    public Guid Id { get; } = Guid.NewGuid();
}
