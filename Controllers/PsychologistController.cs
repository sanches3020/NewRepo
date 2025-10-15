using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("psychologist")]
public class PsychologistController : Controller
{
    private readonly SofiaDbContext _context;

    public PsychologistController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        // Если это психолог, показываем его записи
        if (userRole == "psychologist")
        {
            var psychologist = await _context.Psychologists
                .FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));
            
            if (psychologist != null)
            {
                var appointments = await _context.PsychologistAppointments
                    .Where(a => a.PsychologistId == psychologist.Id)
                    .Include(a => a.User)
                    .OrderByDescending(a => a.AppointmentDate)
                    .ToListAsync();

                ViewBag.Psychologist = psychologist;
                ViewBag.Appointments = appointments;
                return View("PsychologistDashboard");
            }
        }

        // Для обычных пользователей показываем список психологов
        var psychologists = await _context.Psychologists
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();

        // Получаем последние заметки пользователя для анализа
        var recentNotes = await _context.Notes
            .Where(n => n.UserId == int.Parse(userId) && n.ShareWithPsychologist)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .ToListAsync();

        ViewBag.Psychologists = psychologists;
        ViewBag.RecentNotes = recentNotes;

        return View();
    }

    [HttpGet("profile/{id}")]
    public async Task<IActionResult> Profile(int id)
    {
        var psychologist = await _context.Psychologists
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (psychologist == null)
        {
            return NotFound();
        }

        // Получаем отзывы о психологе
        var reviews = await _context.PsychologistReviews
            .Where(r => r.PsychologistId == id)
            .OrderByDescending(r => r.CreatedAt)
            .Take(10)
            .ToListAsync();

        // Получаем доступные слоты для записи
        var availableSlots = GetAvailableSlots(psychologist.Id);

        ViewBag.Psychologist = psychologist;
        ViewBag.Reviews = reviews;
        ViewBag.AvailableSlots = availableSlots;

        return View();
    }

    [HttpPost("book")]
    public async Task<IActionResult> BookAppointment(int psychologistId, DateTime appointmentDate, string notes)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Необходимо войти в систему" });
        }

        // Создаем запись на консультацию
        var appointment = new PsychologistAppointment
        {
            PsychologistId = psychologistId,
            UserId = int.Parse(userId),
            AppointmentDate = appointmentDate,
            Notes = notes,
            Status = AppointmentStatus.Scheduled,
            CreatedAt = DateTime.Now
        };

        _context.PsychologistAppointments.Add(appointment);
        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = "Запись на консультацию успешно создана!",
            appointmentId = appointment.Id
        });
    }

    [HttpGet("appointments")]
    public async Task<IActionResult> Appointments()
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return RedirectToAction("Login", "Auth");
        }

        var appointments = await _context.PsychologistAppointments
            .Where(a => a.UserId == int.Parse(userId))
            .Include(a => a.Psychologist)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        ViewBag.Appointments = appointments;
        return View();
    }

    [HttpPost("review")]
    public async Task<IActionResult> AddReview(int psychologistId, int rating, string comment)
    {
        var review = new PsychologistReview
        {
            PsychologistId = psychologistId,
            Rating = rating,
            Comment = comment,
            CreatedAt = DateTime.Now
        };

        _context.PsychologistReviews.Add(review);
        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = "Отзыв успешно добавлен!"
        });
    }

    [HttpPost("update-status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequest request)
    {
        try
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");
            
            if (string.IsNullOrEmpty(userId) || userRole != "psychologist")
            {
                return Json(new { success = false, message = "Доступ запрещен" });
            }

            var appointment = await _context.PsychologistAppointments
                .Include(a => a.Psychologist)
                .FirstOrDefaultAsync(a => a.Id == request.AppointmentId && a.Psychologist.UserId == int.Parse(userId));

            if (appointment == null)
            {
                return Json(new { success = false, message = "Запись не найдена" });
            }

            if (Enum.TryParse<AppointmentStatus>(request.Status, out var newStatus))
            {
                appointment.Status = newStatus;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Неверный статус" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    private List<DateTime> GetAvailableSlots(int psychologistId)
    {
        // Генерируем доступные слоты на следующие 2 недели
        var slots = new List<DateTime>();
        var startDate = DateTime.Now.Date.AddDays(1);
        
        for (int i = 0; i < 14; i++)
        {
            var date = startDate.AddDays(i);
            
            // Добавляем слоты с 9:00 до 18:00 с интервалом в 1 час
            for (int hour = 9; hour < 18; hour++)
            {
                var slot = date.AddHours(hour);
                if (slot > DateTime.Now) // Только будущие слоты
                {
                    slots.Add(slot);
                }
            }
        }

        return slots;
    }
}

public class UpdateStatusRequest
{
    public int AppointmentId { get; set; }
    public string Status { get; set; } = string.Empty;
}


