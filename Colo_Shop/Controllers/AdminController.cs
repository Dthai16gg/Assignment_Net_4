using static Colo_Shop.Controllers.UserController;

namespace Colo_Shop.Controllers;

using System.Text.RegularExpressions;

using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;

using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    private readonly IRoleServices _roleServices;

    private readonly IUserServices _services;

    public AdminController()
    {
        this._services = new UserServices();
        this._roleServices = new RoleServices();
    }

    public bool CheckLogin(string username, string password)
    {
        var user = this._services.GetUserByUserName(username).FirstOrDefault();
        if (user != null && user.Password == password && user.Status == 1)
        {
            var role = this._roleServices.GetRoleById(user.RoleId);
            if (role.RoleName == "Admin") return true;
        }

        return false;
    }

    public IActionResult HomePage()
    {
        var idUsers = this.HttpContext.Session.GetString("idUsers");
        this.ViewData["idUsers"] = idUsers;
        if (!string.IsNullOrEmpty(idUsers))
            return this.View();
        return this.RedirectToAction("Login");
    }

    public IActionResult Index()
    {
        return this.RedirectToAction("Login");
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

    public bool IsValidPhoneNumber(string phoneNumber)
    {
        var regex = new Regex(@"^(03|05|07|08|09)[0-9]{8}$");
        return regex.IsMatch(phoneNumber);
    }

    public IActionResult Login()
    {
        return this.View();
    }

    [HttpPost]
    public IActionResult Login(string Username, string Password)
    {
        var isValid = this.CheckLogin(Username, Password);
        if (isValid)
        {
            var user = this._services.GetUserByUserName(Username).FirstOrDefault();
            var idUsers = user.Id.ToString();
            this.HttpContext.Session.SetString("idUsers", idUsers);
            if (user != null)
                return this.RedirectToAction("HomePage");

            // handle the case where the user is not found
            // e.g. display an error message or redirect to a login page
            this.ViewData["ErrorMessage"] = "User not found";
        }
        else
        {
            this.ViewBag.ErrorMessage = "The user name or password provided is incorrect.";
        }

        return this.View("Login");
    }

    public IActionResult MyAccount()
    {
        var idUsers = this.HttpContext.Session.GetString("idUsers");
        this.ViewData["idUsers"] = idUsers;
        if (!string.IsNullOrEmpty(idUsers))
            return this.View();
        return this.RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult MyAccount(User user)
    {
        if (user.Password.Length < 8 || !user.Password.Any(char.IsLetter) || user.Password == null)
        {
            this.ViewBag.AlertMessage = "Password must be at least 8 characters long and contain at least one letter.";
            return this.View();
        }

        if (!this.IsValidPhoneNumber(user.NumberPhone))
        {
            this.ViewBag.AlertMessage = "Please enter a valid phone number.";
            return this.View();
        }

        if (!this.IsValidEmail(user.Email))
        {
            this.ViewBag.AlertMessage = "Please enter a valid email.";
            return this.View();
        }

        if (this.IsValidName(user.Name))
        {
            this.ViewBag.AlertMessage = "Please enter a valid name.";
            return this.View();
        }

        var existingUsers = this._services.GetAllUsers(user.Id);
        if (existingUsers.Any(u => u.Username == user.Username.Trim()))
        {
            this.ViewBag.AlertMessage = "Username already exists.";
            return this.View();
        }

        if (this._services.UpdateUser(user)) return this.RedirectToAction("HomePage");
        return this.BadRequest();
    }

    public IActionResult Register()
    {
        var viewModel = new CreateViewModel { Roles = this._roleServices.GetAllRoles().ToList(), User = new User() };
        return this.View(viewModel);
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        var viewModel = new CreateViewModel { Roles = this._roleServices.GetAllRoles().ToList(), User = new User() };
        try
        {
            if (user.Password.Length < 8 || !user.Password.Any(char.IsLetter) || user.Password == null)
            {
                this.ViewBag.AlertMessage =
                    "Password must be at least 8 characters long and contain at least one letter.";
                return this.View(viewModel);
            }

            if (!this.IsValidPhoneNumber(user.NumberPhone))
            {
                this.ViewBag.AlertMessage = "Please enter a valid phone number.";
                return this.View(viewModel);
            }

            if (!this.IsValidEmail(user.Email))
            {
                this.ViewBag.AlertMessage = "Please enter a valid email.";
                return this.View(viewModel);
            }

            if (this.IsValidName(user.Name))
            {
                this.ViewBag.AlertMessage = "Please enter a valid name.";
                return this.View(viewModel);
            }

            if (this._services.GetAllUsers().Any(u => u.Username == user.Username.Trim()))
            {
                this.ViewBag.AlertMessage = "Username already exists.";
                return this.View(viewModel);
            }

            if (this._services.CreateNewUsers(user)) ;

            return this.RedirectToAction("Login");
        }
        catch (Exception e)
        {
            return this.Content(e.Message);
        }

        return this.Content("Not User");
    }

    public IActionResult SingOut()
    {
        this.HttpContext.Session.Remove("idUsers");
        return this.RedirectToAction("Login");
    }
}