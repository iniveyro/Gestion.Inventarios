using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using server.Models;

namespace server.Context.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> entityBuilder)
        {
            entityBuilder.ToTable("User");
            entityBuilder.HasKey(x=>x.UserId);
            entityBuilder.Property(x=>x.NomApe).IsRequired().HasMaxLength(150);
            entityBuilder.Property(x=>x.Username).IsRequired().HasMaxLength(64);
            entityBuilder.Property(x=>x.Password).IsRequired().HasMaxLength(16);
            entityBuilder.Property(x=>x.Rol).HasMaxLength(20);
        }
    }
}