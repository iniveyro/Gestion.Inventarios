using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class MonitorConfiguration : IEntityTypeConfiguration<MonitorModel>
    {
        public void Configure(EntityTypeBuilder<MonitorModel> entityBuilder)
        {
            entityBuilder.ToTable("Monitor");
            entityBuilder.HasBaseType<EquipoModel>();
            entityBuilder.HasOne<EquipoModel>()
            .WithOne()
            .HasForeignKey<MonitorModel>(m => m.IdEquipo)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}