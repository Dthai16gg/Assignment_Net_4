using Colo_Shop.IServices;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers
{
    public class UserController : Controller
    {
        private IUserServices _userServices;

        public UserController()
        {
            _userServices = new UserServices();
        }

        public IActionResult ShowList()
        {
            return View();
        }

    }
}
