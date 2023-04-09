namespace Colo_Shop.Models;

using System.Reflection;

using Microsoft.EntityFrameworkCore;

public class ShopDbContext : DbContext
{
    public ShopDbContext()
    {
    }

    public ShopDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public object BillDetail { get; internal set; }

    public object BillDetails { get; internal set; }

    public DbSet<BillDetails> BillDetailss { get; set; }

    public DbSet<Bill> Bills { get; set; }

    public DbSet<CartDetail> CartDetails { get; set; }

    public DbSet<Cart> Carts { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Data Source=DTHAI16GG\SQLEXPRESS;Initial Catalog=Assignment_Net-4-Main;Integrated Security=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}