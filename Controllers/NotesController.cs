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
    public async Task<IActionResult> Create([FromBody] CreateNoteRequest? request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        if (request == null || string.IsNullOrEmpty(request.Content))
        {
            return Json(new { success = false, message = "Содержание заметки обязательно" });
        }

        if (!Enum.TryParse<EmotionType>(request.Emotion, true, out var emotionType))
        {
            return Json(new { success = false, message = "Неверный тип эмоции" });
        }

        var userIdInt = int.Parse(userId);
        var targetDate = !string.IsNullOrEmpty(request.Date) && DateTime.TryParse(request.Date, out var parsedDate) 
            ? parsedDate 
            : DateTime.Today;

        var note = new Note
        {
            UserId = userIdInt,
            Content = request.Content,
            Tags = request.Tags,
            Emotion = emotionType,
            Activity = request.Activity,
            Date = targetDate,
            IsPinned = request.IsPinned,
            ShareWithPsychologist = request.ShareWithPsychologist,
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
    public async Task<IActionResult> Edit(int id, [FromBody] CreateNoteRequest? request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Пользователь не авторизован" });
        }

        if (request == null || string.IsNullOrEmpty(request.Content))
        {
            return Json(new { success = false, message = "Содержание заметки обязательно" });
        }

        if (!Enum.TryParse<EmotionType>(request.Emotion, true, out var emotionType))
        {
            return Json(new { success = false, message = "Неверный тип эмоции" });
        }

        var userIdInt = int.Parse(userId);
        var note = await _context.Notes
            .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userIdInt);

        if (note == null)
        {
            return Json(new { success = false, message = "Заметка не найдена" });
        }

        note.Content = request.Content;
        note.Tags = request.Tags;
        note.Emotion = emotionType;
        note.Activity = request.Activity;
        if (!string.IsNullOrEmpty(request.Date) && DateTime.TryParse(request.Date, out var parsedDate))
        {
            note.Date = parsedDate;
        }
        note.IsPinned = request.IsPinned;
        note.ShareWithPsychologist = request.ShareWithPsychologist;
        note.UpdatedAt = DateTime.Now;

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
