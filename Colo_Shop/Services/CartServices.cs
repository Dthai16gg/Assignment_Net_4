namespace Colo_Shop.Services;

using Colo_Shop.IServices;
using Colo_Shop.Models;

using Microsoft.EntityFrameworkCore;

public class CartServices : ICartServices
{
    private readonly ShopDbContext _dbConText;

    public CartServices()
    {
        this._dbConText = new ShopDbContext();
    }

    public bool CreateNewCarts(Cart Cart)
    {
        try
        {
            this._dbConText.Carts.Add(Cart);
            this._dbConText.SaveChanges();
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
            var _Cart = this._dbConText.Carts.Find(id);
            this._dbConText.Carts.Remove(_Cart);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Cart> GetAllCarts()
    {
        return this._dbConText.Carts.ToList();
    }

    public Cart GetCartByUserId(Guid userId)
    {
        return this._dbConText.Carts.Include(c => c.Details).ThenInclude(d => d.Product)
            .FirstOrDefault(c => c.User.Id == userId);
    }

    public bool UpdateCart(Cart Cart)
    {
        try
        {
            var _Cart = this._dbConText.Carts.Find(Cart.Id);
            _Cart.Description = Cart.Description;
            _Cart.UserId = Cart.UserId;
            this._dbConText.Carts.Update(_Cart);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}