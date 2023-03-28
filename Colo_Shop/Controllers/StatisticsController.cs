using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers
{
    public class StatisticsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
