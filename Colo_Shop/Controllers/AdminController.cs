using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }
        public IActionResult HomePage()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult MyAccount()
        {
            return View();
        }
    }
}
