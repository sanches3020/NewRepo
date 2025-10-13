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
        // Получаем статистику пользователя
        var totalNotes = await _context.Notes.CountAsync();
        var totalGoals = await _context.Goals.CountAsync();
        var completedGoals = await _context.Goals.CountAsync(g => g.Status == GoalStatus.Completed);
        var sharedNotes = await _context.Notes.CountAsync(n => n.ShareWithPsychologist);
        var pinnedNotes = await _context.Notes.CountAsync(n => n.IsPinned);

        // Получаем последние активности
        var recentNotes = await _context.Notes
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .ToListAsync();

        var recentGoals = await _context.Goals
            .OrderByDescending(g => g.CreatedAt)
            .Take(3)
            .ToListAsync();

        ViewBag.TotalNotes = totalNotes;
        ViewBag.TotalGoals = totalGoals;
        ViewBag.CompletedGoals = completedGoals;
        ViewBag.SharedNotes = sharedNotes;
        ViewBag.PinnedNotes = pinnedNotes;
        ViewBag.RecentNotes = recentNotes;
        ViewBag.RecentGoals = recentGoals;

        return View();
    }

    [HttpGet("profile")]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpPost("profile")]
    public IActionResult UpdateProfile(string name, string email, string bio, string timezone)
    {
        // В реальном приложении здесь была бы логика обновления профиля
        // Пока что просто возвращаем успех
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
    public IActionResult UpdatePreferences(string theme, bool notifications, bool emailUpdates, string language)
    {
        // В реальном приложении здесь была бы логика сохранения предпочтений
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


