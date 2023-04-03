using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Colo_Shop.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
    
    public IActionResult ForgotPassword()
    {
        return View();
    }
}