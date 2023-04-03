namespace Colo_Shop.Models;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int AvailableQuantity { get; set; }
    public int Status { get; set; }
    public string Supplier { get; set; }
    public string Description { get; set; }
    public string Style { get; set; }
    public byte[] Image { get; set; }
    public virtual ICollection<BillDetails> BillDetails { get; set; }
    public virtual ICollection<CartDetail> CartDetails { get; set; }
}