namespace Colo_Shop.IServices;

using Colo_Shop.Models;

public interface IProductServices
{
    public bool CreateNewProducts(Product product);

    public bool DeleteProduct(Guid id);

    public List<Product> GetAllProducts();

    public Product GetProductById(Guid id);

    public List<Product> GetProductByName(string name);

    public bool UpdateProduct(Product product);
}