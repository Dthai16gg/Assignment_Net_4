namespace Colo_Shop.Models;

public class Bill
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid UserID { get; set; }
    public int Status { get; set; }
    public virtual IQueryable<BillDetails> Details { get; set; }
    public virtual User User { get; set; }
}