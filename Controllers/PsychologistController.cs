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
            .Where(u => u != null)
            .Distinct()
            .ToListAsync();

        // Получаем данные клиентов для анализа
        var clientData = new List<ClientDataViewModel>();
        foreach (var client in clients)
        {
            if (client == null) continue;
            
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
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (psychologist == null)
        {
            return NotFound();
        }

        // Получаем отзывы о психологе (только одобренные и видимые)
        var reviews = await _context.PsychologistReviews
            .Where(r => r.PsychologistId == id && r.IsApproved && r.IsVisible)
            .OrderByDescending(r => r.CreatedAt)
            .Take(10)
            .ToListAsync();

        // Загружаем пользователей для отзывов отдельно, чтобы избежать проблем с Include
        foreach (var review in reviews.Where(r => r.UserId.HasValue))
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == review.UserId!.Value);
            if (user != null)
            {
                review.User = user;
            }
        }

        // Получаем доступные слоты для записи
        var availableSlots = await GetAvailableSlots(psychologist.Id);

        ViewBag.Psychologist = psychologist;
        ViewBag.Reviews = reviews;
        ViewBag.AvailableSlots = availableSlots;

        return View();
    }

    [HttpPost("book")]
    public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentRequest? request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Необходимо войти в систему" });
        }

        if (request == null)
        {
            return Json(new { success = false, message = "Неверные данные запроса" });
        }

        var userIdInt = int.Parse(userId);

        // Парсим дату
        if (!DateTime.TryParse(request.AppointmentDate, out var appointmentDate))
        {
            return Json(new { success = false, message = "Неверный формат даты" });
        }

        // Нормализуем дату (убираем время, оставляем только дату)
        var appointmentDateOnly = appointmentDate.Date;
        var appointmentTime = appointmentDate.TimeOfDay;

        // Проверяем, доступен ли слот
        var timeSlot = await _context.PsychologistTimeSlots
            .FirstOrDefaultAsync(t => t.PsychologistId == request.PsychologistId && 
                                    t.Date.Date == appointmentDateOnly &&
                                    t.StartTime == appointmentTime &&
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
            PsychologistId = request.PsychologistId,
            UserId = userIdInt,
            AppointmentDate = appointmentDateOnly.Add(appointmentTime),
            Notes = request.Notes ?? string.Empty,
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

    [HttpPost("appointments/cancel/{id}")]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Необходимо войти в систему" });
        }

        var appointment = await _context.PsychologistAppointments
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == int.Parse(userId));

        if (appointment == null)
        {
            return Json(new { success = false, message = "Запись не найдена" });
        }

        if (appointment.Status != AppointmentStatus.Scheduled)
        {
            return Json(new { success = false, message = "Невозможно отменить" });
        }

        appointment.Status = AppointmentStatus.Cancelled;
        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    [HttpPost("review")]
    public async Task<IActionResult> AddReview([FromBody] AddReviewRequest request)
    {
        var userId = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "Необходимо войти в систему" });
        }

        var userIdInt = int.Parse(userId);

        // Проверяем, что пользователь действительно записывался к этому психологу
        var hasAppointment = await _context.PsychologistAppointments
            .AnyAsync(a => a.PsychologistId == request.PsychologistId && a.UserId == userIdInt);

        if (!hasAppointment)
        {
            return Json(new { success = false, message = "Вы можете оставить отзыв только после записи на консультацию" });
        }

        var review = new PsychologistReview
        {
            PsychologistId = request.PsychologistId,
            UserId = userIdInt,
            Rating = request.Rating,
            Comment = request.Comment,
            Title = request.Title,
            IsVisible = true,
            IsApproved = false, // Требует одобрения психолога
            CreatedAt = DateTime.Now
        };

        _context.PsychologistReviews.Add(review);
        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = "Отзыв успешно добавлен! Он будет опубликован после одобрения психологом."
        });
    }

    [HttpGet("reviews")]
    public async Task<IActionResult> Reviews()
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

        var reviews = await _context.PsychologistReviews
            .Where(r => r.PsychologistId == psychologist.Id)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        ViewBag.Psychologist = psychologist;
        ViewBag.Reviews = reviews;

        return View();
    }

    [HttpPost("review/{id}/approve")]
    public async Task<IActionResult> ApproveReview(int id)
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

        var review = await _context.PsychologistReviews
            .FirstOrDefaultAsync(r => r.Id == id && r.PsychologistId == psychologist.Id);

        if (review == null)
        {
            return Json(new { success = false, message = "Отзыв не найден" });
        }

        review.IsApproved = true;
        review.IsVisible = true;
        review.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Отзыв одобрен" });
    }

    [HttpPost("review/{id}/reject")]
    public async Task<IActionResult> RejectReview(int id)
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

        var review = await _context.PsychologistReviews
            .FirstOrDefaultAsync(r => r.Id == id && r.PsychologistId == psychologist.Id);

        if (review == null)
        {
            return Json(new { success = false, message = "Отзыв не найден" });
        }

        review.IsApproved = false;
        review.IsVisible = false;
        review.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Отзыв отклонен" });
    }

    [HttpPost("review/{id}/delete")]
    public async Task<IActionResult> DeleteReview(int id)
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

        var review = await _context.PsychologistReviews
            .FirstOrDefaultAsync(r => r.Id == id && r.PsychologistId == psychologist.Id);

        if (review == null)
        {
            return Json(new { success = false, message = "Отзыв не найден" });
        }

        _context.PsychologistReviews.Remove(review);
        await _context.SaveChangesAsync();

        return Json(new { success = true, message = "Отзыв удален" });
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

        // Получаем расписание психолога (без сортировки в SQL)
        var schedules = await _context.PsychologistSchedules
            .Where(s => s.PsychologistId == psychologist.Id)
            .ToListAsync();

        // Сортируем уже в памяти
        schedules = schedules
            .OrderBy(s => s.DayOfWeek)
            .ThenBy(s => s.StartTime)
            .ToList();

        // Получаем существующие слоты на ближайшие 2 недели (без сортировки в SQL)
        var startDate = DateTime.Today;
        var endDate = startDate.AddDays(14);

        var existingSlots = await _context.PsychologistTimeSlots
            .Where(t => t.PsychologistId == psychologist.Id &&
                       t.Date >= startDate &&
                       t.Date <= endDate)
            .ToListAsync();

        // Сортируем уже в памяти
        existingSlots = existingSlots
            .OrderBy(t => t.Date)
            .ThenBy(t => t.StartTime)
            .ToList();

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

    [HttpPost("schedule/add-time-slot")]
    public async Task<IActionResult> AddTimeSlot([FromBody] AddTimeSlotRequest request)
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

        var date = DateTime.Parse(request.Date);
        var startTime = TimeSpan.Parse(request.StartTime);
        var endTime = TimeSpan.Parse(request.EndTime);

        if (endTime <= startTime)
        {
            return Json(new { success = false, message = "Время окончания должно быть позже времени начала" });
        }

        var newSlots = new List<PsychologistTimeSlot>();
        var currentTime = startTime;

        while (currentTime < endTime)
        {
            // Проверяем, не существует ли уже такой слот
            var existingSlot = await _context.PsychologistTimeSlots
                .FirstOrDefaultAsync(s => s.PsychologistId == psychologist.Id && 
                                       s.Date.Date == date.Date && 
                                       s.StartTime == currentTime);

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

        if (newSlots.Any())
        {
            _context.PsychologistTimeSlots.AddRange(newSlots);
            await _context.SaveChangesAsync();
        }

        return Json(new { 
            success = true, 
            message = $"Создано {newSlots.Count} новых слотов!",
            slotsCount = newSlots.Count
        });
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
        for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
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

    [HttpGet("clients")]
    public async Task<IActionResult> Clients()
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

        // Получаем клиентов психолога
        var clients = await _context.PsychologistAppointments
            .Where(a => a.PsychologistId == psychologist.Id)
            .Include(a => a.User)
            .Select(a => a.User)
            .Where(u => u != null)
            .Distinct()
            .ToListAsync();

        // Получаем данные клиентов для анализа
        var clientData = new List<ClientDataViewModel>();
        foreach (var client in clients)
        {
            if (client == null) continue;
            
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

        ViewBag.ClientData = clientData;
        ViewBag.Psychologist = psychologist;

        return View();
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
                       t.Date.Date >= startDate.Date && 
                       t.Date.Date <= endDate.Date)
            .ToListAsync();

        // Получаем расписание психолога
        var schedules = await _context.PsychologistSchedules
            .Where(s => s.PsychologistId == psychologistId && s.IsAvailable)
            .ToListAsync();

        var newSlotsToAdd = new List<PsychologistTimeSlot>();

        // Генерируем слоты на основе расписания, но ограничиваем количество (3-7 слотов)
        var availableSlotsCount = existingSlots.Count(s => s.IsAvailable && !s.IsBooked);
        var maxSlotsToGenerate = Math.Max(0, 7 - availableSlotsCount); // Максимум 7 слотов всего
        
        if (maxSlotsToGenerate > 0)
        {
            var possibleDates = new List<DateTime>();
            for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
                if (daySchedule != null)
                {
                    possibleDates.Add(date);
                }
            }
            
            // Случайно выбираем даты для создания слотов
            var random = new Random();
            var selectedDates = possibleDates.OrderBy(x => random.Next()).Take(maxSlotsToGenerate).ToList();
            
            foreach (var date in selectedDates)
            {
                var daySchedule = schedules.FirstOrDefault(s => s.DayOfWeek == date.DayOfWeek);
                if (daySchedule != null)
                {
                    // Выбираем случайное время в пределах расписания
                    var hoursAvailable = daySchedule.EndTime.Hours - daySchedule.StartTime.Hours;
                    if (hoursAvailable > 0)
                    {
                        var randomHour = random.Next(daySchedule.StartTime.Hours, daySchedule.EndTime.Hours);
                        var startTime = new TimeSpan(randomHour, 0, 0);
                        
                        // Проверяем, не существует ли уже такой слот
                        var existingSlot = existingSlots.FirstOrDefault(s => 
                            s.Date.Date == date && s.StartTime == startTime);

                        if (existingSlot == null)
                        {
                            // Создаем новый слот
                            var newSlot = new PsychologistTimeSlot
                            {
                                PsychologistId = psychologistId,
                                Date = date,
                                StartTime = startTime,
                                EndTime = startTime.Add(TimeSpan.FromHours(1)),
                                IsAvailable = true,
                                IsBooked = false,
                                CreatedAt = DateTime.Now
                            };
                            newSlotsToAdd.Add(newSlot);
                            existingSlots.Add(newSlot); // Добавляем в список, чтобы не создавать дубликаты
                        }
                    }
                }
            }
        }

        // Сохраняем новые слоты в базу данных
        if (newSlotsToAdd.Any())
        {
            _context.PsychologistTimeSlots.AddRange(newSlotsToAdd);
            await _context.SaveChangesAsync();
        }

        // Возвращаем все слоты (существующие + новые)
        return existingSlots
            .Where(s => s.IsAvailable && !s.IsBooked)
            .OrderBy(s => s.Date)
            .ThenBy(s => s.StartTime)
            .ToList();
    }
}

public class BookAppointmentRequest
{
    public int PsychologistId { get; set; }
    public string AppointmentDate { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateStatusRequest
{
    public int AppointmentId { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class AddReviewRequest
{
    public int PsychologistId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? Comment { get; set; }
}

public class AddTimeSlotRequest
{
    public string Date { get; set; } = string.Empty;
    public string StartTime { get; set; } = string.Empty;
    public string EndTime { get; set; } = string.Empty;
}

public class ClientDataViewModel
{
    public User User { get; set; } = null!;
    public List<Note> Notes { get; set; } = new();
    public List<Goal> Goals { get; set; } = new();
    public List<EmotionEntry> Emotions { get; set; } = new();
    public List<PsychologistAppointment> RecentAppointments { get; set; } = new();
}


