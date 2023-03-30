using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Colo_Shop.Controllers
{
    public class ProductsController : Controller
    {
        private IProductServices _productServices;

        public ProductsController()
        {
            _productServices = new ProductServices();
        }

        public IActionResult ShowList()
        {
            var products = _productServices.GetAllProducts();
            return View(products.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile imageFile)
        {
            if (product.Price < 0 || product.AvailableQuantity < 0)
            {
                return Content("Kiem tra lai");
            }

            if (imageFile != null)
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    product.Image = stream.ToArray();
                }
            }

            _productServices.CreateNewProducts(product);

            return RedirectToAction("ShowList");
        }


        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product = _productServices.GetProductById(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
        {
            if (product.Price < 0 || product.AvailableQuantity < 0)
            {
                return Content("Kiem tra lai");
            }

            using (var stream = new MemoryStream())
            {
                await imageFile.CopyToAsync(stream);
                product.Image = stream.ToArray();
            }
            _productServices.UpdateProduct(product);
            return RedirectToAction(nameof(ShowList));
        }



        public IActionResult Delete(Guid id)
        {
            _productServices.DeleteProduct(id);
            return RedirectToAction("ShowList");
        }

        public IActionResult Search(string name)
        {
            if (name == "" || name == null)
            {
                var r = _productServices.GetAllProducts();
                return View("ShowList", r.ToList());
            }
            else
            {
                var p = _productServices.GetProductByName(name);
                return View("ShowList", p.ToList());
            }
        }

        public IActionResult Details(Guid id)
        {
            var p = _productServices.GetProductById(id);
            return View(p);
        }
    }
}
