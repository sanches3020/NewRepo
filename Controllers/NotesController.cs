using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("notes")]
public class NotesController : Controller
{
    private readonly SofiaDbContext _context;

    public NotesController(SofiaDbContext context)
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

        var notes = await _context.Notes
            .Where(n => n.UserId == userIdInt)
            .OrderByDescending(n => n.Date)
            .ThenByDescending(n => n.CreatedAt)
            .ToListAsync();

        ViewBag.Notes = notes;

        return View();
    }

    [HttpGet("create")]
    public IActionResult Create(string? date)
    {
        var targetDate = !string.IsNullOrEmpty(date) ? DateTime.Parse(date) : DateTime.Today;
        ViewBag.TargetDate = targetDate;
        return View();
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(string content, string? tags, EmotionType emotion, string? activity, DateTime? date, bool isPinned = false, bool shareWithPsychologist = false)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var targetDate = date ?? DateTime.Today;

        var note = new Note
        {
            UserId = userIdInt,
            Content = content,
            Tags = tags,
            Emotion = emotion,
            Activity = activity,
            Date = targetDate,
            IsPinned = isPinned,
            ShareWithPsychologist = shareWithPsychologist,
            CreatedAt = DateTime.Now
        };

        _context.Notes.Add(note);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Заметка создана!" });
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var userIdInt = int.Parse(userId);
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userIdInt);

        if (note == null)
        {
            return NotFound();
        }

        return View(note);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(int id, string content, string? tags, EmotionType emotion, string? activity, DateTime? date, bool isPinned = false, bool shareWithPsychologist = false)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userIdInt);

        if (note == null)
        {
            return Json(new { success = false, message = "Заметка не найдена" });
        }

        note.Content = content;
        note.Tags = tags;
        note.Emotion = emotion;
        note.Activity = activity;
        note.Date = date ?? note.Date;
        note.IsPinned = isPinned;
        note.ShareWithPsychologist = shareWithPsychologist;

        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Заметка обновлена!" });
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userIdInt);

        if (note == null)
        {
            return Json(new { success = false, message = "Заметка не найдена" });
        }

        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Заметка удалена!" });
    }

    [HttpPost("toggle-pin/{id}")]
    public async Task<IActionResult> TogglePin(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userIdInt);

        if (note == null)
        {
            return Json(new { success = false, message = "Заметка не найдена" });
        }

        note.IsPinned = !note.IsPinned;
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = note.IsPinned ? "Заметка закреплена!" : "Заметка откреплена!" });
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        var userIdInt = int.Parse(userId);
        var today = DateTime.Today;

        var todayNotes = await _context.Notes
            .CountAsync(n => n.UserId == userIdInt && n.Date.Date == today);

        var pinnedNotes = await _context.Notes
            .CountAsync(n => n.UserId == userIdInt && n.IsPinned);

        var sharedNotes = await _context.Notes
            .CountAsync(n => n.UserId == userIdInt && n.ShareWithPsychologist);

        return Json(new
        {
            success = true,
            todayNotes = todayNotes,
            pinnedNotes = pinnedNotes,
            sharedNotes = sharedNotes
        });
    }
}
