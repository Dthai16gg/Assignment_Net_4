namespace Colo_Shop.Configurations;

using Colo_Shop.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Description).HasColumnType("nvarchar(MAX)"); // nvarchar(1000)
        builder.HasOne(p => p.User).WithMany().HasForeignKey(p => p.UserId);
    }
}