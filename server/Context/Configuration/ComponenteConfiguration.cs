using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class ComponenteConfiguration : IEntityTypeConfiguration<ComponenteModel>
    {
        public void Configure(EntityTypeBuilder<ComponenteModel> entityBuilder)
        {
            entityBuilder.ToTable("Componentes");
            entityBuilder.HasKey(x => x.IdComp);
            entityBuilder.Property(x => x.IdComp).UseIdentityColumn();
            entityBuilder.Property(x => x.Marca).HasMaxLength(80);
            entityBuilder.Property(x => x.Modelo).HasMaxLength(80);
            entityBuilder.HasIndex(x => x.Modelo).IsUnique();
            entityBuilder.Property(x => x.Detalle).HasMaxLength(200);
            entityBuilder.Property(x => x.Tipo).HasMaxLength(80);
            entityBuilder.Property(x=>x.Cantidad).HasMaxLength(1000);
        }
    }
}