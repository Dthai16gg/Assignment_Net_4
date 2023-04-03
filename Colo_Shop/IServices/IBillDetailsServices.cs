using Colo_Shop.Models;

namespace Colo_Shop.IServices;

public interface IBillDetailsServices
{
    public bool CreateNewBillDetails(BillDetails BillDetails);
    public bool UpdateBillDetails(BillDetails BillDetails);
    public bool DeleteBillDetails(Guid id);
    public BillDetails GetBillDetailsById(Guid id);
    public List<BillDetails> GetAllBillDetails();
}