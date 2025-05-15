using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class PcConfiguration : IEntityTypeConfiguration<PcModel>
    {
        public void Configure(EntityTypeBuilder<PcModel> entityBuilder)
        {
            entityBuilder.ToTable("Pc");
            entityBuilder.Property(x=>x.Disco).HasMaxLength(100);
            entityBuilder.Property(x=>x.Fuente).HasMaxLength(100);
            entityBuilder.Property(x=>x.Procesador).HasMaxLength(100);
            entityBuilder.Property(x=>x.Ram).HasMaxLength(100);
            entityBuilder.Property(x=>x.TipoRam).HasMaxLength(10);
        }
    }
}