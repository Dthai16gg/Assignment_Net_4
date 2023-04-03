using Colo_Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colo_Shop.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(p => p.UserId);
        builder.Property(p => p.Description)
            .HasColumnType("nvarchar(MAX)"); // nvarchar(1000)
    }
}