namespace Colo_Shop.Models;

public class Bill
{
    public DateTime CreateDate { get; set; }

    public virtual List<BillDetails> Details { get; set; }

    public Guid Id { get; set; }

    public int Status { get; set; }

    public virtual User User { get; set; }

    public Guid UserID { get; set; }
}