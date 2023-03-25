using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
