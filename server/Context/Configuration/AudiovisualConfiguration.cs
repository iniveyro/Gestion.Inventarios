using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class AudiovisualConfiguration : IEntityTypeConfiguration<AudiovisualModel>
    {
        public void Configure(EntityTypeBuilder<AudiovisualModel> entityBuilder)
        {
            entityBuilder.ToTable("Audiovisual");
            entityBuilder.HasBaseType<EquipoModel>();
            entityBuilder.HasOne<EquipoModel>()
            .WithOne()
            .HasForeignKey<AudiovisualModel>(x => x.IdEquipo)
            .OnDelete(DeleteBehavior.Cascade);
            entityBuilder.Property(x=>x.Accesorio).HasMaxLength(200);
            entityBuilder.Property(x=>x.Tipo).HasMaxLength(100);
        }
    }
}