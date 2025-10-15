using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("goals")]
public class GoalsController : Controller
{
    private readonly SofiaDbContext _context;

    public GoalsController(SofiaDbContext context)
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

        var goals = await _context.Goals
            .Where(g => g.UserId == int.Parse(userId))
            .OrderByDescending(g => g.Status == GoalStatus.Active)
            .ThenByDescending(g => g.CreatedAt)
            .ToListAsync();
        
        return View(goals);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        return View(new Goal());
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(Goal goal)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (ModelState.IsValid)
        {
            goal.CreatedAt = DateTime.Now;
            goal.UserId = int.Parse(userId);
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(goal);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var goal = await _context.Goals
            .Where(g => g.Id == id && g.UserId == int.Parse(userId))
            .FirstOrDefaultAsync();
        
        if (goal == null) return NotFound();
        return View(goal);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(int id, Goal goal)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        if (id != goal.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                goal.UserId = int.Parse(userId);
                _context.Update(goal);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GoalExists(goal.Id)) return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(goal);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var goal = await _context.Goals
            .Where(g => g.Id == id && g.UserId == int.Parse(userId))
            .FirstOrDefaultAsync();
        
        if (goal != null)
        {
            _context.Goals.Remove(goal);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("update-progress/{id}")]
    public async Task<IActionResult> UpdateProgress(int id, int progress)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var goal = await _context.Goals
            .Where(g => g.Id == id && g.UserId == int.Parse(userId))
            .FirstOrDefaultAsync();
        
        if (goal != null)
        {
            goal.Progress = Math.Max(0, Math.Min(100, progress));
            if (goal.Progress == 100)
            {
                goal.Status = GoalStatus.Completed;
            }
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool GoalExists(int id)
    {
        return _context.Goals.Any(e => e.Id == id);
    }
}


