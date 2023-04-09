namespace Colo_Shop.Services;

using Colo_Shop.IServices;
using Colo_Shop.Models;

using Microsoft.EntityFrameworkCore;

public class BillDetailsService : IBillDetailsServices
{
    private readonly ShopDbContext _dbConText;

    public BillDetailsService()
    {
        this._dbConText = new ShopDbContext();
    }

    public bool CreateNewBillDetails(BillDetails BillDetails)
    {
        try
        {
            this._dbConText.Entry(BillDetails).State = EntityState.Added;
            this._dbConText.SaveChanges();
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
            var _BillDetails = this._dbConText.BillDetailss.Find(id);
            this._dbConText.BillDetailss.Remove(_BillDetails);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<BillDetails> GetAllBillDetails()
    {
        return this._dbConText.BillDetailss.ToList();
    }

    public BillDetails GetBillDetailsById(Guid id)
    {
        return this._dbConText.BillDetailss.FirstOrDefault(p => p.Id == id);
    }

    public bool UpdateBillDetails(BillDetails BillDetails)
    {
        try
        {
            var _BillDetails = this._dbConText.BillDetailss.Find(BillDetails.Id);
            _BillDetails.Quantity = BillDetails.Quantity;
            _BillDetails.IdHD = BillDetails.IdHD;
            _BillDetails.IdSp = BillDetails.IdSp;
            _BillDetails.Price = BillDetails.Price;
            this._dbConText.BillDetailss.Update(_BillDetails);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}