namespace Colo_Shop.Models;

public class CartDetail
{
    public virtual Cart Cart { get; set; }

    public Guid CartId { get; set; }

    public Guid Id { get; set; }

    public Guid IdSp { get; set; }

    public virtual Product Product { get; set; }

    public int Quantity { get; set; }
}