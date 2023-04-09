namespace Colo_Shop.Services;

using Colo_Shop.IServices;
using Colo_Shop.Models;

public class CartDetailService : ICartDetailsServices
{
    private readonly ShopDbContext _dbConText;

    public CartDetailService()
    {
        this._dbConText = new ShopDbContext();
    }

    public bool CreateNewCartDetails(CartDetail CartDetail)
    {
        try
        {
            this._dbConText.CartDetails.Add(CartDetail);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeleteCartDetail(Guid id)
    {
        try
        {
            var _CartDetails = this._dbConText.CartDetails.Find(id);
            this._dbConText.CartDetails.Remove(_CartDetails);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<CartDetail> GetAllCartDetails()
    {
        return this._dbConText.CartDetails.ToList();
    }

    public CartDetail GetCartDetailById(Guid id)
    {
        return this._dbConText.CartDetails.FirstOrDefault(p => p.Id == id);
    }

    public bool UpdateCartDetail(CartDetail CartDetail)
    {
        try
        {
            var _CartDetails = this._dbConText.CartDetails.Find(CartDetail.Id);
            _CartDetails.IdSp = CartDetail.IdSp;
            _CartDetails.CartId = CartDetail.CartId;
            _CartDetails.Quantity = CartDetail.Quantity;
            this._dbConText.CartDetails.Update(_CartDetails);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}