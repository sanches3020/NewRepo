using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("calendar")]
public class CalendarController : Controller
{
    private readonly SofiaDbContext _context;

    public CalendarController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(int? year, int? month)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userIdInt = int.Parse(userId);
        var targetDate = DateTime.Now;
        if (year.HasValue && month.HasValue)
        {
            targetDate = new DateTime(year.Value, month.Value, 1);
        }

        // Исправляем расчет начальной даты для календаря
        var firstDayOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
        var startDate = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
        if (firstDayOfMonth.DayOfWeek == DayOfWeek.Sunday)
        {
            startDate = startDate.AddDays(-6);
        }
        else
        {
            startDate = startDate.AddDays(1);
        }
        var endDate = startDate.AddDays(41); // 6 weeks

        var notes = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date.Date >= startDate.Date && n.Date.Date < endDate.Date)
            .ToListAsync();

        var emotions = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date.Date >= startDate.Date && e.Date.Date < endDate.Date)
            .ToListAsync();

        var calendarData = new Dictionary<DateTime, List<Note>>();
        var emotionData = new Dictionary<DateTime, List<EmotionEntry>>();
        
        for (var date = startDate.Date; date < endDate.Date; date = date.AddDays(1))
        {
            var dateKey = date.Date;
            calendarData[dateKey] = notes.Where(n => n.Date.Date == dateKey).ToList();
            emotionData[dateKey] = emotions.Where(e => e.Date.Date == dateKey).ToList();
        }

        ViewBag.CurrentMonth = targetDate;
        ViewBag.PreviousMonth = targetDate.AddMonths(-1);
        ViewBag.NextMonth = targetDate.AddMonths(1);
        ViewBag.CalendarData = calendarData;
        ViewBag.EmotionData = emotionData;

        return View();
    }

    [HttpPost("save-emotion")]
    public async Task<IActionResult> SaveEmotion([FromBody] SaveEmotionRequest? request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        if (request == null || string.IsNullOrEmpty(request.Date))
        {
            return Json(new { success = false, message = "Дата не указана" });
        }

        if (!DateTime.TryParse(request.Date, out var date))
        {
            return Json(new { success = false, message = "Неверный формат даты" });
        }
        date = date.Date; // Нормализуем дату, убирая время

        // Парсим EmotionType из строки
        if (!Enum.TryParse<EmotionType>(request.Emotion, true, out var emotionType))
        {
            return Json(new { success = false, message = "Неверный тип эмоции: " + request.Emotion });
        }

        var userIdInt = int.Parse(userId);

        // Проверяем, сколько эмоций уже записано на этот день
        var existingEmotions = await _context.EmotionEntries
            .CountAsync(e => e.UserId == userIdInt && e.Date.Date == date.Date);

        if (existingEmotions >= 5)
        {
            return Json(new { success = false, message = "Максимум 5 эмоций в день" });
        }

        var emotionEntry = new EmotionEntry
        {
            UserId = userIdInt,
            Date = date,
            Emotion = emotionType,
            Note = request.Note,
            CreatedAt = DateTime.Now
        };

        _context.EmotionEntries.Add(emotionEntry);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Эмоция сохранена!" });
    }


    [HttpGet("day-details")]
    public async Task<IActionResult> DayDetails(string date)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        if (!DateTime.TryParse(date, out var targetDate))
        {
            return Json(new { success = false, message = "Неверный формат даты" });
        }
        targetDate = targetDate.Date;

        var notes = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date.Date == targetDate)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var emotions = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date.Date == targetDate)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

        var goals = await _context.Goals
            .Where(g => g.UserId == userIdInt && g.Date.Date == targetDate)
            .ToListAsync();

        ViewBag.Date = targetDate;
        ViewBag.Notes = notes;
        ViewBag.Emotions = emotions;
        ViewBag.Goals = goals;

        return PartialView("_DayDetails");
    }

    [HttpGet("emotion-stats")]
    public async Task<IActionResult> EmotionStats(int? days)
    {
        var daysBack = days ?? 30;
        var startDate = DateTime.Now.AddDays(-daysBack);

        var emotionStats = await _context.Notes
            .Where(n => n.CreatedAt >= startDate)
            .GroupBy(n => n.Emotion)
            .Select(g => new { Emotion = g.Key, Count = g.Count() })
            .ToListAsync();

        ViewBag.EmotionStats = emotionStats;
        ViewBag.DaysBack = daysBack;

        return View();
    }
}

