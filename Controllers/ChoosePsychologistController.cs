using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;

namespace Sofia.Web.Controllers;

[Route("choose-psychologist")]
public class ChoosePsychologistController : Controller
{
    private readonly SofiaDbContext _context;

    public ChoosePsychologistController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var psychologists = await _context.Psychologists
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();

        ViewBag.Psychologists = psychologists;
        return View();
    }
}


