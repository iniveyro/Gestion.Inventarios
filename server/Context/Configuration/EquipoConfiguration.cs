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
            entityBuilder.Property(x => x.NroInventario).IsRequired(false).HasDefaultValue(0); // Asegura que la columna permite NULL
            entityBuilder.HasIndex(x => x.NroInventario).IsUnique().HasFilter(@"""NroInventario"" <> 0"); // Filtro para ignorar NULLs en la unicidad
            entityBuilder.Property(x => x.NroSerie).IsRequired(false); // Asegura que la columna permite NULL
            entityBuilder.HasIndex(x => x.NroSerie).IsUnique().HasFilter(@"""NroSerie"" IS NOT NULL AND ""NroSerie"" <> ''"); // Filtro para ignorar NULLs en la unicidad
            entityBuilder.Property(x=>x.Marca).IsRequired();
            entityBuilder.Property(x=>x.Modelo).IsRequired();
            entityBuilder.Property(x=>x.Observacion);
        }
    }
}