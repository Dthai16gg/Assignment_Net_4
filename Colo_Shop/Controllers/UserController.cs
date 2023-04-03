using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;

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
        var viewModel = new ShowModel
        {
            Roles = _roleServices.GetAllRoles().ToList(),
            User = _userServices.GetAllUsers().ToList()
        };
        return View(viewModel);
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

    [HttpPost]
    public async Task<IActionResult> Create(User user, IFormFile imageFile)
    {
        if (imageFile != null)
            using (var stream = new MemoryStream())
            {
                await imageFile.CopyToAsync(stream);
                user.ImageUser = stream.ToArray();
            }

        try
        {
            if (_userServices.CreateNewUsers(user)) return RedirectToAction("ShowList");
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
        var viewModel = new CreateViewModel
        {
            Roles = _roleServices.GetAllRoles().ToList(),
            User = _userServices.GetUserById(id)
        };
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CreateViewModel viewModel, IFormFile imageFile)
    {
        if (ModelState.IsValid)
        {
            if (imageFile != null)
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    viewModel.User.ImageUser = stream.ToArray();
                }

            try
            {
                if (_userServices.UpdateUser(viewModel.User)) return RedirectToAction("ShowList");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }
        }

        viewModel.Roles = _roleServices.GetAllRoles().ToList();
        return RedirectToAction("ShowList");
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

public class ViewModel
{
    public List<User> User { get; set; }
    public List<Role> Role { get; set; }
}