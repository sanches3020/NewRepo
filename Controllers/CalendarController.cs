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

        var startDate = targetDate.AddDays(-(int)targetDate.DayOfWeek);
        var endDate = startDate.AddDays(41); // 6 weeks

        var notes = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date >= startDate && n.Date < endDate)
            .ToListAsync();

        var emotions = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date >= startDate && e.Date < endDate)
            .ToListAsync();

        var calendarData = new Dictionary<DateTime, List<Note>>();
        var emotionData = new Dictionary<DateTime, List<EmotionEntry>>();
        
        for (var date = startDate; date < endDate; date = date.AddDays(1))
        {
            calendarData[date] = notes.Where(n => n.Date.Date == date.Date).ToList();
            emotionData[date] = emotions.Where(e => e.Date.Date == date.Date).ToList();
        }

        ViewBag.CurrentMonth = targetDate;
        ViewBag.PreviousMonth = targetDate.AddMonths(-1);
        ViewBag.NextMonth = targetDate.AddMonths(1);
        ViewBag.CalendarData = calendarData;
        ViewBag.EmotionData = emotionData;

        return View();
    }

    [HttpPost("save-emotion")]
    public async Task<IActionResult> SaveEmotion([FromBody] SaveEmotionRequest request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var date = DateTime.Parse(request.Date);

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
            Emotion = request.Emotion,
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
        var targetDate = DateTime.Parse(date);

        var notes = await _context.Notes
            .Where(n => n.UserId == userIdInt && n.Date.Date == targetDate.Date)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var emotions = await _context.EmotionEntries
            .Where(e => e.UserId == userIdInt && e.Date.Date == targetDate.Date)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();

        var goals = await _context.Goals
            .Where(g => g.UserId == userIdInt && g.Date.Date == targetDate.Date)
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

public class SaveEmotionRequest
{
    public string Date { get; set; } = string.Empty;
    public EmotionType Emotion { get; set; }
    public string? Note { get; set; }
}