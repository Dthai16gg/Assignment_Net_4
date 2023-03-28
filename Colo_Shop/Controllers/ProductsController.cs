using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _productServices.CreateNewProducts(product);
            return RedirectToAction("ShowList");
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            var product= _productServices.GetProductById(id);
            return View(product);
        }
        public IActionResult Edit(Product product)
        {
            if (product.AvailableQuantity<0 || product.Price<0 || product.Description == null)
            {
                ViewBag.Message ="Lỗi rồi nhé ! kiểm tra lại đi làm ơn";
            }
            else
            {
                _productServices.UpdateProduct(product);
            }

            return RedirectToAction("ShowList");
        }

        public IActionResult Delete(Guid id)
        {
            _productServices.DeleteProduct(id);
            return RedirectToAction("ShowList");
        }

        public IActionResult Search(string name)
        {
            if (name ==""|| name==null)
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
