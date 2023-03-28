using Colo_Shop.Models;

namespace Colo_Shop.IServices
{
  public interface ICartDetailsServices
  {
    public bool CreateNewCartDetails(CartDetail CartDetail);
    public bool UpdateCartDetail(CartDetail CartDetail);
    public bool DeleteCartDetail(Guid id);
    public CartDetail GetCartDetailById(Guid id);
    public List<CartDetail> GetAllCartDetails();
  }
}
