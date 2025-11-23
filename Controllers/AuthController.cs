using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
        username = (username ?? string.Empty).Trim();
        password = (password ?? string.Empty);

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Введите имя пользователя и пароль");
            return View();
        }

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

        // Если это психолог, сохраняем ID психолога и перенаправляем на дашборд
        if (user.Role == "psychologist")
        {
            var psychologist = await _context.Psychologists
                .FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (psychologist != null)
            {
                HttpContext.Session.SetString("PsychologistId", psychologist.Id.ToString());
                return RedirectToAction("Dashboard", "Psychologist", new { id = psychologist.Id });
            }
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string username, string email, string password, string confirmPassword, string role = "user", 
        string? specialization = null, string? description = null, string? education = null, string? experience = null, 
        string? languages = null, string? methods = null, decimal? pricePerHour = null, string? contactPhone = null)
    {
        username = (username ?? string.Empty).Trim();
        email = (email ?? string.Empty).Trim();
        role = (role ?? "user").Trim();

        if (string.IsNullOrWhiteSpace(username) || username.Length < 3)
        {
            ModelState.AddModelError("", "Имя пользователя должно содержать минимум 3 символа");
            return View();
        }

        if (string.IsNullOrWhiteSpace(email) || !new EmailAddressAttribute().IsValid(email))
        {
            ModelState.AddModelError("", "Введите корректный email");
            return View();
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            ModelState.AddModelError("", "Пароль должен содержать минимум 8 символов");
            return View();
        }

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
                Specialization = specialization ?? "",
                Description = description ?? "",
                Education = education ?? "",
                Experience = experience ?? "",
                Languages = languages,
                Methods = methods,
                PricePerHour = pricePerHour ?? 3000,
                ContactPhone = contactPhone,
                ContactEmail = email,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            _context.Psychologists.Add(psychologist);
            await _context.SaveChangesAsync();
            
            // Создаем базовое расписание для нового психолога (понедельник-пятница, 10:00-18:00)
            var schedules = new List<PsychologistSchedule>();
            for (int day = 1; day <= 5; day++) // Понедельник-Пятница
            {
                schedules.Add(new PsychologistSchedule
                {
                    PsychologistId = psychologist.Id,
                    DayOfWeek = (DayOfWeek)day,
                    StartTime = new TimeSpan(10, 0, 0),
                    EndTime = new TimeSpan(18, 0, 0),
                    IsAvailable = true,
                    CreatedAt = DateTime.Now
                });
            }
            _context.PsychologistSchedules.AddRange(schedules);
            await _context.SaveChangesAsync();
            
            // Сохраняем ID психолога в сессию
            HttpContext.Session.SetString("PsychologistId", psychologist.Id.ToString());
        }

        // Автоматический вход после регистрации
        HttpContext.Session.SetString("UserId", user.Id.ToString());
        HttpContext.Session.SetString("UserRole", user.Role);
        HttpContext.Session.SetString("Username", user.Username);

        // Если это психолог, перенаправляем на дашборд
        if (user.Role == "psychologist")
        {
            var psychologist = await _context.Psychologists
                .FirstOrDefaultAsync(p => p.UserId == user.Id);
            if (psychologist != null)
            {
                return RedirectToAction("Dashboard", "Psychologist", new { id = psychologist.Id });
            }
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("forgot-password")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword(string emailOrUsername)
    {
        emailOrUsername = (emailOrUsername ?? string.Empty).Trim();

        if (string.IsNullOrWhiteSpace(emailOrUsername))
        {
            ModelState.AddModelError("", "Введите email или имя пользователя");
            return View();
        }

        var user = await _context.Users
            .FirstOrDefaultAsync(u => (u.Email == emailOrUsername || u.Username == emailOrUsername) && u.IsActive);

        if (user == null)
        {
            // Не показываем, что пользователь не найден (защита от перебора)
            ViewBag.Message = "Если аккаунт с таким email или именем пользователя существует, инструкции по восстановлению пароля будут отправлены.";
            return View();
        }

        // В реальном приложении здесь бы отправлялся email с токеном сброса
        // Для простоты, передаем идентификатор пользователя через сессию
        HttpContext.Session.SetString("ResetPasswordUserId", user.Id.ToString());
        HttpContext.Session.SetString("ResetPasswordEmail", user.Email);

        return RedirectToAction("ResetPassword");
    }

    [HttpGet("reset-password")]
    public IActionResult ResetPassword()
    {
        var userId = HttpContext.Session.GetString("ResetPasswordUserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("ForgotPassword");
        }

        var email = HttpContext.Session.GetString("ResetPasswordEmail");
        ViewBag.Email = email;
        return View();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(string password, string confirmPassword)
    {
        var userIdStr = HttpContext.Session.GetString("ResetPasswordUserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
        {
            ModelState.AddModelError("", "Сессия истекла. Пожалуйста, начните процесс восстановления пароля заново.");
            return RedirectToAction("ForgotPassword");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
        {
            ModelState.AddModelError("", "Пароль должен содержать минимум 8 символов");
            var email = HttpContext.Session.GetString("ResetPasswordEmail");
            ViewBag.Email = email;
            return View();
        }

        if (password != confirmPassword)
        {
            ModelState.AddModelError("", "Пароли не совпадают");
            var email = HttpContext.Session.GetString("ResetPasswordEmail");
            ViewBag.Email = email;
            return View();
        }

        var user = await _context.Users.FindAsync(userId);
        if (user == null || !user.IsActive)
        {
            ModelState.AddModelError("", "Пользователь не найден");
            HttpContext.Session.Remove("ResetPasswordUserId");
            HttpContext.Session.Remove("ResetPasswordEmail");
            return RedirectToAction("ForgotPassword");
        }

        user.Password = HashPassword(password);
        await _context.SaveChangesAsync();

        // Очищаем сессию восстановления
        HttpContext.Session.Remove("ResetPasswordUserId");
        HttpContext.Session.Remove("ResetPasswordEmail");

        ViewBag.Success = true;
        return View();
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
