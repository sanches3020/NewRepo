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
        // Получаем психологов из базы данных
        var psychologists = await _context.Psychologists
            .Where(p => p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync();

        // Получаем последние заметки пользователя для анализа
        var recentNotes = await _context.Notes
            .Where(n => n.ShareWithPsychologist)
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
        // Создаем запись на консультацию
        var appointment = new PsychologistAppointment
        {
            PsychologistId = psychologistId,
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
        var appointments = await _context.PsychologistAppointments
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


