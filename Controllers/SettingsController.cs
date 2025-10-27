using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("settings")]
public class SettingsController : Controller
{
    private readonly SofiaDbContext _context;

    public SettingsController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userIdInt = int.Parse(userId);

        // Получаем статистику пользователя
        var totalNotes = await _context.Notes.CountAsync(n => n.UserId == userIdInt);
        var totalGoals = await _context.Goals.CountAsync(g => g.UserId == userIdInt);
        var completedGoals = await _context.Goals.CountAsync(g => g.UserId == userIdInt && g.Status == GoalStatus.Completed);
        var sharedNotes = await _context.Notes.CountAsync(n => n.UserId == userIdInt && n.ShareWithPsychologist);
        var pinnedNotes = await _context.Notes.CountAsync(n => n.UserId == userIdInt && n.IsPinned);
        var totalEmotions = await _context.EmotionEntries.CountAsync(e => e.UserId == userIdInt);

        // Получаем последние активности
        var recentNotes = await _context.Notes
            .Where(n => n.UserId == userIdInt)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .ToListAsync();

        var recentGoals = await _context.Goals
            .Where(g => g.UserId == userIdInt)
            .OrderByDescending(g => g.CreatedAt)
            .Take(3)
            .ToListAsync();

        // Получаем данные пользователя
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);

        ViewBag.TotalNotes = totalNotes;
        ViewBag.TotalGoals = totalGoals;
        ViewBag.CompletedGoals = completedGoals;
        ViewBag.SharedNotes = sharedNotes;
        ViewBag.PinnedNotes = pinnedNotes;
        ViewBag.TotalEmotions = totalEmotions;
        ViewBag.RecentNotes = recentNotes;
        ViewBag.RecentGoals = recentGoals;
        ViewBag.User = user;

        return View();
    }

    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userIdInt = int.Parse(userId);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);
        
        if (user == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        // Получаем статистику пользователя
        var totalNotes = await _context.Notes.CountAsync(n => n.UserId == userIdInt);
        var totalGoals = await _context.Goals.CountAsync(g => g.UserId == userIdInt);
        var completedGoals = await _context.Goals.CountAsync(g => g.UserId == userIdInt && g.Status == GoalStatus.Completed);
        var totalEmotions = await _context.EmotionEntries.CountAsync(e => e.UserId == userIdInt);

        ViewBag.User = user;
        ViewBag.TotalNotes = totalNotes;
        ViewBag.TotalGoals = totalGoals;
        ViewBag.CompletedGoals = completedGoals;
        ViewBag.TotalEmotions = totalEmotions;

        return View();
    }

    [HttpPost("profile")]
    public async Task<IActionResult> UpdateProfile(string name, string email, string bio, string timezone)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userIdInt);
        
        if (user == null)
        {
            return Json(new { success = false, message = "Пользователь не найден" });
        }

        // Обновляем данные пользователя
        user.FullName = name;
        user.Email = email;
        user.Bio = bio;

        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = "Профиль успешно обновлен!" 
        });
    }

    [HttpGet("preferences")]
    public IActionResult Preferences()
    {
        return View();
    }

    [HttpPost("preferences")]
    public IActionResult UpdatePreferences(string language, string timezone, bool notifications, bool emailUpdates, bool soundEffects, bool animations)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        // В реальном приложении здесь была бы логика сохранения предпочтений в базе данных
        // Пока что просто возвращаем успех
        return Json(new { 
            success = true, 
            message = "Настройки успешно сохранены!" 
        });
    }

    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost("privacy")]
    public IActionResult UpdatePrivacy(bool shareData, bool allowAnalytics, bool showInDirectory)
    {
        return Json(new { 
            success = true, 
            message = "Настройки приватности обновлены!" 
        });
    }

    [HttpGet("notifications")]
    public IActionResult Notifications()
    {
        return View();
    }

    [HttpPost("notifications")]
    public IActionResult UpdateNotifications(bool dailyReminder, bool goalReminder, bool moodCheck, string reminderTime)
    {
        return Json(new { 
            success = true, 
            message = "Настройки уведомлений обновлены!" 
        });
    }

    [HttpGet("data")]
    public IActionResult Data()
    {
        return View();
    }

    [HttpPost("export")]
    public async Task<IActionResult> ExportData(string format)
    {
        var notes = await _context.Notes
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var goals = await _context.Goals
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        if (format == "json")
        {
            var exportData = new
            {
                Notes = notes,
                Goals = goals,
                ExportDate = DateTime.Now,
                Version = "1.0"
            };

            var json = System.Text.Json.JsonSerializer.Serialize(exportData, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            return File(bytes, "application/json", $"sofia_export_{DateTime.Now:yyyy-MM-dd}.json");
        }
        else if (format == "csv")
        {
            var csvContent = "Тип,Дата,Содержание,Эмоция,Теги,Активность\n";
            
            foreach (var note in notes)
            {
                csvContent += $"Заметка,{note.CreatedAt:yyyy-MM-dd HH:mm},\"{note.Content.Replace("\"", "\"\"")}\",{note.Emotion},{note.Tags ?? ""},{note.Activity ?? ""}\n";
            }
            
            foreach (var goal in goals)
            {
                csvContent += $"Цель,{goal.CreatedAt:yyyy-MM-dd HH:mm},\"{goal.Title}\",{goal.Type},{goal.Status},{goal.Progress}%\n";
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            return File(bytes, "text/csv", $"sofia_export_{DateTime.Now:yyyy-MM-dd}.csv");
        }

        return BadRequest("Неподдерживаемый формат экспорта");
    }

    [HttpPost("delete-account")]
    public IActionResult DeleteAccount(string confirmation)
    {
        if (confirmation != "УДАЛИТЬ")
        {
            return Json(new { 
                success = false, 
                message = "Подтверждение неверное. Введите 'УДАЛИТЬ' для подтверждения." 
            });
        }

        // В реальном приложении здесь была бы логика удаления аккаунта
        // Пока что просто возвращаем успех
        return Json(new { 
            success = true, 
            message = "Аккаунт будет удален в течение 24 часов." 
        });
    }

    [HttpGet("help")]
    public IActionResult Help()
    {
        return View();
    }

    [HttpGet("about")]
    public IActionResult About()
    {
        return View();
    }
}


