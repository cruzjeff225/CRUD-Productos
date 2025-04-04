using Microsoft.EntityFrameworkCore;
using CRUD_Productos.Models;

namespace CRUD_Productos.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Relación: Categoría 1 - M Productos
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaID);

            //Relación: Proveedores 1 - M Productos
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Proveedores)
                .WithMany(prov => prov.Productos)
                .HasForeignKey(p => p.ProveedorID);

            //Relación: Inventario 1 - 1 Productos
            modelBuilder.Entity<Inventario>()
                .HasOne(i => i.Producto)
                .WithOne(p => p.Inventario)
                .HasForeignKey<Inventario>(i => i.ProductoID);

        }
    }
}
