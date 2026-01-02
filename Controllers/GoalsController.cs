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
    public async Task<IActionResult> Index(string sort = null)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var query = _context.Goals.Where(g => g.UserId == int.Parse(userId));

        // If user has no goals yet, seed three sample goals so they are actionable
        if (!await query.AnyAsync())
        {
            var uid = int.Parse(userId);
            var samples = new List<Goal>
            {
                new Goal { Title = "Улучшить сон", Description = "Режим сна, ритуалы и трекер для регулярности.", Type = GoalType.Wellness, Status = GoalStatus.Active, Progress = 20, CreatedAt = DateTime.Now, Date = DateTime.Today, TargetDate = DateTime.Today.AddMonths(1), UserId = uid },
                new Goal { Title = "Сделать презентацию", Description = "Подготовьте слайды, репетиции и дедлайны.", Type = GoalType.Professional, Status = GoalStatus.Active, Progress = 45, CreatedAt = DateTime.Now.AddDays(-7), Date = DateTime.Today.AddDays(-7), TargetDate = DateTime.Today.AddDays(14), UserId = uid },
                new Goal { Title = "Пройти курс", Description = "Разделите обучение на шаги и отслеживайте прогресс.", Type = GoalType.Professional, Status = GoalStatus.Active, Progress = 10, CreatedAt = DateTime.Now, Date = DateTime.Today, TargetDate = DateTime.Today.AddMonths(2), UserId = uid }
            };

            _context.Goals.AddRange(samples);
            await _context.SaveChangesAsync();

            // refresh query to include newly added goals
            query = _context.Goals.Where(g => g.UserId == uid);
        }

        // Sorting
        if (string.IsNullOrEmpty(sort))
        {
            // default: active first, then newest
            query = query.OrderByDescending(g => g.Status == GoalStatus.Active)
                         .ThenByDescending(g => g.CreatedAt);
        }
        else
        {
            ViewBag.CurrentSort = sort;
            switch (sort)
            {
                case "created_asc":
                    query = query.OrderBy(g => g.CreatedAt);
                    break;
                case "created_desc":
                    query = query.OrderByDescending(g => g.CreatedAt);
                    break;
                case "deadline_asc":
                    // put goals without deadline last
                    query = query.OrderBy(g => g.TargetDate ?? DateTime.MaxValue);
                    break;
                case "deadline_desc":
                    // put goals without deadline last (by using MinValue fallback then reverse)
                    query = query.OrderByDescending(g => g.TargetDate ?? DateTime.MinValue);
                    break;
                case "progress_asc":
                    query = query.OrderBy(g => g.Progress);
                    break;
                case "progress_desc":
                    query = query.OrderByDescending(g => g.Progress);
                    break;
                default:
                    query = query.OrderByDescending(g => g.Status == GoalStatus.Active)
                                 .ThenByDescending(g => g.CreatedAt);
                    break;
            }
        }

        var goals = await query.ToListAsync();
        return View(goals);
    }

    [HttpGet("create")]
    public IActionResult Create(string? template)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var model = new Goal { Status = GoalStatus.Active, Progress = 0, Type = GoalType.Personal, CreatedAt = DateTime.Now, Date = DateTime.Today };
        if (!string.IsNullOrEmpty(template))
        {
            switch (template.ToLower())
            {
                case "course":
                    model.Title = "Пройти курс";
                    model.Description = "Завершить онлайн-курс и выполнить все задания.";
                    model.Type = GoalType.Professional;
                    model.TargetDate = DateTime.Today.AddDays(30);
                    break;
                case "sleep":
                    model.Title = "Улучшить сон";
                    model.Description = "Нормализовать режим сна: ложиться и вставать в одно и то же время.";
                    model.Type = GoalType.Wellness;
                    model.TargetDate = DateTime.Today.AddDays(14);
                    break;
                case "presentation":
                    model.Title = "Сделать презентацию";
                    model.Description = "Подготовить и провести презентацию проекта перед командой.";
                    model.Type = GoalType.Professional;
                    model.TargetDate = DateTime.Today.AddDays(7);
                    break;
                default:
                    break;
            }
        }

        return View(model);
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


