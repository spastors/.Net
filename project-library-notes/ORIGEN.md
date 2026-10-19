# Origen y atribución

- **Proyecto de partida:** ejercicio ASP.NET Core facilitado por un profesor y conservado en `../../project-example/Backend`.
- **Autoría del ejercicio original:** corresponde a su profesor/autor. No se presenta como trabajo original de este repositorio.
- **Datos no disponibles:** nombre del autor, curso, enlace de origen y licencia. No se han deducido a partir de nombres de equipos, capturas o cadenas de conexión.
- **Adaptación:** dominio de librería, organización por capas, comentarios explicativos y correcciones para utilizarlo como material de consulta.
- No se modifica ni se elimina la atribución que pueda acompañar al original. Este archivo no concede una licencia sobre código ajeno.

## Correspondencia de ejemplos

| Ejercicio recibido | Adaptación |
|---|---|
| Beer / Brand | Libro / Editorial |
| BeerController y validadores | LibrosController, LibroService, validadores de DTO y repositorio |
| People / PeopleService | Clientes guardados en SQL Server; ejemplos de políticas en `Examples/` |
| PostsService y API remota | Cliente tipado de Open Library con configuración, cancelación y caché |
| OperationController | Cotizaciones con cálculo puro y validación |
| RandomController / servicios | Identificadores Guid para observar los ciclos de vida de DI |
| SomeController / esperas | Ejemplos aislados de espera síncrona, asíncrona y concurrente |
| StoreContext | LibreriaDbContext y base independiente LibreriaCurso |

## Cambios deliberados

- Separación de HTTP, reglas de negocio y acceso a datos.
- DTOs de entrada sin Id; el Id de actualización viene de la ruta.
- Asignación correcta de la editorial al editar un libro.
- Validación de números positivos y existencia de la editorial.
- Índices únicos para ISBN y correo; protección de relaciones en SQL Server.
- Errores uniformes mediante ProblemDetails.
- Sustitución de listas estáticas por persistencia en SQL Server.
- Registro único del cliente HTTP tipado.
- Migración nueva, sin reutilizar el historial ni la conexión de Store.
- Actualización de dependencias de la adaptación y comentarios orientados a Junior.
