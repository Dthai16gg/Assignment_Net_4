using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;


namespace Colo_Shop.Controllers;

public class UserController : Controller
{
    private readonly IRoleServices _roleServices;
    private readonly IUserServices _userServices;

    public UserController()
    {
        _userServices = new UserServices();
        _roleServices = new RoleServices();
    }

    public IActionResult ShowList()
    {
        var ShowModel = new ShowModel
        {
            Roles = _roleServices.GetAllRoles().ToList(),
            User = _userServices.GetAllUsers().ToList()
        };
        return View(ShowModel);
    }

    public IActionResult Create()
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
        Regex regex = new Regex(@"^(03|05|07|08|09)[0-9]{8}$");
        return regex.IsMatch(phoneNumber);
    }
    public bool IsValidEmail(string email)
    {
        Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        return regex.IsMatch(email);
    }
    public bool IsValidName(string name)
    {
        Regex regex = new Regex(@"^[a-zA-Z]+$");
        return regex.IsMatch(name);
    }
    [HttpPost]
    public IActionResult Create(User user)
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
            if (_userServices.GetAllUsers().Any(u => u.Username == user.Username.Trim()))
            {
                ViewBag.AlertMessage = "Username already exists.";
                return View(viewModel);
            }
            else if (_userServices.CreateNewUsers(user)) ; return RedirectToAction("ShowList");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
        return Content("Not User");
    }
    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        //var viewModel = new CreateViewModel
        //{
        //    Roles = _roleServices.GetAllRoles().ToList(),
        //    User = _userServices.GetUserById(id)
        //};
        return View(_userServices.GetUserById(id));
    }
    
    public IActionResult Edit(User user)
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
        var existingUsers = _userServices.GetAllUsers(user.Id);
        if (existingUsers.Any(u => u.Username == user.Username.Trim()))
        {
            ViewBag.AlertMessage = "Username already exists.";
            return View();
        }
        else if (_userServices.UpdateUser(user))
        {
            return RedirectToAction("ShowList");
        }
        return BadRequest();
    }


    public IActionResult Delete(Guid id)
    {
        _userServices.DeleteUser(id);
        return RedirectToAction("ShowList");
    }

    public IActionResult Details(Guid id)
    {
        var p = _userServices.GetUserById(id);
        return View(p);
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

