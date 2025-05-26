using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class EquipoConfiguration : IEntityTypeConfiguration<EquipoModel>
    {
        public void Configure(EntityTypeBuilder<EquipoModel> entityBuilder)
        {
            entityBuilder.ToTable("Equipo");
            
            entityBuilder.HasKey(x => x.IdEquipo);
            entityBuilder.Property(x => x.IdEquipo).UseIdentityColumn();
            entityBuilder.Property(x=>x.NroInventario);
            entityBuilder.HasIndex(x => x.NroInventario).IsUnique();
            entityBuilder.Property(x=>x.NroSerie);
            entityBuilder.HasIndex(x => x.NroSerie).IsUnique();
            entityBuilder.Property(x=>x.Marca).IsRequired();
            entityBuilder.Property(x=>x.Modelo).IsRequired();
            entityBuilder.Property(x=>x.Observacion);
        }
    }
}