using System.Text.RegularExpressions;
using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using static Colo_Shop.Controllers.UserController;

namespace Colo_Shop.Controllers;

public class AdminController : Controller
{
    private readonly IRoleServices _roleServices;
    private readonly IUserServices _services;

    public AdminController()
    {
        _services = new UserServices();
        _roleServices = new RoleServices();
    }

    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    public IActionResult SingOut()
    {
        HttpContext.Session.Remove("idUsers");
        return RedirectToAction("Login");
    }

    public IActionResult HomePage()
    {
        var idUsers = HttpContext.Session.GetString("idUsers");
        ViewData["idUsers"] = idUsers;
        if (!string.IsNullOrEmpty(idUsers))
            return View();
        return RedirectToAction("Login");
    }

    public bool CheckLogin(string username, string password)
    {
        var user = _services.GetUserByUserName(username).FirstOrDefault();
        if (user != null && user.Password == password && user.Status == 1)
        {
            var role = _roleServices.GetRoleById(user.RoleId);
            if (role.RoleName == "Admin") return true;
        }

        return false;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string Username, string Password)
    {
        var isValid = CheckLogin(Username, Password);
        if (isValid)
        {
            var user = _services.GetUserByUserName(Username).FirstOrDefault();
            var idUsers = user.Id.ToString();
            HttpContext.Session.SetString("idUsers", idUsers);
            if (user != null)
                return RedirectToAction("HomePage");
            // handle the case where the user is not found
            // e.g. display an error message or redirect to a login page
            ViewData["ErrorMessage"] = "User not found";
        }
        else
        {
            ViewBag.ErrorMessage = "The user name or password provided is incorrect.";
        }

        return View("Login");
    }

    public IActionResult Register()
    {
        var viewModel = new CreateViewModel
        {
            Roles = _roleServices.GetAllRoles().ToList(),
            User = new User()
        };
        return View(viewModel);
    }

    public bool IsValidPhoneNumber(string phoneNumber)
    {
        var regex = new Regex(@"^(03|05|07|08|09)[0-9]{8}$");
        return regex.IsMatch(phoneNumber);
    }

    public bool IsValidEmail(string email)
    {
        var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        return regex.IsMatch(email);
    }

    public bool IsValidName(string name)
    {
        var regex = new Regex(@"^[a-zA-Z]+$");
        return regex.IsMatch(name);
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        var viewModel = new CreateViewModel
        {
            Roles = _roleServices.GetAllRoles().ToList(),
            User = new User()
        };
        try
        {
            if (user.Password.Length < 8 || !user.Password.Any(char.IsLetter) || user.Password == null)
            {
                ViewBag.AlertMessage = "Password must be at least 8 characters long and contain at least one letter.";
                return View(viewModel);
            }

            if (!IsValidPhoneNumber(user.NumberPhone))
            {
                ViewBag.AlertMessage = "Please enter a valid phone number.";
                return View(viewModel);
            }

            if (!IsValidEmail(user.Email))
            {
                ViewBag.AlertMessage = "Please enter a valid email.";
                return View(viewModel);
            }

            if (IsValidName(user.Name))
            {
                ViewBag.AlertMessage = "Please enter a valid name.";
                return View(viewModel);
            }

            if (_services.GetAllUsers().Any(u => u.Username == user.Username.Trim()))
            {
                ViewBag.AlertMessage = "Username already exists.";
                return View(viewModel);
            }

            if (_services.CreateNewUsers(user)) ;

            return RedirectToAction("Login");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }

        return Content("Not User");
    }

    public IActionResult MyAccount()
    {
        var idUsers = HttpContext.Session.GetString("idUsers");
        ViewData["idUsers"] = idUsers;
        if (!string.IsNullOrEmpty(idUsers))
            return View();
        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult MyAccount(User user)
    {
        if (user.Password.Length < 8 || !user.Password.Any(char.IsLetter) || user.Password == null)
        {
            ViewBag.AlertMessage = "Password must be at least 8 characters long and contain at least one letter.";
            return View();
        }

        if (!IsValidPhoneNumber(user.NumberPhone))
        {
            ViewBag.AlertMessage = "Please enter a valid phone number.";
            return View();
        }

        if (!IsValidEmail(user.Email))
        {
            ViewBag.AlertMessage = "Please enter a valid email.";
            return View();
        }

        if (IsValidName(user.Name))
        {
            ViewBag.AlertMessage = "Please enter a valid name.";
            return View();
        }

        var existingUsers = _services.GetAllUsers(user.Id);
        if (existingUsers.Any(u => u.Username == user.Username.Trim()))
        {
            ViewBag.AlertMessage = "Username already exists.";
            return View();
        }

        if (_services.UpdateUser(user)) return RedirectToAction("HomePage");
        return BadRequest();
    }
}