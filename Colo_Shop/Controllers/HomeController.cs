using static Colo_Shop.Controllers.UserController;

namespace Colo_Shop.Controllers;

using System.Diagnostics;
using System.Text.RegularExpressions;

using Colo_Shop.IServices;
using Colo_Shop.Models;
using Colo_Shop.Services;

using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IBillDetailsServices _billDetailsServices; // = new BillDetailsService();

    private readonly IBillServices _billServices; // = new BillServices();

    private readonly ICartDetailsServices _cartDetailsServices;

    private readonly ICartServices _cartServices;

    private readonly ILogger<HomeController> _logger;

    private readonly IProductServices _productServices;

    private readonly IRoleServices _roleServices;

    private readonly IUserServices _userservices;

    public HomeController(ILogger<HomeController> logger)
    {
        this._logger = logger;
        this._productServices = new ProductServices();
        this._userservices = new UserServices();
        this._roleServices = new RoleServices();
        this._cartDetailsServices = new CartDetailService();
        this._cartServices = new CartServices();
        this._billDetailsServices = new BillDetailsService();
        this._billServices = new BillServices();
    }

    public IActionResult About()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        this.ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
            return this.View();
        return this.RedirectToAction("LoginPage");
    }

    public IActionResult AddToBill()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        if (string.IsNullOrEmpty(idUser)) return this.RedirectToAction("LoginPage");
        var user = this._userservices.GetUserById(Guid.Parse(idUser));
        var cart = this._cartServices.GetCartByUserId(user.Id);
        if (cart == null) return this.View("Shop");
        return this.View(cart);
    }

    // create action for add to bill with list id product and list quantity and status bill
    [HttpPost]
    public IActionResult AddToBill(List<Guid> productId, List<int> quantities, int Status)
    {
        if (productId.Count > 0 && quantities.Count > 0 && (Status == 1 || Status == 0))
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            var idUser = this.HttpContext.Session.GetString("idUser");
            if (string.IsNullOrEmpty(idUser)) return this.RedirectToAction("LoginPage");

            // Lấy thông tin người dùng
            var user = this._userservices.GetUserById(Guid.Parse(idUser));

            // Lấy thông tin giỏ hàng của người dùng
            var cart = this._cartServices.GetCartByUserId(user.Id);

            // Kiểm tra xem giỏ hàng có tồn tại hay không
            if (cart == null)

                // Nếu giỏ hàng không tồn tại thì trả về trang Shop
                return this.View("Shop");

            var bill = new Bill
            {
                Id = Guid.NewGuid(), // ID hóa đơn
                UserID = user.Id, // ID người dùng
                CreateDate = Convert.ToDateTime(DateTime.Now.ToString()), // Ngày tạo hóa đơn
                Status = Status, // Trạng thái hóa đơn//Khởi tạo danh sách chi tiết hóa đơn,
                Details = new List<BillDetails>()
            };
            for (var i = 0; i < productId.Count; i++)
            {
                // Lấy thông tin sản phẩm
                var product = this._productServices.GetProductById(productId[i]);

                // Tạo một đối tượng BillDetails
                var detail = new BillDetails
                {
                    Id = Guid.NewGuid(), // ID chi tiết hóa đơn
                    Quantity = quantities[i], // Số lượng sản phẩm
                    Price = product.Price, // Giá sản phẩm
                    IdHD = bill.Id, // ID hóa đơn
                    IdSp = product.Id // ID sản phẩm
                };
                if (Status == 1 || Status == 0 && product.AvailableQuantity > quantities[i])
                {
                    product.AvailableQuantity = product.AvailableQuantity - quantities[i]; // sau khi thêm thì phải trừ số lượng đó đi ở trong db theo sp
                    this._productServices.UpdateProduct(product);
                    bill.Details.Add(detail);
                }
                else
                {
                    return this.Content("Hết hàng rồi");
                }
            }

            // Lưu hóa đơn và chi tiết hóa đơn vào cơ sở dữ liệu
            if (this._billServices.CreateNewBills(bill))
            {
                this._cartServices.DeleteCart(cart.Id);
                return this.RedirectToAction("Shop");
            }

            // Trở về giỏ hàng
        }
        else
        {
            return this.Content("fake");
        }

        return this.Content("Sai roi");
    }

    [HttpPost]
    public  IActionResult AddToCart(Guid productId, int Quantity)
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        if (!string.IsNullOrEmpty(idUser))
        {
            var user = this._userservices.GetUserById(Guid.Parse(idUser));
            var cart = this._cartServices.GetCartByUserId(user.Id);
            if (cart == null)
            {
                cart = new Cart { UserId = user.Id, Details = new List<CartDetail>() };
                this._cartServices.CreateNewCarts(cart);
            }

            var product = this._productServices.GetProductById(productId);
            if (product == null) return this.NotFound();                
            var cartDetail = cart.Details.FirstOrDefault(d => d.Product.Id == productId);

            if (cartDetail == null)
            {
                // Add a new cart detail if the product is not in the cart
                cartDetail = new CartDetail { IdSp = product.Id, CartId = cart.Id, Quantity = Quantity };
                cart.Details.Add(cartDetail);
            }
            else
            {
                cartDetail.Quantity = cartDetail.Quantity + Quantity;

            }

            this._cartServices.UpdateCart(cart);
            return this.RedirectToAction("ShowCart");
        }

        return this.RedirectToAction("LoginPage");
    }

    public IActionResult Blog()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        this.ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
            return this.View();
        return this.RedirectToAction("LoginPage");
    }

    public bool CheckLogin(string username, string password)
    {
        var user = this._userservices.GetUserByUserName(username.Trim()).FirstOrDefault();
        if (user != null && user.Password == password && user.Status == 1)
        {
            var role = this._roleServices.GetRoleById(user.RoleId);
            if (role.RoleName != "Admin") return true;
        }

        return false;
    }

    public IActionResult Contact()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        this.ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
            return this.View();
        return this.RedirectToAction("LoginPage");
    }

    public IActionResult Delete(Guid id)
    {
        this._userservices.DeleteUser(id);
        return this.RedirectToAction("LoginPage");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
    }

    public IActionResult ForgotPassword()
    {
        return this.View();
    }

    public IActionResult Index()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        this.ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
        {
            var listProduct = this._productServices.GetAllProducts();
            return this.View(listProduct.ToList());
        }

        return this.RedirectToAction("LoginPage");
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

    public IActionResult LoginPage()
    {
        return this.View();
    }

    [HttpPost]
    public IActionResult LoginPage(string Username, string Password)
    {
        var isValid = this.CheckLogin(Username, Password);
        if (isValid)
        {
            var user = this._userservices.GetUserByUserName(Username).FirstOrDefault();
            var idUser = user.Id.ToString();
            this.HttpContext.Session.SetString("idUser", idUser);
            if (user != null)
                return this.RedirectToAction("Index");
            this.ViewData["ErrorMessage"] = "User not found";
        }
        else
        {
            this.ViewBag.ErrorMessage = "The user name or password provided is incorrect.";
        }

        return this.View("LoginPage");
    }

    public IActionResult MyAccount()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        this.ViewData["idUser"] = idUser;
         var usr= this._userservices.GetUserById(Guid.Parse(idUser));
        if (!string.IsNullOrEmpty(idUser) && usr != null)
            return this.View(usr);
        return this.RedirectToAction("LoginPage");
    }
    public IActionResult GetBillDetails(Guid billId)
    {
        var bill = this._billServices.GetBillById(billId);
        return PartialView("PopupView", bill);
    }

    public IActionResult PopupView()
    {
        return this.View();
    }

    public IActionResult ProductDetails(Guid id)
    {
        var product = this._productServices.GetProductById(id);
        return this.View(product);
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

            if (this._userservices.GetAllUsers().Any(u => u.Username == user.Username.Trim()))
            {
                this.ViewBag.AlertMessage = "Username already exists.";
                return this.View(viewModel);
            }

            if (this._userservices.CreateNewUsers(user)) ;

            return this.RedirectToAction("LoginPage");
        }
        catch (Exception e)
        {
            return this.Content(e.Message);
        }
    }

    // Xóa sản phẩm khỏi giỏ hàng
    public IActionResult RemoveFromCart(Guid id)
    {
        this._cartDetailsServices.DeleteCartDetail(id);
        return this.RedirectToAction("ShowCart");
    }

    public IActionResult Search(string name)
    {
        var listProduct = this._productServices.GetProductByName(name);
        if (listProduct.Count == 0)
        {
            var list = this._productServices.GetAllProducts();
            return this.View("Shop", list.ToList());
        }

        this.ViewData["urlShop"] = $"Search Name : {name}";
        return this.View("Shop", listProduct.ToList());
    }

    public IActionResult Shop()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        this.ViewData["idUser"] = idUser;
        if (!string.IsNullOrEmpty(idUser))
        {
            var listProduct = this._productServices.GetAllProducts();
            this.ViewData["urlShop"] = "All";
            return this.View(listProduct.ToList());
        }

        return this.RedirectToAction("LoginPage");
    }

    public IActionResult ShowCart()
    {
        var idUser = this.HttpContext.Session.GetString("idUser");
        if (string.IsNullOrEmpty(idUser)) return this.RedirectToAction("LoginPage");
        this.ViewData["idUser"] = idUser;
        var user = this._userservices.GetUserById(Guid.Parse(idUser));
        var cart = this._cartServices.GetCartByUserId(user.Id);
        if (cart == null)
        {
            // Create a new cart if one doesn't exist
            cart = new Cart { Description = "1", UserId = user.Id };
            this._cartServices.CreateNewCarts(cart);
        }

        return this.View(cart);
    }

    public IActionResult SingOut()
    {
        this.HttpContext.Session.Remove("idUser");
        return this.RedirectToAction("LoginPage");
    }
}