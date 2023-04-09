namespace Web_Application.Configurations;

using Colo_Shop.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasColumnType("nvarchar(MAX)");
        builder.Property(x => x.Supplier).HasColumnType("nvarchar(MAX)");
        builder.Property(x => x.Description).HasColumnType("nvarchar(MAX)");
        builder.Property(x => x.Style).HasColumnType("nvarchar(MAX)");
        builder.Property(x => x.Image);
    }
}