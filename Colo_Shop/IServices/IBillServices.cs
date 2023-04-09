namespace Colo_Shop.IServices;

using Colo_Shop.Models;

public interface IBillServices
{
    public bool CreateNewBills(Bill Bill);

    public bool DeleteBill(Guid id);

    public List<Bill> GetAllBills();

    public List<Bill> GetBillByDateTime(DateTime date);

    public Bill GetBillById(Guid id);

    public bool UpdateBill(Bill Bill);
}