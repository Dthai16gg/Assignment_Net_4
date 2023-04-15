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
        if (CheckTrungTen(product.Name, product.Supplier))
        {
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
        else
        {
            return Content("BỊ TrÙNG");
        }
    }
    //Check trùng (tên + nhà cung cấp) đã tồn tại khi sửa/thêm mới sản phẩm. VD: Bánh Quy - Kinhdo sẽ không trùng với Bánh Quy - Bibica 
    public bool CheckTrungTen(string name, string supplier)
    {
        var Product = _productServices.GetAllProducts()
            .FirstOrDefault(p => p.Name == name && p.Supplier == supplier);
        if (Product != null)
        {
            return false;
        }
        return true;
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

            if (CheckTrungTen(product.Name, product.Supplier))
            {
                this._productServices.UpdateProduct(product);
            }
            else
            {
                return Content("BỊ TrÙNG");
            }
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