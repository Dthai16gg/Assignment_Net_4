namespace Colo_Shop.Models;

public class Cart
{
    public string Description { get; set; }

    public virtual List<CartDetail> Details { get; set; }

    public Guid Id { get; set; }

    public virtual User User { get; set; }

    public Guid UserId { get; set; }
}