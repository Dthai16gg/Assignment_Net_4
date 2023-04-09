namespace Colo_Shop.Controllers;

using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;

using Microsoft.AspNetCore.Mvc;

public class ProductsController : Controller
{
    private readonly IProductServices _productServices;

    public ProductsController()
    {
        this._productServices = new ProductServices();
    }

    public IActionResult Create()
    {
        return this.View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product, IFormFile imageFile)
    {
        if (product.Price < 0 || product.AvailableQuantity < 0) return this.Content("Kiem tra lai");

        if (imageFile != null)
            using (var stream = new MemoryStream())
            {
                await imageFile.CopyToAsync(stream);
                product.Image = stream.ToArray();
            }
        else
            return this.View("Create");

        this._productServices.CreateNewProducts(product);

        return this.RedirectToAction("ShowList");
    }

    public IActionResult Delete(Guid id)
    {
        this._productServices.DeleteProduct(id);
        return this.RedirectToAction("ShowList");
    }

    public IActionResult Details(Guid id)
    {
        var p = this._productServices.GetProductById(id);
        return this.View(p);
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var product = this._productServices.GetProductById(id);

        // Đọc từ Session danh sách sp trong giỏ hàng
        return this.View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
    {
        try
        {
            using (var stream = new MemoryStream())
            {
                await imageFile.CopyToAsync(stream);
                product.Image = stream.ToArray();
            }

            this._productServices.UpdateProduct(product);
        }
        catch (Exception e)
        {
            return this.Content(e.Message);
        }

        return this.RedirectToAction(nameof(this.ShowList));
    }

    public IActionResult Search(string name)
    {
        if (name == string.Empty || name == null)
        {
            var r = this._productServices.GetAllProducts();
            return this.View("ShowList", r.ToList());
        }

        var p = this._productServices.GetProductByName(name);
        return this.View("ShowList", p.ToList());
    }

    public IActionResult ShowList()
    {
        var products = this._productServices.GetAllProducts();
        return this.View(products.ToList());
    }
}