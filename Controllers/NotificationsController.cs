using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sofia.Web.Data;
using Sofia.Web.Models;

namespace Sofia.Web.Controllers;

[Route("notifications")]
public class NotificationsController : Controller
{
    private readonly SofiaDbContext _context;

    public NotificationsController(SofiaDbContext context)
    {
        _context = context;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var notifications = await _context.Notifications
            .Where(n => n.IsActive)
            .OrderByDescending(n => n.CreatedAt)
            .Take(50)
            .ToListAsync();

        var unreadCount = await _context.Notifications
            .CountAsync(n => n.IsActive && !n.IsRead);

        ViewBag.Notifications = notifications;
        ViewBag.UnreadCount = unreadCount;

        return View();
    }

    [HttpPost("mark-read/{id}")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }

        return Json(new { success = true });
    }

    [HttpPost("mark-all-read")]
    public async Task<IActionResult> MarkAllAsRead()
    {
        var notifications = await _context.Notifications
            .Where(n => n.IsActive && !n.IsRead)
            .ToListAsync();

        foreach (var notification in notifications)
        {
            notification.IsRead = true;
        }

        await _context.SaveChangesAsync();

        return Json(new { success = true });
    }

    [HttpPost("dismiss/{id}")]
    public async Task<IActionResult> Dismiss(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            notification.IsActive = false;
            await _context.SaveChangesAsync();
        }

        return Json(new { success = true });
    }

    [HttpGet("settings")]
    public async Task<IActionResult> Settings()
    {
        var settings = await _context.NotificationSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new NotificationSettings();
            _context.NotificationSettings.Add(settings);
            await _context.SaveChangesAsync();
        }

        ViewBag.Settings = settings;
        return View();
    }

    [HttpPost("settings")]
    public async Task<IActionResult> UpdateSettings(
        bool dailyReminder, 
        string dailyReminderTime,
        bool goalReminder,
        bool moodCheckReminder,
        string moodCheckTime,
        bool weeklyReport,
        string weeklyReportDay,
        bool practiceReminder,
        bool psychologistReminder,
        bool emailNotifications,
        bool pushNotifications)
    {
        var settings = await _context.NotificationSettings.FirstOrDefaultAsync();
        if (settings == null)
        {
            settings = new NotificationSettings();
            _context.NotificationSettings.Add(settings);
        }

        settings.DailyReminder = dailyReminder;
        settings.DailyReminderTime = TimeSpan.Parse(dailyReminderTime);
        settings.GoalReminder = goalReminder;
        settings.MoodCheckReminder = moodCheckReminder;
        settings.MoodCheckTime = TimeSpan.Parse(moodCheckTime);
        settings.WeeklyReport = weeklyReport;
        settings.WeeklyReportDay = Enum.Parse<DayOfWeek>(weeklyReportDay);
        settings.PracticeReminder = practiceReminder;
        settings.PsychologistReminder = psychologistReminder;
        settings.EmailNotifications = emailNotifications;
        settings.PushNotifications = pushNotifications;
        settings.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = "Настройки уведомлений обновлены!" 
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateNotification(
        string title,
        string message,
        NotificationType type,
        NotificationPriority priority = NotificationPriority.Medium,
        DateTime? scheduledAt = null)
    {
        var notification = new Notification
        {
            Title = title,
            Message = message,
            Type = type,
            Priority = priority,
            ScheduledAt = scheduledAt,
            CreatedAt = DateTime.Now
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = "Уведомление создано!",
            notificationId = notification.Id
        });
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckNotifications()
    {
        var now = DateTime.Now;
        
        // Проверяем настройки уведомлений
        var settings = await _context.NotificationSettings.FirstOrDefaultAsync();
        if (settings == null) return Json(new { notifications = new List<object>() });

        var notifications = new List<object>();

        // Ежедневное напоминание
        if (settings.DailyReminder && now.TimeOfDay >= settings.DailyReminderTime.Add(TimeSpan.FromMinutes(-5)) && 
            now.TimeOfDay <= settings.DailyReminderTime.Add(TimeSpan.FromMinutes(5)))
        {
            var lastNote = await _context.Notes
                .Where(n => n.CreatedAt.Date == now.Date)
                .FirstOrDefaultAsync();

            if (lastNote == null)
            {
                notifications.Add(new
                {
                    type = "daily_reminder",
                    title = "📝 Время для записи!",
                    message = "Как прошел ваш день? Поделитесь своими мыслями и эмоциями.",
                    priority = "medium",
                    actionUrl = "/notes/create",
                    actionText = "Создать заметку"
                });
            }
        }

        // Проверка настроения
        if (settings.MoodCheckReminder && now.TimeOfDay >= settings.MoodCheckTime.Add(TimeSpan.FromMinutes(-5)) && 
            now.TimeOfDay <= settings.MoodCheckTime.Add(TimeSpan.FromMinutes(5)))
        {
            var lastMoodCheck = await _context.Notes
                .Where(n => n.CreatedAt.Date == now.Date)
                .FirstOrDefaultAsync();

            if (lastMoodCheck == null)
            {
                notifications.Add(new
                {
                    type = "mood_check",
                    title = "😊 Как ваше настроение?",
                    message = "Проверьте свое эмоциональное состояние и поделитесь им.",
                    priority = "high",
                    actionUrl = "/notes/create",
                    actionText = "Записать настроение"
                });
            }
        }

        // Напоминания о целях
        if (settings.GoalReminder)
        {
            var activeGoals = await _context.Goals
                .Where(g => g.Status == GoalStatus.Active && g.Progress < 100)
                .ToListAsync();

            foreach (var goal in activeGoals)
            {
                var lastUpdate = await _context.Notes
                    .Where(n => n.CreatedAt >= goal.CreatedAt && n.Content.Contains(goal.Title))
                    .OrderByDescending(n => n.CreatedAt)
                    .FirstOrDefaultAsync();

                if (lastUpdate == null || lastUpdate.CreatedAt < now.AddDays(-3))
                {
                    notifications.Add(new
                    {
                        type = "goal_reminder",
                        title = $"🎯 Цель: {goal.Title}",
                        message = $"Прогресс: {goal.Progress}%. Не забывайте работать над достижением цели!",
                        priority = "medium",
                        actionUrl = "/goals",
                        actionText = "Посмотреть цели"
                    });
                    break; // Показываем только одну цель за раз
                }
            }
        }

        // Напоминания о практиках
        if (settings.PracticeReminder)
        {
            var lastPractice = await _context.Notes
                .Where(n => n.CreatedAt >= now.AddDays(-2) && !string.IsNullOrEmpty(n.Activity))
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefaultAsync();

            if (lastPractice == null)
            {
                notifications.Add(new
                {
                    type = "practice_reminder",
                    title = "🧘 Время для практики!",
                    message = "Попробуйте одну из техник релаксации или медитации.",
                    priority = "low",
                    actionUrl = "/practices",
                    actionText = "Выбрать практику"
                });
            }
        }

        return Json(new { notifications });
    }

    [HttpPost("test")]
    public async Task<IActionResult> SendTestNotification()
    {
        var notification = new Notification
        {
            Title = "🧪 Тестовое уведомление",
            Message = "Это тестовое уведомление для проверки системы.",
            Type = NotificationType.System,
            Priority = NotificationPriority.Medium,
            CreatedAt = DateTime.Now
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();

        return Json(new { 
            success = true, 
            message = "Тестовое уведомление отправлено!",
            notificationId = notification.Id
        });
    }
}
