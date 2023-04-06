using Colo_Shop.Models;

namespace Colo_Shop.IServices;

public interface ICartServices
{
    public bool CreateNewCarts(Cart Cart);
    public bool UpdateCart(Cart Cart);
    public bool DeleteCart(Guid id);
    public Cart GetCartByUserId(Guid id);
    public List<Cart> GetAllCarts();
}