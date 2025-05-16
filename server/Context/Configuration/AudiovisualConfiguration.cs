using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class AudiovisualConfiguration : IEntityTypeConfiguration<AudiovisualModel>
    {
        public void Configure(EntityTypeBuilder<AudiovisualModel> entityBuilder)
        {
            entityBuilder.ToTable("Audivisual");
            entityBuilder.HasBaseType<EquipoModel>();
        }
    }
}