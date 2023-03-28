using Colo_Shop.Models;

namespace Colo_Shop.IServices
{
  public interface IBillServices
  {
    public bool CreateNewBills(Bill Bill);
    public bool UpdateBill(Bill Bill);
    public bool DeleteBill(Guid id);
    public Bill GetBillById(Guid id);
    public List<Bill> GetBillByDateTime(DateTime date);
    public List<Bill> GetAllBills();
  }
}
