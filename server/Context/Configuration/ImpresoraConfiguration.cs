using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class ImpresoraConfiguration : IEntityTypeConfiguration<ImpresoraModel>
    {
        public void Configure(EntityTypeBuilder<ImpresoraModel> entityBuilder)
        {
            entityBuilder.ToTable("Impresora");
            entityBuilder.HasBaseType<EquipoModel>();
            entityBuilder.HasOne<EquipoModel>()
            .WithOne()
            .HasForeignKey<ImpresoraModel>(x => x.IdEquipo)
            .OnDelete(DeleteBehavior.Cascade);
            entityBuilder.Property(x=>x.TonnerModelo).IsRequired().HasMaxLength(30);
            entityBuilder.Property(x=>x.Tipo).IsRequired().HasMaxLength(30);
            entityBuilder.Property(x=>x.Consumible).IsRequired();
        }
    }
}