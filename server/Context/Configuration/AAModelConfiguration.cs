using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class AAModelConfiguration: IEntityTypeConfiguration<AAModel>
    {
        public void Configure(EntityTypeBuilder<AAModel> entityBuilder)
        {
            entityBuilder.ToTable("AiresAcond");
            entityBuilder.Property(x=>x.Frigorias).HasMaxLength(10);
            entityBuilder.Property(x=>x.Potencia).HasMaxLength(10);
            entityBuilder.Property(x=>x.Tipo).IsRequired();
        }
    }
}