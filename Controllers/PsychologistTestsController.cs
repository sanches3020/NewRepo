using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("psychologist/tests")]
public class PsychologistTestsController : Controller
{
    private readonly SofiaDbContext _context;

    public PsychologistTestsController(SofiaDbContext context)
    {
        _context = context;
    }

    private async Task<int?> GetPsychologistIdAsync()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr)) return null;
        var userId = int.Parse(userIdStr);
        var psychologist = await _context.Psychologists.FirstOrDefaultAsync(p => p.UserId == userId);
        return psychologist?.Id;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "psychologist") return RedirectToAction("Login", "Auth");

        var psyId = await GetPsychologistIdAsync();
        if (!psyId.HasValue) return Forbid();

        var tests = await _context.Tests
            .Where(t => t.CreatedByPsychologistId == psyId.Value)
            .OrderByDescending(t => t.Id)
            .ToListAsync();

        return View(tests);
    }

    [HttpGet("create")]
    public IActionResult Create()
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "psychologist") return RedirectToAction("Login", "Auth");
        return View();
    }

    public class CreateTestRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();

        public class QuestionDto
        {
            public string Text { get; set; } = string.Empty;
            public AnswerDto[] Answers { get; set; } = Array.Empty<AnswerDto>();

            public class AnswerDto
            {
                public string Text { get; set; } = string.Empty;
                public int Value { get; set; }
            }
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateTestRequest request)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "psychologist") return Json(new { success = false, message = "Доступ запрещен" });

        var psyId = await GetPsychologistIdAsync();
        if (!psyId.HasValue) return Json(new { success = false, message = "Психолог не найден" });

        if (string.IsNullOrWhiteSpace(request.Name)) return Json(new { success = false, message = "Название обязательно" });

        var test = new Test
        {
            Name = request.Name,
            Description = request.Description,
            Type = TestType.Custom,
            CreatedByPsychologistId = psyId.Value
        };

        _context.Tests.Add(test);
        await _context.SaveChangesAsync(); // To get test.Id

        var questions = new List<Question>();
        foreach (var q in request.Questions)
        {
            var question = new Question { TestId = test.Id, Text = q.Text, Type = AnswerType.SingleChoice };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            var answers = q.Answers.Select((a, idx) => new Answer { QuestionId = question.Id, Text = a.Text, Value = a.Value, Order = idx }).ToList();
            _context.Answers.AddRange(answers);
            await _context.SaveChangesAsync();
        }

        return Json(new { success = true, redirect = Url.Action("Index") });
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        if (userRole != "psychologist") return Json(new { success = false, message = "Доступ запрещен" });

        var psyId = await GetPsychologistIdAsync();
        if (!psyId.HasValue) return Json(new { success = false, message = "Психолог не найден" });

        var test = await _context.Tests.FirstOrDefaultAsync(t => t.Id == id && t.CreatedByPsychologistId == psyId.Value);
        if (test == null) return Json(new { success = false, message = "Тест не найден" });

        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }
}
