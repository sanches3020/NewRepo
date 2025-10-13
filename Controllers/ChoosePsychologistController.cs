using Microsoft.AspNetCore.Mvc;

namespace Sofia.Web.Controllers;

[Route("choose-psychologist")]
public class ChoosePsychologistController : Controller
{
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }
}


