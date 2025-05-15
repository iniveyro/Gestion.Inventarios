using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class OficinaConfiguration : IEntityTypeConfiguration<OficinaModel>
    {
        public void Configure(EntityTypeBuilder<OficinaModel> entityBuilder)
        {
            entityBuilder.ToTable("Oficina");
            entityBuilder.HasKey(x=>x.OficinaId);
            entityBuilder.Property(x=>x.Nombre).IsRequired().HasMaxLength(100);
            entityBuilder.Property(x=>x.Ubicacion).IsRequired().HasMaxLength(100);
        }
    }
}