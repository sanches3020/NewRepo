using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("tests")]
public class TestsController : Controller
{
    private readonly SofiaDbContext _context;

    public TestsController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Auth");

        var tests = await _context.Tests
            .OrderBy(t => t.Name)
            .ToListAsync();

        return View(tests);
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> Analytics()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Auth");

        var userIdInt = int.Parse(userId);
        var tests = await _context.Tests
            .OrderBy(t => t.Name)
            .ToListAsync();

        ViewBag.Tests = tests;
        return View();
    }

    [HttpGet("analytics/data")]
    public async Task<IActionResult> AnalyticsData(int testId, DateTime from, DateTime to)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId)) return Json(new { success = false, message = "Неавторизован" });

        var userIdInt = int.Parse(userId);

        var results = await _context.TestResults
            .Where(r => r.UserId == userIdInt && r.TestId == testId && r.TakenAt >= from && r.TakenAt <= to)
            .OrderBy(r => r.TakenAt)
            .Select(r => new { date = r.TakenAt, score = r.Score, level = r.Level, interpretation = r.Interpretation })
            .ToListAsync();

        if (!results.Any()) return Json(new { success = true, data = Array.Empty<object>() });

        // Basic interpretation: average, trend
        var avg = results.Average(r => r.score);
        var firstHalfAvg = results.Take(results.Count / 2).Any() ? results.Take(results.Count / 2).Average(r => r.score) : results.First().score;
        var secondHalfAvg = results.Skip(results.Count / 2).Any() ? results.Skip(results.Count / 2).Average(r => r.score) : results.Last().score;
        var trend = secondHalfAvg > firstHalfAvg ? "Рост" : (secondHalfAvg < firstHalfAvg ? "Падение" : "Стабильно");

        return Json(new { success = true, data = results, avg = avg, trend = trend });
    }

    [HttpGet("take/{id}")]
    public async Task<IActionResult> Take(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Auth");

        var test = await _context.Tests
            .Include(t => t.Questions.OrderBy(q => q.Id))
                .ThenInclude(q => q.Answers.OrderBy(a => a.Order))
            .FirstOrDefaultAsync(t => t.Id == id);

        if (test == null) return NotFound();

        return View(test);
    }

    [HttpGet("history/{testId}")]
    public async Task<IActionResult> History(int testId)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Auth");

        var userIdInt = int.Parse(userId);

        var test = await _context.Tests
            .FirstOrDefaultAsync(t => t.Id == testId);
        if (test == null) return NotFound();

        var results = await _context.TestResults
            .Where(r => r.UserId == userIdInt && r.TestId == testId)
            .OrderByDescending(r => r.TakenAt)
            .ToListAsync();

        ViewBag.Test = test;
        return View(results);
    }

    [HttpPost("submit/{id}")]
    public async Task<IActionResult> Submit(int id)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr)) return Json(new { success = false, message = "Пользователь не авторизован" });

        var userId = int.Parse(userIdStr);
        var test = await _context.Tests
            .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (test == null) return Json(new { success = false, message = "Тест не найден" });

        // parse answers from form: keys like q_{questionId}
        var form = Request.Form;
        var userAnswers = new List<UserAnswer>();

        foreach (var question in test.Questions)
        {
            var key = $"q_{question.Id}";
            if (!form.ContainsKey(key)) continue;

            var value = form[key].ToString();
            if (string.IsNullOrEmpty(value)) continue;

            if (question.Type == AnswerType.Text)
            {
                userAnswers.Add(new UserAnswer { UserId = userId, QuestionId = question.Id, TextAnswer = value, CreatedAt = DateTime.Now });
                continue;
            }

            // multiple-choice may provide comma-separated ids
            var parts = value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var part in parts)
            {
                if (int.TryParse(part, out var ansId))
                {
                    userAnswers.Add(new UserAnswer { UserId = userId, QuestionId = question.Id, AnswerId = ansId, CreatedAt = DateTime.Now });
                }
            }
        }

        if (userAnswers.Count > 0)
        {
            _context.UserAnswers.AddRange(userAnswers);
            await _context.SaveChangesAsync();
        }

        // Calculate score
        var selectedAnswerIds = userAnswers.Where(ua => ua.AnswerId.HasValue).Select(ua => ua.AnswerId!.Value).ToList();
        var answers = await _context.Answers.Where(a => selectedAnswerIds.Contains(a.Id)).ToListAsync();
        var score = answers.Sum(a => a.Value);

        // Compute max possible score for this test (sum of max answer value per question)
        var maxScore = 0;
        foreach (var q in test.Questions)
        {
            var max = q.Answers.Any() ? q.Answers.Max(a => a.Value) : 0;
            maxScore += max;
        }

        var percent = (maxScore > 0) ? (double)score / maxScore * 100.0 : 0.0;
        string level;
        string interpretation;

        // Try to find a test-specific interpretation by percent thresholds
        var rule = await _context.TestInterpretations
            .Where(ti => ti.TestId == test.Id && percent >= ti.MinPercent && percent <= ti.MaxPercent)
            .FirstOrDefaultAsync();

        if (rule != null)
        {
            level = rule.Level;
            interpretation = rule.InterpretationText ?? string.Empty;
        }
        else
        {
            // Fallback to generic percent buckets
            if (percent < 33) { level = "Низкий"; interpretation = "Низкий уровень"; }
            else if (percent < 66) { level = "Средний"; interpretation = "Средний уровень"; }
            else { level = "Высокий"; interpretation = "Высокий уровень"; }
        }

        var result = new TestResult
        {
            UserId = userId,
            TestId = test.Id,
            TakenAt = DateTime.Now,
            Score = score,
            Level = level,
            Interpretation = interpretation
        };

        _context.TestResults.Add(result);
        await _context.SaveChangesAsync();

        return Json(new { success = true, redirect = Url.Action("Result", new { id = result.Id }) });
    }

    [HttpGet("result/{id}")]
    public async Task<IActionResult> Result(int id)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr)) return RedirectToAction("Login", "Auth");

        var userId = int.Parse(userIdStr);

        var result = await _context.TestResults
            .Include(r => r.Test)
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (result == null) return NotFound();

        // history for chart
        var history = await _context.TestResults
            .Where(r => r.UserId == userId && r.TestId == result.TestId)
            .OrderBy(r => r.TakenAt)
            .Select(r => new { r.TakenAt, r.Score })
            .ToListAsync();

        ViewBag.History = history;

        return View(result);
    }
}
