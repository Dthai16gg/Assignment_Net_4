namespace Colo_Shop.Services;

using Colo_Shop.IServices;
using Colo_Shop.Models;

public class BillServices : IBillServices
{
    private readonly ShopDbContext _dbConText;

    public BillServices()
    {
        this._dbConText = new ShopDbContext();
    }

    public bool CreateNewBills(Bill Bill)
    {
        try
        {
            this._dbConText.Bills.Add(Bill);
            this._dbConText.SaveChanges();
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
            var _Bill = this._dbConText.Bills.Find(id);
            this._dbConText.Bills.Remove(_Bill);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Bill> GetAllBills()
    {
        return this._dbConText.Bills.ToList();
    }

    public List<Bill> GetBillByDateTime(DateTime date)
    {
        return this._dbConText.Bills.Where(p => p.CreateDate == date).ToList();
    }

    public Bill GetBillById(Guid id)
    {
        return this._dbConText.Bills.FirstOrDefault(p => p.Id == id);
    }

    public bool UpdateBill(Bill Bill)
    {
        try
        {
            var _Bill = this._dbConText.Bills.Find(Bill.Id);
            _Bill.CreateDate = Bill.CreateDate;
            _Bill.UserID = Bill.UserID;
            _Bill.Status = Bill.Status;
            this._dbConText.Bills.Update(_Bill);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}