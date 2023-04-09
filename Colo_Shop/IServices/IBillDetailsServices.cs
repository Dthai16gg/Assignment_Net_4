namespace Colo_Shop.IServices;

using Colo_Shop.Models;

public interface IBillDetailsServices
{
    public bool CreateNewBillDetails(BillDetails BillDetails);

    public bool DeleteBillDetails(Guid id);

    public List<BillDetails> GetAllBillDetails();

    public BillDetails GetBillDetailsById(Guid id);

    public bool UpdateBillDetails(BillDetails BillDetails);
}