using Colo_Shop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Colo_Shop.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.ToTable("HoaDon"); // Đặt tên bảng
        builder.HasKey(p => p.Id); // Set khóa chính
        // Cấu hình cho thuộc tính
        builder.Property(p => p.CreateDate).HasColumnType("Date");
        builder.Property(p => p.Status).HasColumnType("int").IsRequired(); // int not null
        builder.HasOne(p => p.User).WithMany(p => p.Bills).HasForeignKey(p => p.Id).HasConstraintName("FK_Bill_User");
    }
}