using Microsoft.EntityFrameworkCore;
using TP2ProgramacionWeb.Frontend.Data.Configuraciones;
using TP2ProgramacionWeb.Frontend.Models;

namespace TP2ProgramacionWeb.Frontend.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoriaConfiguracion());
            modelBuilder.ApplyConfiguration(new DetalleIngresoConfiguracion());
            modelBuilder.ApplyConfiguration(new ImagenConfiguracion());
            modelBuilder.ApplyConfiguration(new IngresoConfiguracion());
            modelBuilder.ApplyConfiguration(new ProductoConfiguracion());
            modelBuilder.ApplyConfiguration(new ProveedorConfiguracion());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracion());
            modelBuilder.ApplyConfiguration(new UsuarioConfiguracion());


            // https://codewithmukesh.com/blog/global-query-filters-efcore/ Para no tener que poner si está o no eliminado. Esto lo tenemos que hacer con la mayoría de entidades.
            modelBuilder.Entity<Usuario>().HasQueryFilter(u => !u.Eliminado);
            modelBuilder.Entity<Proveedor>().HasQueryFilter(p => !p.Eliminado);
            modelBuilder.Entity<Producto>().HasQueryFilter(p => !p.Eliminado);


        }

        public DbSet<Categoria> Categorias{ get; set; }
        public DbSet<DetalleIngreso> DetallesIngresos { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
