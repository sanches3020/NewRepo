using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Models;
using Sofia.Web.Data;

namespace Sofia.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        // Если это психолог, перенаправляем на дашборд
        if (!string.IsNullOrEmpty(userId) && userRole == "psychologist")
        {
            using var scope = HttpContext.RequestServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<Sofia.Web.Data.SofiaDbContext>();
            var psychologist = await context.Psychologists
                .FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));
            if (psychologist != null)
            {
                return RedirectToAction("Dashboard", "Psychologist", new { id = psychologist.Id });
            }
        }
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Companion()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }
        return View();
    }

    [HttpPost("api/onboarding/complete")]
    public IActionResult CompleteOnboarding()
    {
        HttpContext.Session.SetString("OnboardingCompleted", "true");
        return Ok();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
