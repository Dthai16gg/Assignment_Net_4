namespace Colo_Shop.Controllers;

using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;

using Microsoft.AspNetCore.Mvc;

public class RoleController : Controller
{
    private readonly IRoleServices _roleServices;

    public RoleController()
    {
        this._roleServices = new RoleServices();
    }

    public IActionResult Create()
    {
        return this.View();
    }

    [HttpPost]
    public IActionResult Create(Role role)
    {
        this._roleServices.CreateNewRoles(role);
        return this.RedirectToAction("ShowList");
    }

    public IActionResult Delete(Guid id)
    {
        this._roleServices.DeleteRole(id);
        return this.RedirectToAction("ShowList");
    }

    public IActionResult Details(Guid id)
    {
        var p = this._roleServices.GetRoleById(id);
        return this.View(p);
    }

    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        var role = this._roleServices.GetRoleById(id);
        return this.View(role);
    }

    public IActionResult Edit(Role role)
    {
        if (this._roleServices.UpdateRole(role)) return this.RedirectToAction("ShowList");
        return this.BadRequest();
    }

    public IActionResult ShowList()
    {
        var role = this._roleServices.GetAllRoles();
        return this.View(role.ToList());
    }
}