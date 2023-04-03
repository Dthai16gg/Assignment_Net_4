using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }
}