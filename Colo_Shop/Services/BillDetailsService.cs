using Colo_Shop.IServices;
using Colo_Shop.Models;

namespace Colo_Shop.Services;

public class BillDetailsService : IBillDetailsServices
{
    private readonly ShopDbContext _dbConText;

    public BillDetailsService()
    {
        _dbConText = new ShopDbContext();
    }

    public bool CreateNewBillDetails(BillDetails BillDetails)
    {
        try
        {
            _dbConText.BillDetailss.Add(BillDetails);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeleteBillDetails(Guid id)
    {
        try
        {
            var _BillDetails = _dbConText.BillDetailss.Find(id);
            _dbConText.BillDetailss.Remove(_BillDetails);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool UpdateBillDetails(BillDetails BillDetails)
    {
        try
        {
            var _BillDetails = _dbConText.BillDetailss.Find(BillDetails.Id);
            _BillDetails.Quantity = BillDetails.Quantity;
            _BillDetails.Price = BillDetails.Price;
            _dbConText.BillDetailss.Update(_BillDetails);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<BillDetails> GetAllBillDetails()
    {
        return _dbConText.BillDetailss.ToList();
    }

    public BillDetails GetBillDetailsById(Guid id)
    {
        return _dbConText.BillDetailss.FirstOrDefault(p => p.Id == id);
    }
}