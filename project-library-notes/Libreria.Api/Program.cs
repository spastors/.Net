using System.Globalization;
using FluentValidation;
using Libreria.Api.Data;
using Libreria.Api.DTOs;
using Libreria.Api.Examples;
using Libreria.Api.Infrastructure;
using Libreria.Api.Repositories;
using Libreria.Api.Services;
using Libreria.Api.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// appsettings.json aporta valores por defecto; las variables de entorno pueden sobrescribirlos.
var conexion = builder.Configuration.GetConnectionString("Libreria")
    ?? throw new InvalidOperationException("Falta ConnectionStrings:Libreria.");
builder.Services.AddDbContext<LibreriaDbContext>(options => options.UseSqlServer(conexion));

// Scoped: todos los repositorios de una petición comparten el mismo DbContext.
builder.Services.AddScoped<ILibroRepository, LibroRepository>();
builder.Services.AddScoped<IEditorialRepository, EditorialRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ILibroService, LibroService>();
builder.Services.AddScoped<IEditorialService, EditorialService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ICotizacionService, CotizacionService>();

// Registro explícito para ver qué validator corresponde a cada DTO.
// Se invocan desde los servicios, sin validación automática síncrona de FluentValidation MVC.
ValidatorOptions.Global.LanguageManager.Culture = CultureInfo.GetCultureInfo("es");
builder.Services.AddScoped<IValidator<LibroCrearDto>, LibroCrearValidator>();
builder.Services.AddScoped<IValidator<LibroActualizarDto>, LibroActualizarValidator>();
builder.Services.AddScoped<IValidator<EditorialEntradaDto>, EditorialValidator>();
builder.Services.AddScoped<IValidator<ClienteCrearDto>, ClienteValidator>();
builder.Services.AddScoped<IValidator<CotizacionEntradaDto>, CotizacionValidator>();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient<ICatalogoExternoService, CatalogoExternoService>(http =>
{
    // La barra final mantiene correctamente las rutas relativas como "api/books".
    var url = builder.Configuration["OpenLibrary:BaseUrl"]
        ?? throw new InvalidOperationException("Falta OpenLibrary:BaseUrl.");
    http.BaseAddress = new Uri(url);
    int segundos = builder.Configuration.GetValue<int?>("OpenLibrary:TimeoutSeconds") ?? 10;
    http.Timeout = TimeSpan.FromSeconds(segundos);
    http.DefaultRequestHeaders.UserAgent.ParseAdd("LibreriaCurso/1.0");
});
// AddHttpClient ya registra el cliente tipado. No añadir otro AddScoped para ese contrato.

// Registros pedagógicos con clave. Los Guid permiten observar los ciclos de vida.
builder.Services.AddKeyedTransient<IInstanciaDemo, InstanciaDemo>("transient");
builder.Services.AddKeyedScoped<IInstanciaDemo, InstanciaDemo>("scoped");
builder.Services.AddKeyedSingleton<IInstanciaDemo, InstanciaDemo>("singleton");
builder.Services.AddKeyedTransient<IPoliticaNombre, NombreBasico>("basico");
builder.Services.AddKeyedTransient<IPoliticaNombre, NombreLargo>("largo");

builder.Services.AddControllers(options =>
{
    if (!builder.Environment.IsDevelopment())
        options.Conventions.Add(new OcultarEjemplosConvention());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

var app = builder.Build();

// Convierte excepciones en ProblemDetails. Va antes de los componentes que pueden fallar.
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// La BD se crea con "dotnet ef database update"; no se modifica al iniciar la API.
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
