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
        var targetDate = DateTime.Now;
        if (year.HasValue && month.HasValue)
        {
            targetDate = new DateTime(year.Value, month.Value, 1);
        }

        var startDate = targetDate.AddDays(-(int)targetDate.DayOfWeek);
        var endDate = startDate.AddDays(41); // 6 weeks

        var notes = await _context.Notes
            .Where(n => n.CreatedAt >= startDate && n.CreatedAt < endDate)
            .ToListAsync();

        var calendarData = new Dictionary<DateTime, List<Note>>();
        for (var date = startDate; date < endDate; date = date.AddDays(1))
        {
            calendarData[date] = notes.Where(n => n.CreatedAt.Date == date.Date).ToList();
        }

        ViewBag.CurrentMonth = targetDate;
        ViewBag.PreviousMonth = targetDate.AddMonths(-1);
        ViewBag.NextMonth = targetDate.AddMonths(1);
        ViewBag.CalendarData = calendarData;

        return View();
    }

    [HttpGet("day/{year}/{month}/{day}")]
    public async Task<IActionResult> DayDetails(int year, int month, int day)
    {
        var date = new DateTime(year, month, day);
        var notes = await _context.Notes
            .Where(n => n.CreatedAt.Date == date.Date)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        ViewBag.Date = date;
        ViewBag.Notes = notes;

        return View();
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