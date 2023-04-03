using System.Net.Mail;
using System.Text;
using Colo_Shop.IServices;
using Colo_Shop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Colo_Shop.Controllers;

public class AdminController : Controller
{
    private readonly IRoleServices _roleServices;
    private readonly IUserServices _services;

    public AdminController()
    {
        _services = new UserServices();
        _roleServices = new RoleServices();
    }

    public IActionResult Index()
    {
        return RedirectToAction("Login");
    }

    public IActionResult HomePage()
    {
        return View();
    }

    public bool CheckLogin(string username, string password)
    {
        var user = _services.GetUserByName(username).FirstOrDefault();
        if (user != null && user.Password == password && user.Status == 1)
        {
            var role = _roleServices.GetRoleById(user.RoleId);
            if (role.RoleName == "Admin") return true;
        }

        return false;
    }

    public IActionResult Login()
    {
        return View();
    }

    public void SendEmail(string fromEmail, string toEmail, string subject, string message)
    {
        // Set up the email message
        var mail = new MailMessage(fromEmail, toEmail, subject, message);
        mail.BodyEncoding = Encoding.UTF8;
        mail.SubjectEncoding = Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.ReplyToList.Add(new MailAddress(fromEmail));
        mail.Sender = new MailAddress(fromEmail);
        using var smtpClient = new SmtpClient("localhost");
        try
        {
            smtpClient.Send(mail);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public IActionResult Forgot(string username, string email)
    {
        var user = _services.GetUserByName(username).FirstOrDefault();
        if (user != null && user.Email == email)
        {
            // Generate new password
            var newPassword = GeneratePassword();
            user.Password = newPassword;
            _services.UpdateUser(user);
            SendEmail("thaibdph23339@fpt.edu.vn", user.Email, "Your New Password",
                $"Your new password is: {newPassword}");

            return RedirectToAction("Login");
        }

        return RedirectToAction("Login");
    }

    private string GeneratePassword()
    {
        // Generate a new, random password
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var chars = new char[8];
        for (var i = 0; i < chars.Length; i++) chars[i] = validChars[random.Next(validChars.Length)];
        return new string(chars);
    }

    [HttpPost]
    public IActionResult Login(string Username, string Password)
    {
        var isValid = CheckLogin(Username, Password);
        if (isValid) return RedirectToAction("HomePage");

        ViewBag.ErrorMessage = "The user name or password provided is incorrect.";
        return View("Login");
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult MyAccount()
    {
        return View();
    }
}