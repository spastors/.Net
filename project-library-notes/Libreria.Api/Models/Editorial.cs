namespace Libreria.Api.Models;

// Una editorial publica muchos libros. La FK se configura en LibreriaDbContext.
public class Editorial
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
