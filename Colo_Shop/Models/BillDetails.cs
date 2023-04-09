namespace Colo_Shop.Models;

public class BillDetails
{
    public virtual Bill Bill { get; set; }

    public Guid Id { get; set; }

    public Guid IdHD { get; set; }

    public Guid IdSp { get; set; }

    public int Price { get; set; }

    public virtual Product Product { get; set; }

    public int Quantity { get; set; }
}