using Colo_Shop.IServices;
using Colo_Shop.Models;
using Microsoft.EntityFrameworkCore;

namespace Colo_Shop.Services;

public class CartServices : ICartServices
{
    private readonly ShopDbContext _dbConText;

    public CartServices()
    {
        _dbConText = new ShopDbContext();
    }

    public bool CreateNewCarts(Cart Cart)
    {
        try
        {
            _dbConText.Carts.Add(Cart);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeleteCart(Guid id)
    {
        try
        {
            var _Cart = _dbConText.Carts.Find(id);
            _dbConText.Carts.Remove(_Cart);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool UpdateCart(Cart Cart)
    {
        try
        {
            var _Cart = _dbConText.Carts.Find(Cart.Id);
            _Cart.Description = Cart.Description;
            _Cart.UserId = Cart.UserId;
            _dbConText.Carts.Update(_Cart);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Cart> GetAllCarts()
    {
        return _dbConText.Carts.ToList();
    }

    public Cart GetCartByUserId(Guid userId)
    {
        return _dbConText.Carts
            .Include(c => c.Details)
            .ThenInclude(d => d.Product)
            .FirstOrDefault(c => c.User.Id == userId);
    }
}