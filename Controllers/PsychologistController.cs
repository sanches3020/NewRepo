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

        // Если это психолог, перенаправляем на дашборд
        if (userRole == "psychologist")
        {
            var psychologist = await _context.Psychologists
                .FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));
            
            if (psychologist != null)
            {
                return RedirectToAction("Dashboard", new { id = psychologist.Id });
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

    [HttpGet("dashboard/{id}")]
    public async Task<IActionResult> Dashboard(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(userId) || userRole != "psychologist")
        {
            return RedirectToAction("Login", "Auth");
        }

        var psychologist = await _context.Psychologists
            .FirstOrDefaultAsync(p => p.Id == id && p.UserId == int.Parse(userId));

        if (psychologist == null)
        {
            return NotFound();
        }

        // Получаем записи на консультации
        var appointments = await _context.PsychologistAppointments
            .Where(a => a.PsychologistId == psychologist.Id)
            .Include(a => a.User)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        // Получаем клиентов психолога
        var clients = await _context.PsychologistAppointments
            .Where(a => a.PsychologistId == psychologist.Id)
            .Include(a => a.User)
            .Select(a => a.User)
            .Distinct()
            .ToListAsync();

        // Получаем данные клиентов для анализа
        var clientData = new List<ClientDataViewModel>();
        foreach (var client in clients)
        {
            var clientNotes = await _context.Notes
                .Where(n => n.UserId == client.Id && n.ShareWithPsychologist)
                .OrderByDescending(n => n.CreatedAt)
                .Take(5)
                .ToListAsync();

            var clientGoals = await _context.Goals
                .Where(g => g.UserId == client.Id)
                .OrderByDescending(g => g.CreatedAt)
                .Take(5)
                .ToListAsync();

            var clientEmotions = await _context.EmotionEntries
                .Where(e => e.UserId == client.Id)
                .OrderByDescending(e => e.Date)
                .Take(10)
                .ToListAsync();

            var recentAppointments = await _context.PsychologistAppointments
                .Where(a => a.PsychologistId == psychologist.Id && a.UserId == client.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .Take(3)
                .ToListAsync();

            clientData.Add(new ClientDataViewModel
            {
                User = client,
                Notes = clientNotes,
                Goals = clientGoals,
                Emotions = clientEmotions,
                RecentAppointments = recentAppointments
            });
        }

        ViewBag.Psychologist = psychologist;
        ViewBag.Appointments = appointments;
        ViewBag.ClientData = clientData;

        return View("PsychologistDashboard");
    }

    [HttpGet("profile/{id}")]
    public async Task<IActionResult> Profile(int id)
    {
        var psychologist = await _context.Psychologists
            .Include(p => p.Schedules)
            .Include(p => p.TimeSlots)
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
        var availableSlots = await GetAvailableSlots(psychologist.Id);

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

        var userIdInt = int.Parse(userId);

        // Проверяем, доступен ли слот
        var timeSlot = await _context.PsychologistTimeSlots
            .FirstOrDefaultAsync(t => t.PsychologistId == psychologistId && 
                                    t.Date.Date == appointmentDate.Date &&
                                    t.StartTime == appointmentDate.TimeOfDay &&
                                    t.IsAvailable && !t.IsBooked);

        if (timeSlot == null)
        {
            return Json(new { success = false, message = "Выбранное время недоступно" });
        }

        // Бронируем слот
        timeSlot.IsBooked = true;
        timeSlot.BookedByUserId = userIdInt;

        // Создаем запись на консультацию
        var appointment = new PsychologistAppointment
        {
            PsychologistId = psychologistId,
            UserId = userIdInt,
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

    [HttpGet("schedule")]
    public async Task<IActionResult> Schedule()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(userId) || userRole != "psychologist")
        {
            return RedirectToAction("Login", "Auth");
        }

        var psychologist = await _context.Psychologists
            .FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));

        if (psychologist == null)
        {
            return NotFound();
        }

        // Получаем расписание психолога
        var schedules = await _context.PsychologistSchedules
            .Where(s => s.PsychologistId == psychologist.Id)
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.StartTime)
            .ToListAsync();

        // Получаем существующие слоты на ближайшие 2 недели
        var startDate = DateTime.Today;
        var endDate = startDate.AddDays(14);
        
        var existingSlots = await _context.PsychologistTimeSlots
            .Where(t => t.PsychologistId == psychologist.Id && 
                       t.Date >= startDate && 
                       t.Date <= endDate)
            .OrderBy(t => t.Date)
            .ThenBy(t => t.StartTime)
            .ToListAsync();

        ViewBag.Psychologist = psychologist;
        ViewBag.Schedules = schedules;
        ViewBag.ExistingSlots = existingSlots;
        ViewBag.StartDate = startDate;
        ViewBag.EndDate = endDate;

        return View();
    }

    [HttpPost("schedule/add")]
    public async Task<IActionResult> AddSchedule(int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(userId) || userRole != "psychologist")
        {
            return Json(new { success = false, message = "Доступ запрещен" });
        }

        var psychologist = await _context.Psychologists
            .FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));

        if (psychologist == null)
        {
            return Json(new { success = false, message = "Психолог не найден" });
        }

        // Проверяем, нет ли пересечений
        var existingSchedule = await _context.PsychologistSchedules
            .FirstOrDefaultAsync(s => s.PsychologistId == psychologist.Id && 
                                     s.DayOfWeek == (DayOfWeek)dayOfWeek &&
                                     ((s.StartTime <= startTime && s.EndTime > startTime) ||
                                      (s.StartTime < endTime && s.EndTime >= endTime) ||
                                      (s.StartTime >= startTime && s.EndTime <= endTime)));

        if (existingSchedule != null)
        {
            return Json(new { success = false, message = "Время пересекается с существующим расписанием" });
        }

        var schedule = new PsychologistSchedule
        {
            PsychologistId = psychologist.Id,
            DayOfWeek = (DayOfWeek)dayOfWeek,
            StartTime = startTime,
            EndTime = endTime,
            IsAvailable = true,
            CreatedAt = DateTime.Now
        };

        _context.PsychologistSchedules.Add(schedule);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Расписание добавлено!" });
    }

    [HttpPost("schedule/remove")]
    public async Task<IActionResult> RemoveSchedule(int scheduleId)
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(userId) || userRole != "psychologist")
        {
            return Json(new { success = false, message = "Доступ запрещен" });
        }

        var psychologist = await _context.Psychologists
            .FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));

        if (psychologist == null)
        {
            return Json(new { success = false, message = "Психолог не найден" });
        }

        var schedule = await _context.PsychologistSchedules
            .FirstOrDefaultAsync(s => s.Id == scheduleId && s.PsychologistId == psychologist.Id);

        if (schedule == null)
        {
            return Json(new { success = false, message = "Расписание не найдено" });
        }

        _context.PsychologistSchedules.Remove(schedule);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Расписание удалено!" });
    }

    [HttpPost("schedule/generate-slots")]
    public async Task<IActionResult> GenerateSlots()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(userId) || userRole != "psychologist")
        {
            return Json(new { success = false, message = "Доступ запрещен" });
        }

        var psychologist = await _context.Psychologists
            .FirstOrDefaultAsync(p => p.UserId == int.Parse(userId));

        if (psychologist == null)
        {
            return Json(new { success = false, message = "Психолог не найден" });
        }

        var startDate = DateTime.Today.AddDays(1);
        var endDate = startDate.AddDays(14);

        // Получаем расписание
        var schedules = await _context.PsychologistSchedules
            .Where(s => s.PsychologistId == psychologist.Id && s.IsAvailable)
            .ToListAsync();

        // Получаем существующие слоты
        var existingSlots = await _context.PsychologistTimeSlots
            .Where(t => t.PsychologistId == psychologist.Id && 
                       t.Date >= startDate && 
                       t.Date <= endDate)
            .ToListAsync();

        var newSlots = new List<PsychologistTimeSlot>();

        // Генерируем слоты
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
            if (daySchedule != null)
            {
                var currentTime = daySchedule.StartTime;
                while (currentTime < daySchedule.EndTime)
                {
                    var existingSlot = existingSlots.FirstOrDefault(s => 
                        s.Date.Date == date && s.StartTime == currentTime);

                    if (existingSlot == null)
                    {
                        var newSlot = new PsychologistTimeSlot
                        {
                            PsychologistId = psychologist.Id,
                            Date = date,
                            StartTime = currentTime,
                            EndTime = currentTime.Add(TimeSpan.FromHours(1)),
                            IsAvailable = true,
                            IsBooked = false,
                            CreatedAt = DateTime.Now
                        };
                        newSlots.Add(newSlot);
                    }

                    currentTime = currentTime.Add(TimeSpan.FromHours(1));
                }
            }
        }

        _context.PsychologistTimeSlots.AddRange(newSlots);
        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = $"Создано {newSlots.Count} новых слотов!",
            slotsCount = newSlots.Count
        });
    }

    [HttpGet("client/{userId}")]
    public async Task<IActionResult> ClientDetails(int userId)
    {
        var psychologistUserId = HttpContext.Session.GetString("UserId");
        var userRole = HttpContext.Session.GetString("UserRole");
        
        if (string.IsNullOrEmpty(psychologistUserId) || userRole != "psychologist")
        {
            return RedirectToAction("Login", "Auth");
        }

        var psychologist = await _context.Psychologists
            .FirstOrDefaultAsync(p => p.UserId == int.Parse(psychologistUserId));

        if (psychologist == null)
        {
            return NotFound();
        }

        // Проверяем, что клиент действительно записывался к этому психологу
        var hasAppointment = await _context.PsychologistAppointments
            .AnyAsync(a => a.PsychologistId == psychologist.Id && a.UserId == userId);

        if (!hasAppointment)
        {
            return Forbid();
        }

        var client = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (client == null)
        {
            return NotFound();
        }

        // Получаем данные клиента
        var clientNotes = await _context.Notes
            .Where(n => n.UserId == userId && n.ShareWithPsychologist)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        var clientGoals = await _context.Goals
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        var clientEmotions = await _context.EmotionEntries
            .Where(e => e.UserId == userId)
            .OrderByDescending(e => e.Date)
            .ToListAsync();

        var appointments = await _context.PsychologistAppointments
            .Where(a => a.PsychologistId == psychologist.Id && a.UserId == userId)
            .OrderByDescending(a => a.AppointmentDate)
            .ToListAsync();

        ViewBag.Client = client;
        ViewBag.ClientNotes = clientNotes;
        ViewBag.ClientGoals = clientGoals;
        ViewBag.ClientEmotions = clientEmotions;
        ViewBag.Appointments = appointments;
        ViewBag.Psychologist = psychologist;

        return View("ClientDetails");
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

    private async Task<List<PsychologistTimeSlot>> GetAvailableSlots(int psychologistId)
    {
        var startDate = DateTime.Today.AddDays(1);
        var endDate = startDate.AddDays(14);

        // Получаем существующие слоты
        var existingSlots = await _context.PsychologistTimeSlots
            .Where(t => t.PsychologistId == psychologistId && 
                       t.Date >= startDate && 
                       t.Date <= endDate)
            .ToListAsync();

        // Получаем расписание психолога
        var schedules = await _context.PsychologistSchedules
            .Where(s => s.PsychologistId == psychologistId && s.IsAvailable)
            .ToListAsync();

        var slots = new List<PsychologistTimeSlot>();

        // Генерируем слоты на основе расписания
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
            if (daySchedule != null)
            {
                var currentTime = daySchedule.StartTime;
                while (currentTime < daySchedule.EndTime)
                {
                    var slotDateTime = date.Add(currentTime);
                    
                    // Проверяем, не существует ли уже такой слот
                    var existingSlot = existingSlots.FirstOrDefault(s => 
                        s.Date.Date == date && s.StartTime == currentTime);

                    if (existingSlot == null)
                    {
                        // Создаем новый слот
                        var newSlot = new PsychologistTimeSlot
                        {
                            PsychologistId = psychologistId,
                            Date = date,
                            StartTime = currentTime,
                            EndTime = currentTime.Add(TimeSpan.FromHours(1)),
                            IsAvailable = true,
                            IsBooked = false,
                            CreatedAt = DateTime.Now
                        };
                        slots.Add(newSlot);
                    }
                    else
                    {
                        slots.Add(existingSlot);
                    }

                    currentTime = currentTime.Add(TimeSpan.FromHours(1));
                }
            }
        }

        return slots.OrderBy(s => s.Date).ThenBy(s => s.StartTime).ToList();
    }
}

public class UpdateStatusRequest
{
    public int AppointmentId { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class ClientDataViewModel
{
    public User User { get; set; } = null!;
    public List<Note> Notes { get; set; } = new();
    public List<Goal> Goals { get; set; } = new();
    public List<EmotionEntry> Emotions { get; set; } = new();
    public List<PsychologistAppointment> RecentAppointments { get; set; } = new();
}


