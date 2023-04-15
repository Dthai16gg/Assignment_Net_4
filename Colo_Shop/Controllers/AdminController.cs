using static Colo_Shop.Controllers.UserController;

namespace Colo_Shop.Controllers;

using System.Text.RegularExpressions;

using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;

using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
  private readonly IRoleServices _roleServices; // khai báo biến _roleServices kiểu IRoleServices

  private readonly IUserServices _services; // khai báo biến _services kiểu IUserServices

  public AdminController()
  {
    this._services = new UserServices(); // khởi tạo biến _services kiểu IUserServices bằng UserServices (đã implement IUserServices) để có thể sử dụng các phương thức của UserServices
    this._roleServices = new RoleServices();
  }
  // Compare this snippet from Colo_Shop\Controllers\HomeController.cs:
  public bool CheckLogin(string username, string password) // phương thức CheckLogin kiểm tra username và password có đúng hay không
  {
    var user = this._services.GetUserByUserName(username).FirstOrDefault(); // lấy user theo username
    // nếu user khác null và password của user đó trùng với password truyền vào và status của user đó là 1 (đã kích hoạt)
    if (user != null && user.Password == password && user.Status == 1)
    {
      var role = this._roleServices.GetRoleById(user.RoleId); // lấy role của user đó
      if (role.RoleName == "Admin") return true; // nếu role của user đó là Admin thì trả về true
    }
    return false;
  }

  public IActionResult HomePage()
  {
    var idUsers = this.HttpContext.Session.GetString("idUsers"); // lấy idUsers từ session để hiển thị thông tin user
    this.ViewData["idUsers"] = idUsers; // truyền idUsers vào ViewData để hiển thị thông tin user
    if (!string.IsNullOrEmpty(idUsers)) // nếu idUsers khác null
      return this.View(); // trả về view HomePage
    return this.RedirectToAction("Login"); // nếu idUsers bằng null thì trả về view Login
  }

  public IActionResult Index()
  {
    return this.RedirectToAction("Login"); // trả về view Login
  }

  public bool IsValidEmail(string email) // phương thức kiểm tra email có đúng định dạng hay không
  {
    var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"); // định dạng email phải có @ và .
    return regex.IsMatch(email); // trả về true nếu email đúng định dạng và ngược lại
  }

  public bool IsValidName(string name) // phương thức kiểm tra tên có đúng định dạng hay không
  {
    var regex = new Regex(@"^[a-zA-Z]+$"); // định dạng tên chỉ chứa chữ cái và không có khoảng trắng
    return regex.IsMatch(name); // trả về true nếu tên đúng định dạng và ngược lại
  }

  public bool IsValidPhoneNumber(string phoneNumber) // phương thức kiểm tra số điện thoại có đúng định dạng hay không
  {
    var regex = new Regex(@"^(03|05|07|08|09)[0-9]{8}$"); // định dạng số điện thoại phải bắt đầu bằng 03, 05, 07, 08, 09 và có 10 chữ số
    return regex.IsMatch(phoneNumber); // trả về true nếu số điện thoại đúng định dạng và ngược lại
  }

  public IActionResult Login()
  {
    return this.View(); // trả về view Login
  }

  [HttpPost] // phương thức Login được gọi khi submit form Login (phương thức này có 2 tham số là Username và Password)
  public IActionResult Login(string Username, string Password)
  {
    var isValid = this.CheckLogin(Username, Password); // gọi phương thức CheckLogin để kiểm tra username và password có đúng hay không
    if (isValid)
    {
      var user = this._services.GetUserByUserName(Username).FirstOrDefault();
      var idUsers = user.Id.ToString();// lấy id của user
      this.HttpContext.Session.SetString("idUsers", idUsers); // lưu idUsers vào session để hiển thị thông tin user
      if (user != null) //
        return this.RedirectToAction("HomePage"); // nếu user khác null thì trả về view HomePage
      this.ViewData["ErrorMessage"] = "User not found"; // nếu user bằng null thì hiển thị thông báo User not found
    }
    else
    {
      this.ViewBag.ErrorMessage = "The user name or password provided is incorrect."; // nếu username hoặc password không đúng thì hiển thị thông báo The user name or password provided is incorrect.
    }
    // nếu username hoặc password không đúng thì trả về view Login
    return this.View("Login");
  }

  public IActionResult MyAccount()
  {
    var idUsers = this.HttpContext.Session.GetString("idUsers"); // lấy idUsers từ session để hiển thị thông tin user
    this.ViewData["idUsers"] = idUsers;
    if (!string.IsNullOrEmpty(idUsers)) // nếu idUsers khác null
      return this.View();
    return this.RedirectToAction("Login");
  }

  [HttpPost] // phương thức MyAccount được gọi khi submit form MyAccount (phương thức này có 1 tham số là User)
  public IActionResult MyAccount(User user)
  {// nếu password của user truyền vào khác null và password của user truyền vào nhỏ hơn 8 hoặc không chứa chữ cái hoặc password của user truyền vào bằng null
    if (user.Password.Length < 8 || !user.Password.Any(char.IsLetter) || user.Password == null)
    {
      this.ViewBag.AlertMessage = "Password must be at least 8 characters long and contain at least one letter.";
      return this.View();
    }
    // nếu số điện thoại của user truyền vào không đúng định dạng
    if (!this.IsValidPhoneNumber(user.NumberPhone))
    {
      this.ViewBag.AlertMessage = "Please enter a valid phone number.";
      return this.View();
    }
    // nếu email của user truyền vào không đúng định dạng
    if (!this.IsValidEmail(user.Email))
    {
      this.ViewBag.AlertMessage = "Please enter a valid email.";
      return this.View();
    }
    // nếu tên của user truyền vào không đúng định dạng
    if (this.IsValidName(user.Name))
    {
      this.ViewBag.AlertMessage = "Please enter a valid name.";
      return this.View();
    }
    // nếu username của user truyền vào không đúng định dạng
    var existingUsers = this._services.GetAllUsers(user.Id); // lấy danh sách user
    if (existingUsers.Any(u => u.Username == user.Username.Trim())) // nếu username của user truyền vào đã tồn tại trong danh sách user thì hiển thị thông báo Username already exists.
    {
      this.ViewBag.AlertMessage = "Username already exists.";
      return this.View();
    }
    if (this._services.UpdateUser(user)) return this.RedirectToAction("HomePage"); // nếu update thành công thì trả về view HomePage
    return this.BadRequest(); //    nếu không thì trả về BadRequest
  }

  public IActionResult Register()
  {
    // khai báo viewModel
    var viewModel = new CreateViewModel { Roles = this._roleServices.GetAllRoles().ToList(), User = new User() };
    // trả về view Register
    return this.View(viewModel);
  }

  [HttpPost]
  public IActionResult Register(User user)
  {// nếu password của user truyền vào nhỏ hơn 8 hoặc không chứa chữ cái hoặc password của user truyền vào bằng null
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
    this.HttpContext.Session.Remove("idUsers"); // xóa idUsers trong session
    return this.RedirectToAction("Login");// trả về view Login
  }
}
