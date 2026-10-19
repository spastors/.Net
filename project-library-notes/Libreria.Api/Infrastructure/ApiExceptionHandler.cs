using FluentValidation;
using Libreria.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Api.Infrastructure;

// Un único lugar convierte errores de aplicación en respuestas HTTP.
public sealed class ApiExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ApiExceptionHandler> _logger;
    public ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) => _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
            return false; // La petición ya fue cancelada: no intentamos escribir una respuesta.

        int status;
        string titulo;
        string detalle;
        if (exception is ValidationException validacion)
        {
            var errores = validacion.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).Distinct().ToArray());
            var problemaValidacion = new ValidationProblemDetails(errores)
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Revisa los datos enviados.",
                Instance = context.Request.Path
            };
            problemaValidacion.Extensions["traceId"] = context.TraceIdentifier;
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(problemaValidacion,
                options: (System.Text.Json.JsonSerializerOptions?)null,
                contentType: "application/problem+json", cancellationToken: cancellationToken);
            return true;
        }

        // Las excepciones del servicio no dependen de HTTP; aquí elegimos su código de respuesta.
        (status, titulo, detalle) = exception switch
        {
            RecursoNoEncontradoException => (404, "Recurso no encontrado.", exception.Message),
            ConflictoException => (409, "Conflicto con los datos existentes.", exception.Message),
            ReglaNegocioException => (400, "Operación no válida.", exception.Message),
            // SQL Server comprueba unicidad y relaciones incluso ante escrituras simultáneas.
            DbUpdateException { InnerException: SqlException { Number: 2601 or 2627 or 547 } }
                => (409, "Conflicto al guardar.", "El registro está duplicado o tiene relaciones incompatibles."),
            HttpRequestException => (502, "Error del catálogo externo.", "No se pudo obtener una respuesta válida de Open Library."),
            TimeoutException => (504, "Tiempo de espera agotado.", "Open Library no respondió a tiempo."),
            _ => (500, "Error interno.", "No se pudo completar la operación.")
        };

        if (status >= 500)
            _logger.LogError(exception, "Error en {Ruta}. TraceId: {TraceId}",
                context.Request.Path, context.TraceIdentifier);

        // Nunca devolvemos conexión SQL, stack trace ni mensajes internos del proveedor.
        var problema = new ProblemDetails
        {
            Status = status, Title = titulo, Detail = detalle, Instance = context.Request.Path
        };
        problema.Extensions["traceId"] = context.TraceIdentifier;
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problema,
            options: (System.Text.Json.JsonSerializerOptions?)null,
            contentType: "application/problem+json", cancellationToken: cancellationToken);
        return true;
    }
}
