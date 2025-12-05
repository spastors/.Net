# 📘 Guía de Iniciación a .NET

> **Resumen:** Notas técnicas sobre el ecosistema .NET, sintaxis moderna de C\# y patrones para desarrollo backend.
> **Versión:** Enfocado en .NET 9 (LTS) y C\# 13.

## 1\. 🛠️ Configuración y Entorno

Para empezar a trabajar con el ecosistema moderno de Microsoft.

  * **SDK:** Descargar la versión LTS (Long Term Support) recomendada: [.NET Download](https://dotnet.microsoft.com/es-es/download/dotnet).
  * **IDE Recomendado:** Visual Studio 2026 (Community) o VS Code con la extensión **C\# Dev Kit**. [Visual Studio Download](https://visualstudio.microsoft.com/es/downloads/)

### Comandos Esenciales (CLI)

La terminal es tu amiga en .NET Core/.NET 5+.

```bash
dotnet --version            # Verificar instalación
dotnet new list             # Ver plantillas disponibles
dotnet new webapi -n MyApi  # Crear una API Web
dotnet new console -n App   # Crear aplicación de consola
dotnet run                  # Ejecutar proyecto
dotnet build                # Compilar proyecto
```

## 2\. ⌨️ Sintaxis y Sistema de Tipos

C\# es un lenguaje **fuertemente tipado** (aunque tiene inferencia de tipos) y **compilado**.

```markdown
### 🧬 C# 13 Cheat Sheet: Tipos de Datos Básicos

| Categoría | Alias C# | Tipo .NET | Sufijo | Ejemplo de Uso | Notas |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **Enteros** | `int` | `Int32` | - | `var n = 42;` | El estándar por defecto. |
| | `long` | `Int64` | `L` | `var n = 900000L;` | Números muy grandes. |
| **Flotantes** | `double` | `Double` | `d` | `var n = 3.14;` | Por defecto para decimales. Rápido, menos preciso. |
| | `float` | `Single` | `f` | `var n = 3.14f;` | Menor precisión, usado en gráficos/Unity. |
| | `decimal` | `Decimal` | `m` | `var n = 19.99m;` | **Obligatorio para dinero/finanzas**. Alta precisión. |
| **Texto** | `string` | `String` | - | `"Hola"` | Cadena de caracteres inmutable. |
| | `char` | `Char` | - | `'A'` | Un solo carácter (comillas simples). |
| **Lógica** | `bool` | `Boolean` | - | `true` / `false` | Solo dos valores posibles. |
| **Fechas** | `DateTime` | `DateTime` | - | `DateTime.Now` | Fecha + Hora. |
| *(Moderno)* | `DateOnly` | `DateOnly` | - | `new DateOnly(2025,1,1)` | Solo fecha (sin hora). Más eficiente. |
| **Otros** | `object` | `Object` | - | - | La clase base de **todo** en .NET. |
| | `Guid` | `Guid` | - | `Guid.NewGuid()` | Identificador único global. |
```

```csharp
/* ┌─────────────────────────────────────────────────────────────┐
   │               C# TYPE SYSTEM CHEATSHEET 🧪                  │
   └─────────────────────────────────────────────────────────────┘
*/

// 1. NÚMEROS (NUMBERS)
int     entero      = 42;             // Estándar para contar (-2B a +2B)
long    gigante     = 9_000_000L;     // Entero de 64-bits (Nota el guion bajo y 'L')
double  cientifico  = 3.14159;        // Punto flotante estándar (Impreciso para $)
float   graficos    = 3.14f;          // Menor peso, usado en Unity/Juegos ('f')
decimal dinero      = 100.50m;        // ¡CRÍTICO! Alta precisión para finanzas ('m')

// 2. TEXTO (STRINGS)
char    letra       = 'A';            // Comillas simples, solo 1 carácter
string  texto       = "Hola Mundo";   // Comillas dobles estándar
string  json        = """             
                      {
                        "prop": "val" 
                      }
                      """;            // Raw String Literal (C# 11+): Ideal para JSON/XML multilínea

// 3. LÓGICA (BOOLEAN)
bool    esValido    = true;           // Ocupa 1 byte (teóricamente 1 bit)

// 4. FECHAS (DATES - MODERN .NET)
DateTime fechaHora  = DateTime.Now;             // 2025-10-05 14:30:00 (Pesado)
DateOnly soloFecha  = new DateOnly(2025,10,5);  // 2025-10-05 (Ligero - Recomendado si no usas hora)
TimeOnly soloHora   = new TimeOnly(14,30);      // 14:30 (Ligero)

// 5. IDENTIFICADORES & BYTES
Guid    idUnico     = Guid.NewGuid(); // a1b2c3d4-e5f6... (Ideal para Primary Keys)
byte    unByte      = 255;            // De 0 a 255 (Para manipulación de archivos/imágenes)
```

### Declaración de Variables: `var` vs `dynamic` vs `const`

Esta es una pregunta clásica de entrevista.

  * **`var`**: Inferencia de tipo en **tiempo de compilación**. El compilador "adivina" el tipo y **no cambia**. Es lo estándar hoy en día.
  * **`const`**: Valor inmutable. Debe asignarse al declararse.
  * **`dynamic`**: Se resuelve en **tiempo de ejecución**. Evita el chequeo de tipos del compilador (usar con cuidado, pierdes seguridad).

<!-- end list -->

```csharp
// ✅ Recomendado: El compilador sabe que es int
var edad = 25; 

// 🔒 Constante: Nunca cambiará
const double PI = 3.1416; 

// ⚠️ Peligroso: Si te equivocas, falla cuando el programa corre, no al compilar
dynamic datos = "Hola"; 
datos = 10; // Esto es válido en dynamic, pero daría error en var
```

### Strings: Interpolación y Verbatim

  * **Interpolación (`$`)**: Insertar variables directamente.
  * **Verbatim (`@`)**: Ignora caracteres de escape (útil para rutas de archivos).

<!-- end list -->

```csharp
string nombre = "Ana";
// Combinando ambos (muy útil en rutas o JSON a mano)
string ruta = $@"C:\Usuarios\{nombre}\Documentos"; 
```

### 💾 Tipos de Referencia vs. Tipos de Valor (`struct`)

Este es un concepto **fundamental** en C\# que afecta directamente cómo el programa gestiona la memoria y cómo se comportan tus datos al ser pasados entre métodos. 

#### 1\. Tipos de Valor (Value Types: `struct`, `int`, `bool`, `enum`)

Los Tipos de Valor contienen directamente su información. Cuando los pasas o los copias, copias el **valor** real.

##### Características:

  * **Ubicación en Memoria:** Se almacenan en el **Stack** (Pila). El Stack es una región de memoria rápida y bien organizada.
  * **Comportamiento al Copiar:** Al copiar una variable de valor, creas una **copia totalmente independiente** de los datos. Cambiar la copia no afecta al original.
  * **Ejemplos Comunes:** Los tipos primitivos (`int`, `bool`, `char`, `float`, `decimal`) son estructuras (structs) en C\# y se comportan como tipos de valor. La estructura `struct` te permite crear tus propios tipos de valor.

##### 📝 Analogía de la Copia:

Imagina que copias un **billete de $10**. Tienes dos billetes de $10. Si quemas tu copia (la modificas), el billete original sigue intacto.

##### Ejemplo de Código (`struct`):

```csharp
public struct Coordenada
{
    public int X;
    public int Y;
}

var puntoA = new Coordenada { X = 10, Y = 20 };
var puntoB = puntoA; // Se copia el VALOR (10, 20)
puntoB.X = 50;       // Solo cambiamos la copia (puntoB)

Console.WriteLine($"A: {puntoA.X}"); // Output: A: 10 (El original no cambia)
Console.WriteLine($"B: {puntoB.X}"); // Output: B: 50
```

#### 2\. Tipos de Referencia (Reference Types: `class`, `string`, `array`)

Los Tipos de Referencia no contienen los datos directamente; contienen una **referencia** (una dirección de memoria) que apunta a dónde están los datos reales.

##### Características:

  * **Ubicación en Memoria:** Se almacenan en el **Heap** (Montón). El Heap es una región de memoria dinámica, más lenta de gestionar que el Stack, y es gestionada por el **Garbage Collector (GC)**.
  * **Comportamiento al Copiar:** Al copiar una variable de referencia, copias la **dirección** (la referencia), no los datos. Ambas variables apuntan al *mismo* objeto en el Heap.
  * **Ejemplos Comunes:** Todas las clases (`class`), las cadenas (`string`) y los arrays (aunque los strings son inmutables, se comportan como referencia).

#### 📝 Analogía de la Copia:

Imagina que copias la **dirección de tu casa** en dos sobres. Tienes dos sobres (variables), pero ambos apuntan a la *misma casa*. Si entras por el sobre 1 y pintas la pared (modificas el objeto), si luego entras por el sobre 2, verás la pared pintada.

#### Ejemplo de Código (`class`):

```csharp
public class Punto
{
    public int X;
    public int Y;
}

var puntoA = new Punto { X = 10, Y = 20 };
var puntoB = puntoA; // Se copia la REFERENCIA (ambos apuntan al mismo objeto)
puntoB.X = 50;       // Cambiamos el objeto compartido

Console.WriteLine($"A: {puntoA.X}"); // Output: A: 50 (El original SÍ cambia)
Console.WriteLine($"B: {puntoB.X}"); // Output: B: 50
```

## 3\. 🧬 Programación Orientada a Objetos (POO)

### Clases y Herencia

C\# utiliza la convención **PascalCase** para clases y métodos (`NombreMetodo`) y **camelCase** para variables locales (`miVariable`).

C\# se basa fuertemente en el paradigma POO. Es vital entender cómo se definen las estructuras de datos y cómo interactúan.

### Anatomía de una Clase Típica

Una **clase** es la plantilla para crear objetos. Debe seguir la convención **PascalCase** (`NombreClase`, `NombreMetodo`).

```csharp
public class Producto
{
    // 1. Campo (Field): Variable interna de la clase, típicamente privada.
    private int _stockMinimo = 5;

    // 2. Propiedad (Property): Encapsula el acceso al campo, controlando su lectura/escritura.
    //    Usando 'auto-implemented property' (lo más común):
    public string Nombre { get; set; }

    // 3. Constructor: Método especial que se ejecuta al crear el objeto (con 'new').
    public Producto(string nombreInicial)
    {
        Nombre = nombreInicial;
    }

    // 4. Método (Method): Define la acción o comportamiento del objeto.
    public void AñadirStock(int cantidad)
    {
        // Lógica de negocio
        if (cantidad < 0) return;
        
        Console.WriteLine($"Añadiendo {cantidad} unidades a {Nombre}.");
    }
}

// Uso:
var lapiz = new Producto("Lápiz HB"); 
lapiz.AñadirStock(10);
```

### Clases y Herencia: `virtual` vs `override`

La **Herencia** permite que una clase hija adquiera las propiedades y métodos de una clase padre. Los conceptos `virtual` y `override` son la base del **Polimorfismo** y son preguntas garantizadas en cualquier entrevista.

Para permitir que un método sea modificado por una clase hija, usamos `virtual` y `override`.

```markdown
| Palabra Clave | Uso | Propósito |
| :--- | :--- | :--- |
| **`virtual`** | Se define en el **método de la clase padre**. | Permite que este comportamiento **pueda ser modificado** (sobrescrito) por las clases que hereden de ella. |
| **`override`** | Se define en el **método de la clase hija**. | **Reemplaza** el comportamiento definido en la clase padre. |
```

#### Ejemplo de Código:

```csharp
public class Empleado
{
    // Este método es virtual, la clase hija PUEDE cambiarlo.
    public virtual string CalcularSalario() 
    {
        return "Salario base según contrato.";
    }
}

public class Manager : Empleado 
{
    // Usamos override para cambiar el comportamiento del padre.
    public override string CalcularSalario() 
    {
        return base.CalcularSalario() + " Más bonificación por objetivos de equipo.";
        // 'base' llama a la implementación original del padre.
    }
}

// Uso:
Empleado e = new Empleado();
Manager m = new Manager();

Console.WriteLine(e.CalcularSalario()); // Salario base según contrato.
Console.WriteLine(m.CalcularSalario()); // Salario base según contrato. Más bonificación...
```

### Interfaces

Definen un **contrato**. Una interfaz solo contiene las firmas de los métodos, propiedades, o eventos, pero **no su implementación**. La clase que implementa la interfaz *debe* proporcionar el código para esos miembros.

> **Importancia en Backend:** Las interfaces son vitales para la **Inyección de Dependencias (DI)**, ya que permiten trabajar con el *contrato* (`IProducto`) en lugar de la *implementación concreta* (`ProductoServicio`), facilitando los *mocks* en pruebas unitarias.

```csharp
public interface IGuardable
{
    // Define el contrato: debe haber un método Guardar sin implementación.
    void Guardar(string datos);
}

// La clase concreta implementa el contrato
public class RepositorioSql : IGuardable
{
    public void Guardar(string datos)
    {
        Console.WriteLine($"Guardando '{datos}' en la base de datos SQL.");
    }
}
```

## 4\. 🚀 Funciones de Primera Clase y Delegados (Callbacks)

> **Concepto Clave:** En C\#, las funciones pueden tratarse como variables. Esto permite pasar lógica como parámetro (Callbacks).

### 1\. Action (Void)

Delegado que **NO retorna valor** (void).

  * *Uso:* Cuando quieres ejecutar algo (un log, una notificación) sin esperar respuesta.

<!-- end list -->

```csharp
// Recibe un string, no devuelve nada
void ProcesarDatos(Action<string> fnCallback, string mensaje) 
{
    Console.WriteLine("Inicio del proceso...");
    fnCallback(mensaje); // Ejecutamos la función que nos pasaron
    Console.WriteLine("Fin del proceso.");
}

// Uso:
var miLogger = (string msg) => Console.WriteLine($"LOG: {msg}");
ProcesarDatos(miLogger, "Usuario registrado");
```

### 2\. Func (Return)

Delegado que **SÍ retorna valor**.

  * `Func<Entrada, Salida>`: El último tipo es siempre lo que devuelve.
  * *Uso:* Transformar datos, cálculos, LINQ.

<!-- end list -->

```csharp
// Recibe string, devuelve string transformado
void Transformar(Func<string, string> fnTransformacion, string texto)
{
    string resultado = fnTransformacion(texto);
    Console.WriteLine($"Resultado: {resultado}");
}

// Uso:
var aMayusculas = (string s) => s.ToUpper();
Transformar(aMayusculas, "hola mundo"); // Imprime: HOLA MUNDO
```

## 5\. 🔍 Generics y Colecciones

Los **Genéricos (`<T>`)** nos permiten escribir código reutilizable y seguro sin especificar el tipo de dato exacto hasta el momento de usarlo.

> **¿Por qué usarlos?** Evitan el "Boxing/Unboxing" (coste de rendimiento) y garantizan que no metas un `int` en una lista de `string`.

```csharp
// Lista genérica: Solo acepta enteros
List<int> numeros = new List<int>(); 

// Método genérico
public T DevolverElemento<T>(T elemento)
{
    return elemento;
}
```

## 6\. 📊 LINQ (Language Integrated Query)

La herramienta más potente de .NET para manipular datos (Arrays, Listas, Bases de Datos).

Existen dos sintaxis:

1.  **Query Syntax (Parecido a SQL):** Más legible para consultas complejas.
2.  **Method Syntax (Lambdas):** Más común, conciso y potente.

<!-- end list -->

```csharp
var numeros = new List<int> { 1, 5, 8, 10, 3 };

// 1. Method Syntax (Más usada en el día a día)
var mayoresDeCinco = numeros.Where(n => n > 5).ToList();

// 2. Query Syntax
var query = from n in numeros
            where n > 5
            select n;
```
