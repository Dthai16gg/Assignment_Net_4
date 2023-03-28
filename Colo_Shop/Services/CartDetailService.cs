using Colo_Shop.Models;
using Colo_Shop.IServices;

namespace Colo_Shop.Services
{
  public class CartDetailService : ICartDetailsServices
  {
    ShopDbContext _dbConText;
    public CartDetailService()
    {
      _dbConText = new ShopDbContext();
    }
    public bool CreateNewCartDetails(CartDetail CartDetail)
    {
      try
      {
        _dbConText.CartDetails.Add(CartDetail);
        _dbConText.SaveChanges();
        return true;
      }
      catch (System.Exception)
      {
        return false;
      }
    }

    public bool DeleteCartDetail(Guid id)
    {
      try
      {
        var _CartDetails = _dbConText.CartDetails.Find(id);
        _dbConText.CartDetails.Remove(_CartDetails);
        _dbConText.SaveChanges();
        return true;
      }
      catch (System.Exception)
      {
        return false;
      }
    }
    public bool UpdateCartDetail(CartDetail CartDetail)
    {
      try
      {
        var _CartDetails = _dbConText.CartDetails.Find(CartDetail.Id);
        _CartDetails.IdSp = CartDetail.IdSp;
        _CartDetails.UserId = CartDetail.UserId;
        _CartDetails.Quantity = CartDetail.Quantity;
        _dbConText.CartDetails.Update(_CartDetails);
        _dbConText.SaveChanges();
        return true;
      }
      catch (System.Exception)
      {
        return false;
      }
    }
    public List<CartDetail> GetAllCartDetails()
    {
      return _dbConText.CartDetails.ToList();
    }

    public CartDetail GetCartDetailById(Guid id)
    {
      return _dbConText.CartDetails.FirstOrDefault(p => p.Id == id);
    }

  }
}
