using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers
{
    public class AdminController : Controller
    {
        private IUserServices _services;
        private IRoleServices _roleServices;

        public AdminController()
        {
            _services = new UserServices();
            _roleServices = new RoleServices();
        }

        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        public IActionResult HomePage()
        {
            return View();
        }

        public bool CheckLogin(string username, string password)
        {
            var user = _services.GetUserByName(username).FirstOrDefault();
            if (user != null && user.Password == password && user.Status == 1)
            {
                var role = _roleServices.GetRoleById(user.RoleId);
                if (role != null && role.RoleName == "Admin")
                {
                    return true;
                }
            }
            return false;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Username,string Password)
        {
            bool isValid = CheckLogin(Username, Password);
            if (isValid == true)
            {
                return RedirectToAction("HomePage");
            }
            else
            {
                ViewBag.ErrorMessage = "The user name or password provided is incorrect.";
                return View("Login");
            }
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
