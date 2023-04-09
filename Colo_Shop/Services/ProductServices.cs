namespace Colo_Shop.Services;

using Colo_Shop.IServices;
using Colo_Shop.Models;

public class ProductServices : IProductServices
{
    private readonly ShopDbContext _dbConText;

    public ProductServices()
    {
        this._dbConText = new ShopDbContext();
    }

    public bool CreateNewProducts(Product product)
    {
        try
        {
            this._dbConText.Products.Add(product);
            this._dbConText.SaveChanges();
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
            var _product = this._dbConText.Products.Find(id);
            this._dbConText.Products.Remove(_product);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public List<Product> GetAllProducts()
    {
        return this._dbConText.Products.ToList();
    }

    public Product GetProductById(Guid id)
    {
        return this._dbConText.Products.FirstOrDefault(p=>p.Id ==id);
    }

    public List<Product> GetProductByName(string name)
    {
        return this._dbConText.Products.Where(p => p.Name.Contains(name)).ToList();
    }

    public bool UpdateProduct(Product product)
    {
        try
        {
            var _product = this._dbConText.Products.Find(product.Id);
            _product.Name = product.Name;
            _product.Price = product.Price;
            _product.Supplier = product.Supplier;
            _product.Status = product.Status;
            _product.AvailableQuantity = product.AvailableQuantity;
            _product.Description = product.Description;
            _product.Style = product.Style;
            _product.Image = product.Image;
            this._dbConText.Products.Update(_product);
            this._dbConText.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}