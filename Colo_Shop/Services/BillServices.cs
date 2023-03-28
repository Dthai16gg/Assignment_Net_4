using Colo_Shop.IServices;
using Colo_Shop.Models;
namespace Colo_Shop.Services
{
  public class BillServices : IBillServices
  {
    ShopDbContext _dbConText;
    public BillServices()
    {
      _dbConText = new ShopDbContext();
    }
    public bool CreateNewBills(Bill Bill)
    {
      try
      {
        _dbConText.Bills.Add(Bill);
        _dbConText.SaveChanges();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public bool DeleteBill(Guid id)
    {
      try
      {
        var _Bill = _dbConText.Bills.Find(id);
        _dbConText.Bills.Remove(_Bill);
        _dbConText.SaveChanges();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }
    public bool UpdateBill(Bill Bill)
    {
      try
      {
        var _Bill = _dbConText.Bills.Find(Bill.Id);
        _Bill.CreateDate = Bill.CreateDate;
        _Bill.UserID = Bill.UserID;
        _Bill.Status = Bill.Status;
        _dbConText.Bills.Update(_Bill);
        _dbConText.SaveChanges();
        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public List<Bill> GetAllBills()
    {
      return _dbConText.Bills.ToList();
    }
    public Bill GetBillById(Guid id)
    {
      return _dbConText.Bills.FirstOrDefault(p => p.Id == id);
    }

    public List<Bill> GetBillByDateTime(DateTime date)
    {
      return _dbConText.Bills.Where(p => p.CreateDate == date).ToList();
    }

  }
}
