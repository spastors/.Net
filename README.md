# 📘 Guía de Iniciación a .NET (Backend)

> **Resumen:** Notas técnicas sobre el ecosistema .NET, sintaxis moderna de C\# y patrones para desarrollo backend.
> **Versión:** Enfocado en .NET 8 LTS y C\# 12.

## 1\. 🛠️ Configuración y Entorno

Para empezar a trabajar con el ecosistema moderno de Microsoft.

  * **SDK:** Descargar la versión LTS (Long Term Support) recomendada: [.NET Download](https://dotnet.microsoft.com/es-es/download/dotnet).
  * **IDE Recomendado:** Visual Studio 2022 o 2026 (Community) o VS Code con la extensión **C\# Dev Kit**. [Visual Studio Download](https://visualstudio.microsoft.com/es/downloads/)

### Comandos Esenciales (CLI)

La terminal es tu amiga en .NET Core/.NET 5+.

```bash
dotnet --version            # Verificar instalación
dotnet new list             # Ver plantillas disponibles
dotnet new webapi -n MyApi  # Crear una API Web
dotnet new console -n App   # Crear aplicación de consola
dotnet run                  # Ejecutar proyecto
dotnet build                # Compilar proyecto
```

## 2\. ⌨️ Sintaxis y Sistema de Tipos

C\# es un lenguaje **fuertemente tipado** (aunque tiene inferencia de tipos) y **compilado**.

### Fundamentos de Sintaxis y Control

La estructura básica de un programa es: `using` (importar bibliotecas), `namespace` (agrupador lógico), `class` (plantilla de objeto) y el punto de entrada, que en las aplicaciones modernas de Consola o Web API es implícito (**Top-Level Statements**).

  * **Datos Primitivos:** Cubiertos en el *Cheatsheet* (ej: `int`, `bool`, `char`).
  * **Funciones/Métodos:** Bloques de código reutilizables.
  * **Estructuras de Control:** `if/else`, `switch`, y bucles (`for`, `foreach`, `while`).

#### Cheatsheet de Tipos Básicos

```csharp
/*
   ┌─────────────────────────────────────────────────────────────┐
   │               C# TYPE SYSTEM CHEATSHEET 🧪                  │
   └─────────────────────────────────────────────────────────────┘
*/

// 1. NÚMEROS (NUMBERS)
int     entero      = 42;             // Estándar para contar (-2B a +2B)
long    gigante     = 9_000_000L;     // Entero de 64-bits (Nota el guion bajo y 'L')
double  cientifico  = 3.14159;        // Punto flotante estándar (Impreciso para $)
float   graficos    = 3.14f;          // Menor peso, usado en Unity/Juegos ('f')
decimal dinero      = 100.50m;        // ¡CRÍTICO! Alta precisión para finanzas ('m')

// 2. TEXTO (STRINGS)
char    letra       = 'A';            // Comillas simples, solo 1 carácter
string  texto       = "Hola Mundo";   // Comillas dobles estándar
string  json        = """             
                      {
                        "prop": "val" 
                      }
                      """;            // Raw String Literal (C# 11+): Ideal para JSON/XML multilínea

// 3. LÓGICA (BOOLEAN)
bool    esValido    = true;           // Ocupa 1 byte (teóricamente 1 byte)

// 4. FECHAS (DATES - MODERN .NET)
DateTime fechaHora  = DateTime.Now;             // 2025-10-05 14:30:00 (Pesado)
DateOnly soloFecha  = new DateOnly(2025,10,5);  // 2025-10-05 (Ligero - Recomendado si no usas hora)
TimeOnly soloHora   = new TimeOnly(14,30);      // 14:30 (Ligero)

// 5. IDENTIFICADORES & BYTES
Guid    idUnico     = Guid.NewGuid(); // a1b2c3d4-e5f6... (Ideal para Primary Keys)
byte    unByte      = 255;            // De 0 a 255 (Para manipulación de archivos/imágenes)
```

#### Declaración de Variables: `var` vs `dynamic` vs `const`

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

#### Strings: Interpolación y Verbatim

  * **Interpolación (`$`)**: Insertar variables directamente.
  * **Verbatim (`@`)**: Ignora caracteres de escape (útil para rutas de archivos).

<!-- end list -->

```csharp
string nombre = "Ana";
// Combinando ambos (muy útil en rutas o JSON a mano)
string ruta = $@"C:\Usuarios\{nombre}\Documentos"; 
```

## 💾 Tipos de Referencia vs. Tipos de Valor

Esta es la diferencia más importante de C\#: cómo se guardan los datos en la memoria y qué sucede cuando se copian.

### 1\. Tipos de Valor (Value Types: `int`, `bool`, `struct`) 🎁

Los Tipos de Valor son como una **caja que contiene su propio contenido**.

| Concepto | Explicación Simple | Analogía |
| :--- | :--- | :--- |
| **¿Qué guardan?** | El **dato** real. (Ej: el número 10, o el valor `true`). | Una **fotografía física**. |
| **¿Dónde viven?** | En el **Stack** (Pila). Piensa en esto como una estantería pequeña y súper rápida. | |
| **Al copiar...** | Se hace una **copia idéntica e independiente** del contenido. | Si copias la fotografía, tienes **dos fotos separadas**. Si dibujas en la copia, la original no se altera. |
| **Ejemplos** | Tipos primitivos (`int`, `decimal`, `bool`) y estructuras que defines con `struct`. | |

#### Ejemplo de Código (`struct`):

```csharp
public struct Coordenada { public int X; }

var original = new Coordenada { X = 10 };
var copia = original; // 🎁 Se copia el valor 10.
copia.X = 50;         // Modificamos SÓLO la copia.

Console.WriteLine($"Original: {original.X}"); // Output: 10 (No cambia)
Console.WriteLine($"Copia:    {copia.X}");    // Output: 50
```

### 2\. Tipos de Referencia (Reference Types: `class`, `array`) 🔑

Los Tipos de Referencia son como una **etiqueta que apunta a un objeto más grande**.

| Concepto | Explicación Simple | Analogía |
| :--- | :--- | :--- |
| **¿Qué guardan?** | La **dirección** o la "llave" del objeto que vive en otro lugar. | Una **llave de coche**. |
| **¿Dónde viven?** | En el **Heap** (Montón). Piensa en esto como un almacén grande y dinámico, gestionado por el Garbage Collector (GC). | |
| **Al copiar...** | Se hace una **copia de la dirección (la llave)**. Ambas variables apuntan al mismo objeto. | Si copias la llave, tienes **dos llaves** que abren el **mismo coche**. Si pintas el coche usando una llave, la otra llave también abrirá el coche pintado. |
| **Ejemplos** | Todas las clases (`class`), arrays, listas (`List<T>`). | |

#### Ejemplo de Código (`class`):

```csharp
public class Punto { public int X; }

var original = new Punto { X = 10 };
var referencia = original; // 🔑 Se copia la referencia (la dirección).
referencia.X = 50;         // Modificamos el objeto COCHE compartido.

Console.WriteLine($"Original: {original.X}"); // Output: 50 (¡SÍ cambia!)
Console.WriteLine($"Referencia: {referencia.X}"); // Output: 50
```

### 💡 Resumen Rápido (Entrevista)

> La diferencia principal es el **comportamiento de la copia**: ¿Se copia el **valor** real (independiente) o se copia la **dirección** (compartida)?

| Aspecto | Tipo de Valor (`struct`, `int`) | Tipo de Referencia (`class`) |
| :--- | :--- | :--- |
| **Memoria** | **Stack** (Rápido, estático) | **Heap** (Gestionado por el GC) |
| **Comportamiento** | **Independiente** (La copia no afecta al original) | **Compartido** (La copia SÍ afecta al original) |
| **Uso Ideal** | Objetos pequeños y rápidos (ej: Coordenadas, IDs) | Lógica de negocio, colecciones grandes, objetos complejos |


## 3\. 🧬 Programación Orientada a Objetos (POO)

### ✍️ Convenciones de Nomenclatura (Estándar C\#)

Seguir estas convenciones de estilo es crucial para el *Clean Code* y la consistencia en proyectos .NET:

| Elemento | Convención | Estilo | Ejemplo | Acceso Sugerido |
| :--- | :--- | :--- | :--- | :--- |
| **Clases, Métodos, Propiedades, Namespaces** | **PascalCase** | `Public Name` | `public string NombreUsuario { get; set; }` | `public` |
| **Parámetros de Métodos, Variables Locales** | **camelCase** | `private name` | `void Proceso(int idUsuario)` | `var` / local |
| **Campos Privados (Fields)** | **\_camelCase** | `private _name` | `private int _stockMinimo;` | `private` |
| **Interfaces** | **PascalCase Prefijo I** | `Interface Name` | `public interface IRepositorio;` | `public` |

### Anatomía de una Clase Típica y Colecciones

Una **clase** es la plantilla para crear objetos.

  * **Creación de Objeto:** A diferencia de JavaScript, donde a veces puedes omitir `new`, en C\# la instancia de una clase siempre requiere **`new`** (excepto en DI). `var object = new Class();`

<!-- end list -->

```csharp
public class Producto
{
    // 1. Campo (Field) Variable interna de la clase, típicamente privada con convención _camelCase
    private int _stockMinimo = 5;

    // 2. Propiedad (Property): Encapsula el acceso al campo, controlando su lectura/escritura (pública con PascalCase)
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
}
```

#### Colecciones Básicas

Las colecciones te permiten agrupar datos:

  * **`array`:** Colección de tamaño fijo y del mismo tipo. `int[] numeros = new int[5];`
  * **`List<T>`:** Colección dinámica (genérica) del mismo tipo.
  * **`Dictionary<TKey, TValue>`:** Colección de pares clave-valor (genérica).
  * **`HashSet<T>`:** Colección sin orden y que garantiza que **no hay duplicados**.
  * **`Tuple`:** Tipo de valor que permite agrupar varios elementos relacionados de diferentes tipos. `(int id, string name) user = (1, "Anna");`

### Modificadores de Acceso y Sobrecarga

| Modificador | Acceso | Descripción |
| :--- | :--- | :--- |
| **`public`** | Ilimitado | Accesible desde cualquier parte del código. |
| **`private`** | Limitado | Solo accesible **dentro de la misma clase**. (Recomendado para campos internos). |
| **`protected`** | Limitado | Accesible dentro de la misma clase y por las **clases hijas (heredadas)**. |

#### Ejemplos de Modificadores de Acceso

```csharp
public class CuentaBancaria
{
    // 1. private: Solo accesible dentro de esta clase (Encapsulación).
    private decimal _saldo = 1000m; 

    // 2. protected: Accesible aquí y en clases que hereden de CuentaBancaria.
    protected decimal Interes = 0.05m; 

    // 3. public: Accesible desde cualquier lugar.
    public string NumeroCuenta { get; set; }

    // Método público que interactúa con el campo privado
    public decimal ObtenerSaldo() 
    {
        return _saldo;
    }
}

public class CuentaAhorro : CuentaBancaria
{
    public void CalcularInteres()
    {
        // ✅ Acceso permitido a 'protected' (Interes), porque es una clase hija.
        decimal nuevoInteres = this.ObtenerSaldo() * this.Interes;
        
        // ❌ Error de compilación si intentamos acceder a '_saldo' directamente,
        // porque '_saldo' es 'private' en la clase base.
    }
}

// Uso desde fuera de las clases:
var cuenta = new CuentaBancaria();
cuenta.NumeroCuenta = "12345";       // ✅ OK: Es public
// cuenta._saldo = 500m;             // ❌ Error: Es private
// decimal interes = cuenta.Interes; // ❌ Error: Es protected
```

### Sobrecarga de Métodos (Overloading)

La sobrecarga permite definir **múltiples métodos con el mismo nombre** dentro de una clase, siempre y cuando tengan **diferentes firmas**. La firma se define por el **número** o el **tipo** de sus parámetros.

> **Importante:** El tipo de retorno del método **no** es suficiente para diferenciar una sobrecarga.

#### Ejemplo de Sobrecarga

```csharp
public class Calculadora
{
    // Sobrecarga 1: Suma de dos enteros
    public int Sumar(int a, int b) 
    {
        return a + b;
    }

    // Sobrecarga 2: Suma de tres enteros (diferente número de parámetros)
    public int Sumar(int a, int b, int c) 
    {
        return a + b + c;
    }

    // Sobrecarga 3: Suma de dos decimales (diferente tipo de parámetros)
    public decimal Sumar(decimal a, decimal b) 
    {
        return a + b;
    }
}

// Uso: El compilador elige automáticamente qué método usar
var calc = new Calculadora();
var resultadoInt = calc.Sumar(5, 10);        // Llama a Sobrecarga 1
var resultadoDecimal = calc.Sumar(5.5m, 10m);  // Llama a Sobrecarga 3
```

### Clases y Herencia: `virtual` vs `override`

La **Herencia** permite que una clase hija adquiera las propiedades y métodos de una clase padre. Los conceptos `virtual` y `override` son la base del **Polimorfismo**.

| Palabra Clave | Uso | Propósito |
| :--- | :--- | :--- |
| **`virtual`** | Se define en el **método de la clase padre**. | Permite que este comportamiento **pueda ser modificado** (sobrescrito) por las clases que hereden de ella. |
| **`override`** | Se define en el **método de la clase hija**. | **Reemplaza** el comportamiento definido en la clase padre. |
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
## 4\. 🔗 Datos, Serialización y Funcional

### Serialización y Deserialización

Es el proceso de **convertir un objeto C\#** (como una instancia de una clase) **en un formato de transmisión** (como JSON o XML) y viceversa. Esto es fundamental para las API Web.

  * **Serialización:** `Objeto C#` $\rightarrow$ `JSON/XML` (Para enviar datos a un cliente).
  * **Deserialización:** `JSON/XML` $\rightarrow$ `Objeto C#` (Para recibir datos del cliente).

La biblioteca estándar para esto en .NET es **`System.Text.Json`**.

```csharp
// Serialización a JSON
var producto = new Producto("Teclado");
string json = System.Text.Json.JsonSerializer.Serialize(producto);
// Output: {"Nombre":"Teclado"}
```

### 🚀 Funciones de Primera Clase y Delegados (Callbacks)

> **Concepto Clave:** En C\#, las funciones pueden tratarse como variables. Esto permite pasar lógica como parámetro (Callbacks).

#### 1\. Action (Void)

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

#### 2\. Func (Return)

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

#### 3\. Lambda Functions (`=>`)

Es la sintaxis concisa para crear delegados (`Action` o `Func`) en línea.

```csharp
// Lógica que recibe string y devuelve string (Func)
Func<string, string> aMayusculas = (texto) => texto.ToUpper(); 

// Lógica que recibe string y no devuelve nada (Action)
Action<string> logger = (msg) => Console.WriteLine($"LOG: {msg}");
```

## 5\. 🔍 Generics y Colecciones

Los **Genéricos (`<T>`)** nos permiten escribir código reutilizable y seguro sin especificar el tipo de dato exacto hasta el momento de usarlo.

> **¿Por qué usarlos?** Evitan el "Boxing/Unboxing" (coste de rendimiento) y garantizan que no metas un `int` en una lista de `string` (seguridad de tipos).

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
