using Colo_Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Web_Application.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Description).HasColumnType("nvarchar(MAX)");
        builder.Property(p => p.RoleName).HasColumnType("nvarchar(MAX)");
    }
}