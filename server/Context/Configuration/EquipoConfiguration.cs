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
            entityBuilder.HasKey(x=>x.NroInventario);
            entityBuilder.HasAlternateKey(x=>x.NroSerie);
            
            entityBuilder.Property(x=>x.Marca).IsRequired();
            entityBuilder.Property(x=>x.Modelo).IsRequired();
            entityBuilder.Property(x=>x.Observacion);
        }
    }
}