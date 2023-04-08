using System.Diagnostics;
using System.Text.RegularExpressions;
using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;
using static Colo_Shop.Controllers.UserController;

namespace Colo_Shop.Controllers;

public class HomeController : Controller
{
    private readonly IBillDetailsServices _billDetailsServices;
    private readonly IBillServices _billServices;
    private readonly ICartDetailsServices _cartDetailsServices;
    private readonly ICartServices _cartServices;
    private readonly ILogger<HomeController> _logger;
    private readonly IProductServices _productServices;
    private readonly IRoleServices _roleServices;
    private readonly IUserServices _userservices;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        _productServices = new ProductServices();
        _userservices = new UserServices();
        _roleServices = new RoleServices();
        _cartDetailsServices = new CartDetailService();
        _cartServices = new CartServices();
        _billDetailsServices = new BillDetailsService();
        _billServices = new BillServices();
    }

    public IActionResult Index()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
        {
            var listProduct = _productServices.GetAllProducts();
            return View(listProduct.ToList());
        }

        return RedirectToAction("LoginPage");
    }

    public bool CheckLogin(string username, string password)
    {
        var user = _userservices.GetUserByUserName(username).FirstOrDefault();
        if (user != null && user.Password == password && user.Status == 1)
        {
            var role = _roleServices.GetRoleById(user.RoleId);
            if (role.RoleName != "Admin") return true;
        }

        return false;
    }

    public IActionResult Shop()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
        {
            var listProduct = _productServices.GetAllProducts();
            ViewData["urlShop"] = "All";
            return View(listProduct.ToList());
        }

        return RedirectToAction("LoginPage");
    }

    public IActionResult Search(string name)
    {
        var listProduct = _productServices.GetProductByName(name);
        if (listProduct.Count == 0)
        {
            var list = _productServices.GetAllProducts();
            return View("Shop", list.ToList());
        }

        ViewData["urlShop"] = $"Search Name : {name}";
        return View("Shop", listProduct.ToList());
    }

    public IActionResult Delete(Guid id)
    {
        _userservices.DeleteUser(id);
        return RedirectToAction("LoginPage");
    }

    public IActionResult Contact()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
            return View();
        return RedirectToAction("LoginPage");
    }

    public IActionResult Blog()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
            return View();
        return RedirectToAction("LoginPage");
    }

    public IActionResult About()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
            return View();
        return RedirectToAction("LoginPage");
    }

    public IActionResult MyAccount()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
            return View();
        return RedirectToAction("LoginPage");
    }

    public IActionResult ProductDetails(Guid id)
    {
        var product = _productServices.GetProductById(id);
        return View(product);
    }

    public IActionResult ShowCart()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        if (string.IsNullOrEmpty(idUser)) return RedirectToAction("LoginPage");
        ViewData["idUser"] = idUser;
        var user = _userservices.GetUserById(Guid.Parse(idUser));
        var cart = _cartServices.GetCartByUserId(user.Id);
        if (cart == null)
        {
            // Create a new cart if one doesn't exist
            cart = new Cart
            {
                Description = "1",
                UserId = user.Id
            };
            _cartServices.CreateNewCarts(cart);
        }
        return View(cart);
    }

    [HttpPost]
    public IActionResult AddToCart(Guid productId)
    {
        var idUser = HttpContext.Session.GetString("idUser");
        if (!string.IsNullOrEmpty(idUser))
        {
            var user = _userservices.GetUserById(Guid.Parse(idUser));
            var cart = _cartServices.GetCartByUserId(user.Id);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = user.Id
                };
                _cartServices.CreateNewCarts(cart);
            }

            var product = _productServices.GetProductById(productId);
            if (product == null) return NotFound();
            var cartDetail = cart.Details.FirstOrDefault(d => d.Product.Id == productId);

            if (cartDetail == null)
            {
                // Add a new cart detail if the product is not in the cart
                cartDetail = new CartDetail
                {
                    IdSp = product.Id,
                    CartId = cart.Id,
                    Quantity = 1
                };
                cart.Details.Add(cartDetail);
            }
            else
            {
                cartDetail.Quantity++;
            }

            _cartServices.UpdateCart(cart);
            return RedirectToAction("ShowCart");
        }

        return RedirectToAction("LoginPage");
    }
    public IActionResult AddToBill()
    {
        var idUser = HttpContext.Session.GetString("idUser");
        if (string.IsNullOrEmpty(idUser)) return RedirectToAction("LoginPage");
        var user = _userservices.GetUserById(Guid.Parse(idUser));
        var cart = _cartServices.GetCartByUserId(user.Id);
        if (cart == null)
        {
            return View("Shop");
        }
        return View(cart);
    }
    public IActionResult RemoveFromCart(Guid id)
    {
        _cartDetailsServices.DeleteCartDetail(id);
        return RedirectToAction("ShowCart");
    }

    public IActionResult LoginPage()
    {
        return View();
    }

    [HttpPost]
    public IActionResult LoginPage(string Username, string Password)
    {
        var isValid = CheckLogin(Username, Password);
        if (isValid)
        {
            var user = _userservices.GetUserByUserName(Username).FirstOrDefault();
            var idUser = user.Id.ToString();
            HttpContext.Session.SetString("idUser", idUser);
            if (user != null)
                return RedirectToAction("Index");
            ViewData["ErrorMessage"] = "User not found";
        }
        else
        {
            ViewBag.ErrorMessage = "The user name or password provided is incorrect.";
        }

        return View("LoginPage");
    }

    public IActionResult SingOut()
    {
        HttpContext.Session.Remove("idUser");
        return RedirectToAction("LoginPage");
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

            if (_userservices.GetAllUsers().Any(u => u.Username == user.Username.Trim()))
            {
                ViewBag.AlertMessage = "Username already exists.";
                return View(viewModel);
            }

            if (_userservices.CreateNewUsers(user)) ;

            return RedirectToAction("LoginPage");
        }
        catch (Exception e)
        {
            return Content(e.Message);
        }
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}