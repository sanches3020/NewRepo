using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("practices")]
public class PracticesController : Controller
{
    private readonly SofiaDbContext _context;

    public PracticesController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string? category, int? duration)
    {
        var query = _context.Practices.Where(p => p.IsActive);

        if (!string.IsNullOrEmpty(category) && Enum.TryParse<PracticeCategory>(category, out var cat))
        {
            query = query.Where(p => p.Category == cat);
        }

        if (duration.HasValue)
        {
            query = query.Where(p => p.DurationMinutes <= duration.Value);
        }

        var practices = await query.OrderBy(p => p.Category).ThenBy(p => p.Name).ToListAsync();
        
        ViewBag.Categories = Enum.GetValues<PracticeCategory>();
        ViewBag.SelectedCategory = category;
        ViewBag.SelectedDuration = duration;
        
        return View(practices);
    }

    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var practice = await _context.Practices.FindAsync(id);
        if (practice == null) return NotFound();
        
        return View(practice);
    }

    [HttpPost("start/{id}")]
    public async Task<IActionResult> Start(int id)
    {
        var practice = await _context.Practices.FindAsync(id);
        if (practice == null) return NotFound();
        
        // Here you could log the practice session
        TempData["SuccessMessage"] = $"Практика '{practice.Name}' начата!";
        
        return RedirectToAction(nameof(Details), new { id });
    }
}


