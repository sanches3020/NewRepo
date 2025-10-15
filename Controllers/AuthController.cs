using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;
using System.Security.Cryptography;
using System.Text;

namespace Sofia.Web.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly SofiaDbContext _context;

    public AuthController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

        if (user == null || !VerifyPassword(password, user.Password))
        {
            ModelState.AddModelError("", "Неверное имя пользователя или пароль");
            return View();
        }

        // Простая аутентификация через сессию
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserRole", user.Role);
        HttpContext.Session.SetString("Username", user.Username);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string email, string password, string confirmPassword, string role = "user")
    {
        if (password != confirmPassword)
        {
            ModelState.AddModelError("", "Пароли не совпадают");
            return View();
        }

        if (await _context.Users.AnyAsync(u => u.Username == username))
        {
            ModelState.AddModelError("", "Пользователь с таким именем уже существует");
            return View();
        }

        if (await _context.Users.AnyAsync(u => u.Email == email))
        {
            ModelState.AddModelError("", "Пользователь с таким email уже существует");
            return View();
        }

        var user = new User
        {
            Username = username,
            Email = email,
            Password = HashPassword(password),
            Role = role,
            CreatedAt = DateTime.Now
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Если это психолог, создаем профиль психолога
        if (role == "psychologist")
        {
            var psychologist = new Psychologist
            {
                Name = username,
                UserId = user.Id,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            _context.Psychologists.Add(psychologist);
            await _context.SaveChangesAsync();
        }

        // Автоматический вход после регистрации
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserRole", user.Role);
        HttpContext.Session.SetString("Username", user.Username);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return HashPassword(password) == hashedPassword;
    }
}
