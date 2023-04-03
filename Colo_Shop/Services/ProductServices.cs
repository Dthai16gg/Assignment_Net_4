using Colo_Shop.IServices;
using Colo_Shop.Models;

namespace Colo_Shop.Services;

public class ProductServices : IProductServices
{
    private readonly ShopDbContext _dbConText;

    public ProductServices()
    {
        _dbConText = new ShopDbContext();
    }

    public bool CreateNewProducts(Product product)
    {
        try
        {
            _dbConText.Products.Add(product);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool DeleteProduct(Guid id)
    {
        try
        {
            var _product = _dbConText.Products.Find(id);
            _dbConText.Products.Remove(_product);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public bool UpdateProduct(Product product)
    {
        try
        {
            var _product = _dbConText.Products.Find(product.Id);
            _product.Name = product.Name;
            _product.Price = product.Price;
            _product.Supplier = product.Supplier;
            _product.Status = product.Status;
            _product.AvailableQuantity = product.AvailableQuantity;
            _product.Description = product.Description;
            _product.Style = product.Style;
            _product.Image = product.Image;
            _dbConText.Products.Update(_product);
            _dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Product> GetAllProducts()
    {
        return _dbConText.Products.ToList();
    }

    public Product GetProductById(Guid id)
    {
        return _dbConText.Products.FirstOrDefault(p => p.Id == id); //
    }

    public List<Product> GetProductByName(string name)
    {
        return _dbConText.Products.Where(p => p.Name.Contains(name)).ToList();
    }
}