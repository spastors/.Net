# Comprobaciones de la adaptación

Fecha: **19-10-2026**. SDK utilizado: **10.0.401**. Comprobaciones manuales; no se añade un proyecto de pruebas.

| Comprobación | Resultado |
|---|---|
| Compilación | Correcta; cero errores y cero advertencias |
| Migración inicial | Aplicada correctamente en SQL Server LocalDB temporal |
| Crear editorial y libro | `201` y cabecera `Location` |
| Modificar editorial del libro | `200`; el GET refleja la nueva relación |
| Buscar libros con paginación | `200`; total y elementos correctos |
| Alta y búsqueda de clientes | Persistencia correcta; correo normalizado |
| ISBN/correo duplicados | `409` |
| Editorial con libros asociados | Borrado rechazado con `409` |
| Recurso inexistente | `404` |
| Editorial inexistente al crear libro | `400` |
| Borrar libro y editorial libre | `204` |
| DTO de libro inválido / ISBN externo inválido | `400` |
| Cotización: 15 × 2 con 10 % | Total `27` |
| Lectura de header y body | Valores recibidos correctamente |
| Esperas concurrentes | `200` |
| Ciclos de vida DI | Transient nuevo; scoped por petición; singleton compartido |
| Swagger y ejemplos en Production | `404` |
| Open Library: ISBN 9780140328721 | `200`: Fantastic Mr. Fox, Roald Dahl |

La instancia LocalDB temporal utilizada para comprobar el CRUD se detuvo y eliminó. No se aplicaron migraciones a Store ni a LibreriaCurso.

Para utilizar la base local de la aplicación, sigue el arranque indicado en [README.md](README.md). Las respuestas futuras de Open Library dependen del proveedor.

