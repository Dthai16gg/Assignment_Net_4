using System.Diagnostics;
using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly IProductServices _productServices;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _productServices = new ProductServices();
    }

    public IActionResult Index()
    {
        var listProduct = _productServices.GetAllProducts();
        return View(listProduct.ToList());
    }

    public IActionResult Shop()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    public IActionResult Blog()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult MyAccount()
    {
        return View();
    }

    public IActionResult ProductDetails()
    {
        return View();
    }

    public IActionResult Cart()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}