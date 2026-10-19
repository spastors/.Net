using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Libreria.Api.Examples;

// Elimina estos controladores de las rutas y de OpenAPI fuera de Development.
public sealed class OcultarEjemplosConvention : IApplicationModelConvention
{
    public void Apply(ApplicationModel application)
    {
        var ejemplos = application.Controllers
            .Where(c => c.ControllerType.Namespace == typeof(OcultarEjemplosConvention).Namespace)
            .ToList();
        foreach (var controlador in ejemplos)
            application.Controllers.Remove(controlador);
    }
}
