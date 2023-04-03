using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers;

public class RoleController : Controller
{
    private readonly IRoleServices _roleServices;

    public RoleController()
    {
        _roleServices = new RoleServices();
    }

    public IActionResult ShowList()
    {
        var role = _roleServices.GetAllRoles();
        return View(role.ToList());
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Role role)
    {
        _roleServices.CreateNewRoles(role);
        return RedirectToAction("ShowList");
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var role = _roleServices.GetRoleById(id);
        return View(role);
    }

    public IActionResult Edit(Role role)
    {
        if (_roleServices.UpdateRole(role)) return RedirectToAction("ShowList");
        return BadRequest();
    }

    public IActionResult Delete(Guid id)
    {
        _roleServices.DeleteRole(id);
        return RedirectToAction("ShowList");
    }

    public IActionResult Details(Guid id)
    {
        var p = _roleServices.GetRoleById(id);
        return View(p);
    }
}