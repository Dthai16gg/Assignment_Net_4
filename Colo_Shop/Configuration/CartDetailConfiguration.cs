namespace Colo_Shop.Configurations;

using Colo_Shop.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
{
    public void Configure(EntityTypeBuilder<CartDetail> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(p => p.Cart).WithMany(p => p.Details).HasForeignKey(p => p.CartId);
        builder.HasOne(p => p.Product).WithMany(p => p.CartDetails).HasForeignKey(p => p.IdSp);
    }
}