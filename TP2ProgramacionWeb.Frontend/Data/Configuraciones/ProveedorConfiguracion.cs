using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TP2ProgramacionWeb.Frontend.Models;

namespace TP2ProgramacionWeb.Frontend.Data.Configuraciones
{
    public class ProveedorConfiguracion : IEntityTypeConfiguration<Proveedor>
    {
        public void Configure(EntityTypeBuilder<Proveedor> builder)
        {
            builder.ToTable("Proveedores");

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.CUIT)
                .IsUnique();
            
            builder.HasIndex(x => x.Email)
                .IsUnique();
            
            builder.Property(x => x.RazonSocial)
                .IsRequired()
                .HasMaxLength(30);
            
            builder.Property(x => x.CUIT)
                .IsRequired()
                .HasMaxLength(13);
            
            builder.Property(x => x.Telefono)
                .IsRequired()
                .HasMaxLength(13);
            
            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(320);
            
            builder.Property(x => x.Direccion)
                .IsRequired()
                .HasMaxLength(40);

        }
    }
}
