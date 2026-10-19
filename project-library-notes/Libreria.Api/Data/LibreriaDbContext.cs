using Libreria.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Libreria.Api.Data;

// AddDbContext entrega un contexto scoped: una sesión de trabajo por petición.
public class LibreriaDbContext : DbContext
{
    public LibreriaDbContext(DbContextOptions<LibreriaDbContext> options) : base(options) { }

    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Editorial> Editoriales => Set<Editorial>();
    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var editorial = modelBuilder.Entity<Editorial>();
        editorial.Property(e => e.Nombre).HasMaxLength(100).IsRequired();

        var libro = modelBuilder.Entity<Libro>();
        libro.Property(l => l.Titulo).HasMaxLength(200).IsRequired();
        libro.Property(l => l.Isbn).HasMaxLength(13).IsRequired();
        libro.Property(l => l.Precio).HasPrecision(12, 2);
        // La BD protege también frente a dos peticiones que intentan insertar el mismo ISBN.
        libro.HasIndex(l => l.Isbn).IsUnique();
        libro.ToTable("Libros", tabla =>
            tabla.HasCheckConstraint("CK_Libros_Precio", "[Precio] > 0"));
        libro.HasOne(l => l.Editorial).WithMany(e => e.Libros)
            .HasForeignKey(l => l.EditorialId).OnDelete(DeleteBehavior.Restrict);

        var cliente = modelBuilder.Entity<Cliente>();
        cliente.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
        cliente.Property(c => c.Email).HasMaxLength(254).IsRequired();
        cliente.HasIndex(c => c.Email).IsUnique();
    }
}
