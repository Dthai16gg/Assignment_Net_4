namespace Colo_Shop.IServices;

using Colo_Shop.Models;

public interface ICartDetailsServices
{
    public bool CreateNewCartDetails(CartDetail CartDetail);

    public bool DeleteCartDetail(Guid id);

    public List<CartDetail> GetAllCartDetails();

    public CartDetail GetCartDetailById(Guid id);

    public bool UpdateCartDetail(CartDetail CartDetail);
}