namespace Colo_Shop.Controllers;

using System.Text.RegularExpressions;

using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;

using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly IRoleServices _roleServices;

    private readonly IUserServices _userServices;

    public UserController()
    {
        this._userServices = new UserServices();
        this._roleServices = new RoleServices();
    }

    public IActionResult Create()
    {
        var viewModel = new CreateViewModel { Roles = this._roleServices.GetAllRoles().ToList(), User = new User() };
        return this.View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(User user)
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

            if (this._userServices.GetAllUsers().Any(u => u.Username == user.Username.Trim()))
            {
                this.ViewBag.AlertMessage = "Username already exists.";
                return this.View(viewModel);
            }

            if (this._userServices.CreateNewUsers(user)) ;

            return this.RedirectToAction("ShowList");
        }
        catch (Exception e)
        {
            return this.Content(e.Message);
        }

        return this.Content("Not User");
    }

    public IActionResult Delete(Guid id)
    {
        this._userServices.DeleteUser(id);
        return this.RedirectToAction("ShowList");
    }

    public IActionResult Details(Guid id)
    {
        var p = this._userServices.GetUserById(id);
        return this.View(p);
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        // var viewModel = new CreateViewModel
        // {
        // Roles = _roleServices.GetAllRoles().ToList(),
        // User = _userServices.GetUserById(id)
        // };
        return this.View(this._userServices.GetUserById(id));
    }

    public IActionResult Edit(User user)
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

        var existingUsers = this._userServices.GetAllUsers(user.Id);
        if (existingUsers.Any(u => u.Username.Trim() == user.Username.Trim() && u.Id != user.Id))
        {
            this.ViewBag.AlertMessage = "Username already exists.";
            return this.View();
        }


        if (this._userServices.UpdateUser(user)) return this.RedirectToAction("ShowList");
        return this.BadRequest();
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

    public IActionResult ShowList()
    {
        var ShowModel = new ShowModel
        {
            Roles = this._roleServices.GetAllRoles().ToList(),
            User = this._userServices.GetAllUsers().ToList()
        };
        return this.View(ShowModel);
    }
    [HttpPost]
    public IActionResult SearchByUser(string username)
    {
        var ShowModel = new ShowModel
        {
            Roles = this._roleServices.GetAllRoles().ToList(),
            User = this._userServices.GetAllUsers().Where(p => p.Username.ToLower().Contains(username.ToLower().Trim())).ToList()
        };
        return this.View("ShowList", ShowModel);
    }
    public class CreateViewModel
    {
        public List<Role> Roles { get; set; }

        public User User { get; set; }
    }
    public class ShowModel
    {
        public List<Role> Roles { get; set; }

        public List<User> User { get; set; }
    }
}