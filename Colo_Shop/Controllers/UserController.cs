using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
