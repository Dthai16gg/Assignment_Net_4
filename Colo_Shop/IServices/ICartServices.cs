namespace Colo_Shop.IServices;

using Colo_Shop.Models;

public interface ICartServices
{
    public bool CreateNewCarts(Cart Cart);

    public bool DeleteCart(Guid id);

    public List<Cart> GetAllCarts();

    public Cart GetCartByUserId(Guid id);

    public bool UpdateCart(Cart Cart);
}