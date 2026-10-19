namespace Libreria.Api.Services;

// El servicio expresa lo ocurrido sin depender de MVC ni de códigos HTTP.
public class RecursoNoEncontradoException(string mensaje) : Exception(mensaje) { }
public class ConflictoException(string mensaje) : Exception(mensaje) { }
public class ReglaNegocioException(string mensaje) : Exception(mensaje) { }
